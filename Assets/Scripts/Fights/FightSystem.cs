using CatFishScripts.Characters;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour {
    public ChangeMessage message;
    List<UnityCharacter> satellites;
    List<UnityCharacter> enemies;
    public GameObject deathScreen;
    int turn;
    Collider2D collision;
    public Button endTurn;
    public bool FightStarted {
        get;
        private set;
    }
    private void Awake() {
        FightStarted = false;
    }
    public void StartBattle(UnityCharacter enemy, UnityCharacter player, Collider2D collision) {
        if (FightStarted) {
            return;
        }
        this.FightStarted = true;
        GameObject.Find("Saving").GetComponent<SaveManager>().Save();
        message.Show("Время битвы!");
        this.satellites = player.satellites;
        satellites.Add(player);
        foreach (var satellite in this.satellites) {
            satellite.Character.DeathEvent += OnDeath;
        }
        this.enemies = enemy.satellites;
        enemies.Add(enemy);
        foreach (var satellite in this.enemies) {
            satellite.Character.DeathEvent += OnDeath;
            if (satellite.gameObject.name != enemy.gameObject.name)
                satellite.transform.Find("FightZone").GetComponent<Collider2D>().enabled = false;
        }
        this.collision = collision;
        this.turn = 0;

        Battle();
    }
    public void EndTurn() {
        Battle();
    }
    private void Battle() {
        if (!FightStarted) {
            endTurn.gameObject.SetActive(false);
            return;
        }
        foreach (var enemy in enemies) {
            if (enemy.Character.Condition == Character.ConditionType.poisoned) {
                enemy.Character.Hp -= 2;
            }
        }
        foreach (var satellite in satellites) {
            if (satellite.Character.Condition == Character.ConditionType.poisoned) {
                satellite.Character.Hp -= 2;
            }
        }
        
        turn++;

        endTurn.gameObject.SetActive(false);
        message.Show("Вражеский ход " + turn.ToString());
        index = 0;
        EnemiesLoop();
        
    }
    int index;
    private void EnemiesLoop() {
        if (index < enemies.Count && enemies[index] != null) {
            if (index > 0) {
                enemies[index - 1].GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (enemies[index].Character.Condition != Character.ConditionType.dead) {
                enemies[index].Attack(satellites);
                enemies[index].GetComponent<SpriteRenderer>().color = Color.red;
                Invoke("EnemiesLoop", 1);
            } else {
                Invoke("EnemiesLoop", 0);
            }
            index++;
            return;
        }
        AfterLoop();
    }
    private void AfterLoop() { 
        enemies[enemies.Count - 1].GetComponent<SpriteRenderer>().color = Color.white;
        endTurn.gameObject.SetActive(true);

        foreach (var satellite in satellites) {
            for (int i = 0; i < satellite.Character.Inventory.Artifacts.Count; i++) {
                satellite.Character.Inventory.Artifacts[i].Used = false;
            }
        }
        if (!FightStarted) {
            endTurn.gameObject.SetActive(false);
            return;
        }
        message.Show("Ваш ход " + turn.ToString());
    }
    private void CheckEnemies() {
        bool allAreDead = true;
        foreach (var enemy in enemies) {
            if (enemy.Character.Condition != Character.ConditionType.dead) {
                allAreDead = false;
            }
        }
        if (allAreDead) {

            message.Show("Вы выиграли");
            FinishBattle();
        }
    }
    private void CheckSatellites() {
        bool allAreDead = true;
        foreach (var satellite in satellites) {
            if (satellite.Character.Condition != Character.ConditionType.dead) {
                allAreDead = false;
            }
        }
        if (allAreDead) {
            //redo
            message.Show("Вы проиграли");
        }
    }
    private void OnDeath(object sender, EventArgs args) {
        CheckEnemies();
        CheckSatellites();
        //запустить анимацию
        var move = (sender as Character).gameObject.GetComponent<CharacterScript>();
        if (move != null) {
            move.enabled = false;
        }
        if ((sender as Character).gameObject.tag == "Enemy") {
            if ((sender as Character).gameObject.GetComponent<SpriteRenderer>().sprite.rect.width >=
                (sender as Character).gameObject.GetComponent<SpriteRenderer>().sprite.rect.height) {
                (sender as Character).gameObject.transform.rotation = new Quaternion(0, 0, 180, 1);
            } else {
                (sender as Character).gameObject.transform.rotation = new Quaternion(0, 0, 90, 1);
            }
            (sender as Character).gameObject.transform.position = new Vector3((sender as Character).gameObject.transform.position.x,
                (sender as Character).gameObject.transform.position.y - 0.25f, (sender as Character).gameObject.transform.position.z);
        }
        if ((sender as Character).gameObject.tag == "Player") {
            FightStarted = false;
            (sender as Character).gameObject.GetComponent<UnityCharacter>().isArifactSelected = false;
            (sender as Character).gameObject.GetComponent<UnityCharacter>().selectedArtifactIndex = 0;
            var artifactButton =
                GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>();
            artifactButton.sprite = null;
            artifactButton.color = Color.white;
            (sender as Character).gameObject.GetComponent<UnityCharacter>().isSpellSelected = false;
            (sender as Character).gameObject.GetComponent<UnityCharacter>().selectedSpellIndex = 0;
            var spellButton =
                GameObject.Find("Canvas").transform.transform.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>();
            spellButton.sprite = null;
            spellButton.color = Color.white;
            deathScreen.SetActive(true);
            return;
        }
        var canvas = (sender as Character).gameObject.transform.Find("CharacterCanvas");
        if (canvas != null) {
            canvas.gameObject.SetActive(false);
        }
        var questComponent = (sender as Character).gameObject.transform.Find("QuestReceiver");
        if (questComponent != null) {
            questComponent.GetComponent<QuestReceiver>().FinishQuest();
        }
        var lootComponent = (sender as Character).gameObject.transform.Find("LootGiver");
        if (lootComponent != null) {
            lootComponent.GetComponent<LootGiver>().GiveLoot();
        }
    }
    private void FinishBattle() {
        collision.enabled = false;
        this.FightStarted = false;
        foreach (var player in satellites) {
            player.Character.Condition = Character.ConditionType.healthy;
        }
        EnemiesLoop();

        
        endTurn.gameObject.SetActive(false);
        GameObject.Find("Saving").GetComponent<SaveManager>().Save();
    }

}