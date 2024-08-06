using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DoorOpenerScript : MonoBehaviour {
    public Image messageBox;
    private ChangeMessage message;
    public GameObject DialogSystemObject;
    public FightSystem fightSystem;
    public SaveManager saveManager;
    private DialogSystem dialogSystem;
    private QuestSystem questSystem;

    private void Start() {
        message = messageBox.GetComponent<ChangeMessage>();
        dialogSystem = DialogSystemObject.GetComponent<DialogSystem>();
        questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
        saveManager = GameObject.Find("Saving").GetComponent<SaveManager>();
        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
        dialogSystem.DialogEndedEvent += OnDialogEnded;
    }
    private void OnDialogEnded(object sender, DialogEventArgs args) {
        if (args.dialogFile == "06Mag.txt") {
            saveManager.isGameStarted = false;
            saveManager.Save();
            SceneManager.LoadScene("Menu");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Door") {
            if (!collision.GetComponent<DoorScript>().isOpened) {
                message.Show("Нажмите E, чтобы открыть");
            }
        }
        if (collision.gameObject.tag == "PickUp" || collision.gameObject.tag == "PickUpUnique") {
            message.Show("Нажмите E, чтобы подобрать");
        }
        if (collision.gameObject.name == "MagicClothes") {
            message.Show("Нажмите E, чтобы надеть");
        }
        if (collision.gameObject.tag == "FightZone") {
            fightSystem.StartBattle(collision.transform.parent.GetComponent<UnityCharacter>(), this.transform.parent.GetComponent<UnityCharacter>(),
                collision);
        }
        if (collision.gameObject.tag == "Talkable") {
            message.Show("Нажмите E, чтобы поговорить");
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "PickUpUnique") {
            if (Input.GetKey(KeyCode.E)) {
                if (!CharacterHas(collision)) {
                    PickUp(collision);
                } else {
                    dialogSystem.Show("UniqueTwicePickUp.txt");
                }
            }
        }
        if (collision.gameObject.tag == "Door") {
            if (Input.GetKey(KeyCode.E)) {
                DoorScript door = collision.gameObject.GetComponent<DoorScript>();
                if (door.isAccessible) {
                    door.Open();
                    if (collision.name == "FlatDoor") {
                        saveManager.Save();
                        SceneManager.LoadScene("02-Secondhand");
                    }
                    if (collision.name == "FlatWardrobe") {
                        collision.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        door.isAccessible = false;
                    }
                } else {
                    if (collision.name == "FlatDoor") {
                        dialogSystem.Show("01EarlyDoor.txt");
                    }
                    if (collision.name == "FlatDoor") {
                        dialogSystem.Show("01EarlyDoor.txt");
                    }
                    //                    message.Show("Кажется, мне туда ещё рано...");
                }
            }
        }
        if (collision.gameObject.tag == "PickUp") 
        {
            if (Input.GetKey(KeyCode.E)) {
                PickUp(collision);
            }
        }
        if (collision.gameObject.tag == "PickUpSpell") {
            if (Input.GetKey(KeyCode.E)) {
                PickUpSpell(collision);
            }
        }
        if (collision.gameObject.name == "MagicClothes")
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                collision.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
            }
        }
        if (collision.gameObject.tag == "ExitBlocker") {
            var blocker = collision.GetComponent<ExitBlockerScript>();
            if (blocker.isOpened) {
                if (SceneManager.GetActiveScene().name == "03-Castle") {
                   
                }
                if (SceneManager.GetActiveScene().name == "04-Village") {
                    saveManager.Save();
                    SceneManager.LoadScene("05-Forest");
                }
                if (SceneManager.GetActiveScene().name == "05-Forest") {
                    saveManager.Save();
                    SceneManager.LoadScene("06-Town");
                }
                if (SceneManager.GetActiveScene().name == "06-Town") {
                    saveManager.Save();
                    SceneManager.LoadScene("07-Castle");
                }
            } else {
              //  Debug.Log((dialogSystem == null).ToString()+ "d" + this.gameObject.name);
                GameObject.Find("DialogSystem").GetComponent<DialogSystem>().Show("WrongWay.txt");
            }
        }
        if (collision.gameObject.tag == "Talkable")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(collision.name == "granny") {
                    var quest = questSystem.GetByTag("GrannyQuest");
                    if (quest == null || !quest.IsFinished) { 
                        dialogSystem.Show("03VillageGranny.txt");
                        questSystem.Add(new CatFishScripts.Quest("GrannyQuest", null, "Собрать цветы для зелья"));
                    }
                    else {
                        dialogSystem.Show("03VillageGrannyCompleted.txt");
                    }
                }

                if (collision.name == "OldMan") {
                    var quest = questSystem.GetByTag("BullQuest");
                    var quest2 = questSystem.GetByTag("OldManQuest");
                    collision.transform.Find("???QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                    if (quest == null || quest2 == null) {
                       
                        dialogSystem.Show("03VillageOldMan.txt");
                        questSystem.Add(new CatFishScripts.Quest("BullQuest", null, "Успокоить быка на поле"));
                        questSystem.Add(new CatFishScripts.Quest("OldManQuest", null, "Помочь людям в деревне"));
                    } else {
                        if (questSystem.GetByTag("BullKilledQuest") == null ||
                            questSystem.GetByTag("SaintWaterTell") == null || !questSystem.GetByTag("SaintWaterTell").IsFinished ||
                            questSystem.GetByTag("GrannyQuest") == null || !questSystem.GetByTag("GrannyQuest").IsFinished) {
                            dialogSystem.Show("03VillageOldManCompleting.txt");
                            return;
                        }
                        collision.transform.Find("MainQuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        collision.transform.Find("BullKilledQuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        dialogSystem.Show("03VillageOldManCompleted.txt");
                    }
                }
                if (collision.name == "SaintBrother") {
                    var quest = questSystem.GetByTag("SaintWater");
                    if (quest == null) {
                        dialogSystem.Show("03SaintBrotherStart.txt");
                        questSystem.Add(new CatFishScripts.Quest("SaintWater", null, "Освятите колодец"));
                    } else if (!quest.IsFinished) {
                        dialogSystem.Show("03SaintBrotherCompleting.txt");
                    } else {
                        collision.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        dialogSystem.Show("03SaintBrotherCompleted.txt");
                    }
                }
                if (collision.name == "oldboy") {
                    var quest = questSystem.GetByTag("Mushrooms");
                    if (quest == null) {
                        dialogSystem.Show("04OldBoyStart.txt");
                        questSystem.Add(new CatFishScripts.Quest("Mushrooms", null, "Соберите съедобные грибы"));
                    } else {
                        //collision.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        dialogSystem.Show("04OldBoyFinish.txt");
                    }
                }
                if (collision.name == "goblin") {
                    dialogSystem.Show("04Goblin.txt");
                }

                if (collision.name == "priest") {
                    dialogSystem.Show("03Priest.txt");
                }
                if (collision.name == "gnome" || collision.name == "gnome2") {
                    dialogSystem.Show("05Gnomes.txt");
                }
                if (collision.name == "warrior") {
                    dialogSystem.Show("05Security.txt");
                }
                if (collision.name == "warrior") {
                    dialogSystem.Show("05Security.txt");
                }
                if (collision.name == "Mag") {
                    dialogSystem.Show("06Mag.txt");
                }
                if (collision.name == "elf") {
                    var quest = questSystem.GetByTag("ElfQuest");
                    if (quest == null) {
                        dialogSystem.Show("05ElfStart.txt");
                        questSystem.Add(new CatFishScripts.Quest("ElfQuest", null, "Помогите эльфу"));
                    } else 
                    if (GameObject.Find("goblin").GetComponent<UnityCharacter>().Character.Condition== CatFishScripts.Characters.Character.ConditionType.dead
                        && collision.GetComponent<UnityCharacter>().Character.Inventory.Artifacts.Count == 1){
                        collision.transform.Find("QuestReceiver").GetComponent<QuestReceiver>().FinishQuest();
                        dialogSystem.Show("05ElfEnd.txt");
                    } else if (quest.IsFinished) {
                        dialogSystem.Show("05ElfFullEnd.txt");
                    }
                }
            }
        }
    }

    private void PickUp(Collider2D collision) 
    {
        CatFishScripts.Artifacts.Artifact artifact = collision.GetComponent<UnityArtifact>().Behavior;
        var activeObject = GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player;
        var character = activeObject.GetComponent<UnityCharacter>().Character;
        if (this.transform.parent.gameObject.name != activeObject.name) {
            return;
        }
        if (character.Inventory.Artifacts.Count == 25) {
            message.Show("Инвентарь переполнен!!!");
        } else {
            character.Inventory.AddArtifact(artifact);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.enabled = false;
        }
    }
    private void PickUpSpell(Collider2D collision) {
        CatFishScripts.Spells.Spell spell = collision.GetComponent<UnitySpell>().Behavior;
        var activeObject = GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player;

        var character = activeObject.GetComponent<UnityCharacter>().Character;
        if (this.transform.parent.gameObject.name != activeObject.name) {
            return;
        }
        if (character.GetType() != typeof(CatFishScripts.Characters.Magician)) {
            message.Show("Только маги могут изучить заклинания");
            return;
        }
        if (character.Inventory.Artifacts.Count == 25) {
            message.Show("Слишком много заклинаний!");
        } else {
            (character as CatFishScripts.Characters.Magician).SpellsList.AddSpell(spell);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.enabled = false;
        }
    }
    private bool CharacterHas(Collider2D collision) {
        CatFishScripts.Artifacts.Artifact artifact = collision.GetComponent<UnityArtifact>().Behavior;
        CatFishScripts.Characters.Character character =
            GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player.GetComponent<UnityCharacter>().Character;
        bool has = false;
        for (int i = 0; i < character.Inventory.Artifacts.Count; i++) {
            if (character.Inventory.Artifacts[i].Name == artifact.Name) {
                has = true;
            }
        }
        return has;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        //  if (collision.gameObject.tag == "Door") {
        message.Hide();
        // }
        if (collision.gameObject.tag == "FightZone") {
            dialogSystem.Show("GiveUp.txt");
            this.transform.parent.transform.position = collision.transform.position;
        }
    }
    private void Update() {
        if (Input.GetKey(KeyCode.F5)) {
            if (fightSystem.FightStarted) {
                message.Show("Нельзя сохраняться в бою!");
            } else {
                message.Show("Игра сохраняется...");
                saveManager.Save();
                message.Show("Сохранено");
            }
        }
        if (Input.GetKey(KeyCode.F6)) {
            message.Show("Загружается последнее быстрое сохранение...");
            saveManager.Load();
            message.Show("Загружено последнее сохранение");

        }
        if (Input.GetKey(KeyCode.Escape)) {
            saveManager.Save();
            SceneManager.LoadScene("Menu");

        }
    }
}
