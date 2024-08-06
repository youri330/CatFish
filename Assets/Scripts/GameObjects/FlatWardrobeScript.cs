using UnityEngine;

public class FlatWardrobeScript : MonoBehaviour {
    public string questTag;
    public QuestSystem questSystem;
    void Start() {
    }
    public void FinishQuest(string tag) {
        questSystem.GetByTag(tag).IsFinished = true;
        GameObject.Find("Rat").SetActive(true);
    }

}
