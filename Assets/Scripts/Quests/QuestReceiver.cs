using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestReceiver : MonoBehaviour {
    public string questTag;
    public GameObject[] awards;
    public QuestSystem questSystem;
    public DialogSystem dialogSystem;
    void Start() {
        //questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
    }
    public void FinishQuest() {
        if (questSystem.GetByTag(questTag) == null || questSystem.GetByTag(questTag).IsFinished) {
            return;
        }
        questSystem.GetByTag(questTag).IsFinished = true;
        //Debug.Log("Шкаф открыт");
        //////////////////////////////////////////
        if (this.transform.parent.name == "FlatWardrobe" && questTag == "Clothing") {
            dialogSystem.Show("01Rat.txt");
            GameObject.Find("Rat").GetComponent<VisibleScript>().Enable();
            questSystem.Add(new CatFishScripts.Quest("RatKill", null, "Убейте крысу"));
        }
        if (this.transform.parent.name == "Rat" && questTag == "RatKill") {
            dialogSystem.Show("01RatKill.txt");
            GameObject.Find("FlatDoor").GetComponent<DoorScript>().isAccessible = true;
            questSystem.Add(new CatFishScripts.Quest("BuyNewClothes", null, "Купите новую одежду"));

            GameObject.Find("Saving").GetComponent<SaveManager>().Save();
        }
        if (this.transform.parent.name == "MagicClothes" && questTag == "BuyNewClothes") {
            questSystem.Add(new CatFishScripts.Quest("MoveToVillage", null, "???"));
            GameObject.Find("Saving").GetComponent<SaveManager>().Save();
            SceneManager.LoadScene("03-Castle");
        }
        if (this.transform.parent.name == "bool" && questTag == "BullQuest") {
            questSystem.Add(new CatFishScripts.Quest("BullKilledQuest", null, "Поговорите со старейшиной"));

            GameObject.Find("Saving").GetComponent<SaveManager>().Save();
        }
        
        if (this.transform.parent.name == "OldMan" && questTag == "OldManQuest") {
            GameObject.Find("ExitBlocker").GetComponent<ExitBlockerScript>().isOpened = true;

        }
        if (this.transform.parent.name == "Well" && questTag == "SaintWater") {
            questSystem.Add(new CatFishScripts.Quest("SaintWaterTell", null, "Поговорите с диаконом"));
        }

        if (this.transform.parent.name == "SaintBrother" && questTag == "SaintWaterTell") {
            GameObject saint = this.transform.parent.gameObject;
            GameObject.Find("Player").GetComponent<UnityCharacter>().satellites.Add(this.awards[1].GetComponent<UnityCharacter>());
            saint.SetActive(false);
        }
        /////////////////////////////////////
        if (awards != null) {
            foreach (var award in awards) {
                if (award != null) {
                    if (!award.activeSelf)
                        award.SetActive(true);
                    award.GetComponent<VisibleScript>().Enable();
                }
            }
        }
        questSystem.ActiveQuests--;
        if (questSystem.demonstrator.gameObject.activeSelf) {
            questSystem.demonstrator.Hide();
            questSystem.demonstrator.Show();
        }
    }
}

