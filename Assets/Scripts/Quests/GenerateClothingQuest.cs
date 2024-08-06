using UnityEngine;

public class GenerateClothingQuest : MonoBehaviour {
    QuestSystem questSystem;
    public DialogSystem dialogSystem;
    // Start is called before the first frame update
    void Start() {
        dialogSystem.Show("01Awake.txt");
        questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
        questSystem.Add(new CatFishScripts.Quest("Clothing", null, "Одеться на важную встречу."));
    }
}
