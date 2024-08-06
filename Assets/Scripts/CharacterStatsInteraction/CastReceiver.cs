using CatFishScripts.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CastReceiver : MonoBehaviour {
    public Character thisReceiver;
    public UnityCharacter active;
    public UnityInventory unityInventory;
    public UnitySpellbook unitySpellbook;
    public ChangeMessage message;
    public FightSystem fightSystem;
    private DialogSystem dialogSystem;
    private QuestSystem questSystem;

    // Start is called before the first frame update
    void Start() {
        fightSystem = GameObject.Find("FightSystem").GetComponent<FightSystem>();
        thisReceiver = this.GetComponent<UnityCharacter>().Character;
        unityInventory = GameObject.Find("Canvas").transform.Find("Inventory").GetComponent<UnityInventory>();
        unitySpellbook = GameObject.Find("Canvas").transform.Find("SpellBook").GetComponent<UnitySpellbook>();
        message = GameObject.Find("Canvas").transform.Find("MessageBox").GetComponent<ChangeMessage>();
        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
        questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
    }

    public void Cast(int power) {
        var item = unityInventory.transform.Find("Slot (" + active.selectedArtifactIndex.ToString() + ")").GetComponent<ItemSlotScript>();
        if (active.Character.Inventory.Artifacts[(int)active.selectedArtifactIndex].HasPower && power == 0) {
            message.Show("Промах!");
        }
        if (active.Character.Inventory.Artifacts[(int)active.selectedArtifactIndex].HasPower &&
            power == Math.Min((int)active.Character.Inventory.Artifacts[(int)active.selectedArtifactIndex].Power, (int)thisReceiver.MaxHp)) {
            message.Show("Критический урон!");
        }
        uint hpBefore = thisReceiver.Hp;
        active.Character.Inventory.ActivateArtifact((int)active.selectedArtifactIndex, thisReceiver, (uint)power);
        uint hpAfter = thisReceiver.Hp;
        if (hpAfter == hpBefore && active.Character.Inventory.Artifacts[(int)active.selectedArtifactIndex].HasPower) {
            message.Show("Промах!");
        }
        if (hpAfter == 0 && hpBefore != 0) {
            message.Show("Смертельный урон!");
        }
        item.ShowDescription();
        item.ShowDescription();
    }
    public void CastSpell(int power) {
        var item = unitySpellbook.transform.Find("Slot (" + active.selectedSpellIndex.ToString() + ")").GetComponent<SpellSlotScript>();
        (active.Character as Magician).SpellsList.CastSpell((int)active.selectedArtifactIndex, thisReceiver, (uint)power);
        item.ShowDescription();
        item.ShowDescription();
    }
    void OnMouseDown() {
        var activePlayerScript = GameObject.Find("CharacterIcon").GetComponent<ActivePlayer>();
        active = activePlayerScript.player.GetComponent<UnityCharacter>();
        if (active.isRegiving) {
            if (this.gameObject.tag != "Player") {
                message.Show("Передавать вещи можно только своим");
            } else {
                active.Character.Inventory.Artifacts.Remove(active.isToBeRegiven);
                thisReceiver.Inventory.Artifacts.Add(active.isToBeRegiven);
            }
            active.isToBeRegiven = null;
            active.isRegiving = false;
            return;
        }
        if (this.gameObject.tag == "Player" && !active.isSpellSelected && !active.isArifactSelected) {
            if (active.isRegiving) {

            } else {
                activePlayerScript.ChangePlayer(this.gameObject);
            }
            return;
        }

        if (active.isArifactSelected) {
            var artifact = active.Character.Inventory.Artifacts[(int)active.selectedArtifactIndex];

            if (active.isArifactSelected && artifact.IsFightOnly && !fightSystem.FightStarted) {
                message.Show("Не к чему бороться в мирное время");
                return;
            }
            if (active.isArifactSelected && artifact.Used) {
                message.Show("Нельзя использовать один артефакт за ход дважды");
                return;
            }
            if (artifact.IsFightOnly) {
                artifact.Used = true;
            }
            var recepient = this.GetComponent<UnityCharacter>().Character;
            if (this.tag == "Enemy" || this.tag == "Player") {
                bool isForOneTime = artifact.IsRechargeable;
                if (artifact.HasPower) {
                    //active.Character.Inventory.ActivateArtifact((int)active.selectedArtifactIndex, thisReceiver, 3);
                    System.Random random = new System.Random();
                    try {
                        Cast(random.Next(0, Math.Min((int)artifact.Power, (int)thisReceiver.MaxHp)) + 1);
                    } catch {

                    }
                } else {
                    try {
                        Cast(1);
                    } catch { }
                }
                // if (isForOneTime) 
                {
                    active.isArifactSelected = false;
                    active.selectedArtifactIndex = 0;
                    var artifactButton =
                        GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>();
                    artifactButton.sprite = null;
                    artifactButton.color = Color.white;
                }
            } else {
                if (this.gameObject.name == "granny") {
                    if (artifact.Name == "Цветочки") {

                        if (questSystem.GetByTag("GrannyQuest") != null &&
                           !questSystem.GetByTag("GrannyQuest").IsFinished) {
                            active.Character.Inventory.ExchangeArtifact(recepient, (int)active.selectedArtifactIndex);
                            dialogSystem.Show("03VillageGrannyCompleting.txt");
                            this.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        } else {
                            dialogSystem.Show("03VillageGrannyBase.txt");
                            return;
                        }
                    } else {
                        dialogSystem.Show("03VillageGrannyDontWant.txt");
                        return;
                    }
                } else
                if (this.gameObject.name == "Well" && questSystem.GetByTag("SaintWater") != null &&
                    !questSystem.GetByTag("SaintWater").IsFinished) {
                    if (artifact.GetType() == typeof(CatFishScripts.Artifacts.LivingWaterBottle)) {
                        active.Character.Inventory.ExchangeArtifact(recepient, (int)active.selectedArtifactIndex);
                        message.Show("Вы освятили колодец");
                        this.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                    }
                } else
                 if (this.gameObject.name == "oldboy") {
                    if (artifact.Name == "Грибы") {
                        if (questSystem.GetByTag("Mushrooms") != null &&
                           !questSystem.GetByTag("Mushrooms").IsFinished) {
                            int i = 0;
                            while (i < active.Character.Inventory.Artifacts.Count) {
                                if (active.Character.Inventory.Artifacts[i].Name == "Грибы") {
                                    active.Character.Inventory.ExchangeArtifact(recepient, i);
                                    i--;
                                }
                                i++;
                            }
                            if (recepient.Inventory.Artifacts.Count == 8) {
                                dialogSystem.Show("04OldBoyFinish.txt");
                                this.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                            }
                        } else {
                            dialogSystem.Show("04OldBoyCompleting.txt");
                            return;
                        }
                    } else {
                        dialogSystem.Show("04OldBoyCompleting.txt");
                        return;
                    }

                } else if (this.gameObject.name == "elf") {
                    if (artifact.GetType() == typeof(CatFishScripts.Artifacts.BasiliskEye)) {
                        if (questSystem.GetByTag("ElfQuest") != null &&
                           !questSystem.GetByTag("ElfQuest").IsFinished) {
                            active.Character.Inventory.ExchangeArtifact(recepient, (int)active.selectedArtifactIndex);
                        }
                    } else {
                        return;
                    }
                }
                active.isArifactSelected = false;
                active.selectedArtifactIndex = 0;
                var artifactButton =
                    GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>();
                artifactButton.sprite = null;
                artifactButton.color = Color.white;
            }
        }
        if (active.isSpellSelected) {
            var spell = (active.Character as Magician).SpellsList.Spells[(int)active.selectedSpellIndex];
            try {
                CastSpell(spell.Power);
            } catch (Exception ex) {
                message.Show(ex.Message);
            }
            active.isSpellSelected = false;
            active.selectedSpellIndex = 0;
            var spellButton =
                GameObject.Find("Canvas").transform.transform.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>();
            spellButton.sprite = null;
            spellButton.color = Color.white;
        }
    }
}