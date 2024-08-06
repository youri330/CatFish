using CatFishScripts.Artifacts;
using CatFishScripts.Characters;
using CatFishScripts.Spells;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityCharacter : MonoBehaviour {
    public bool isMagician;
    public string playerName, race, gender;
    public uint age, maxHp, hp, xp, mana, maxMana;
    public bool isTalkable, isMovable;
    public Artifact selectedArtifact = null;
    public Spell selectedSpell = null;
    public uint selectedArtifactIndex, selectedSpellIndex;
    public bool isArifactSelected = false;
    public bool isSpellSelected = false;
    public Artifact isToBeRegiven = null;
    public bool isRegiving = false;
    public int RegivingIndex = 0;
    public List<UnityCharacter> satellites;
    public Character Character {
        get;
        private set;
    }
    void Awake() {
        if (isMagician) {
            Character = new Magician(playerName,
               (Character.RaceType)Enum.Parse(typeof(Character.RaceType), race, true),
               (Character.GenderType)Enum.Parse(typeof(Character.GenderType), gender, true),
               age, maxHp, hp, mana, maxMana, xp, isTalkable, isMovable);
        } else {
            Character = new Character(playerName,
                (Character.RaceType)Enum.Parse(typeof(Character.RaceType), race, true),
                (Character.GenderType)Enum.Parse(typeof(Character.GenderType), gender, true),
                age, maxHp, hp, xp, isTalkable, isMovable);
        }
        this.Character.gameObject = this.gameObject;
    }
    public void Attack(List<UnityCharacter> players) {
        if (this.tag != "Enemy") {
            throw new Exception("Attack can be used only for enemies");
        }
        if (this.gameObject.name == "Rat") {
            var index = new System.Random().Next(0, players.Count);

            players[index].Character.Hp -= 1;
        }
        if (this.gameObject.name == "King") {
            var index = new System.Random().Next(0, players.Count);

            players[index].Character.Hp -= (uint)new System.Random().Next(0, 6);
        }
        if (this.gameObject.name == "bool" || this.Character.Race == Character.RaceType.goblin)
        {
            var index = new System.Random().Next(0, players.Count);

            players[index].Character.Hp -= (uint)new System.Random().Next(0, 9);
        }
        if (this.gameObject.name == "catdragon" || this.gameObject.name == "Knight" || this.gameObject.name == "Knight (1)") {
            var index = new System.Random().Next(0, players.Count);

            players[index].Character.Hp -= (uint)new System.Random().Next(0, 11);
        }
        if (this.gameObject.name == "orc") {
            var index = new System.Random().Next(0, players.Count);
            players[index].Character.Condition = Character.ConditionType.poisoned;
            players[index].Character.Hp -= (uint)new System.Random().Next(0, 13);
        }
        if (this.gameObject.name == "warrior") {
            var index = new System.Random().Next(0, players.Count);

            players[index].Character.Hp -= (uint)new System.Random().Next(0, 15);
        }
    }
}
