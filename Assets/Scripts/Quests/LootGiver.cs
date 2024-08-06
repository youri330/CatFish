using UnityEngine;
using UnityEngine.SceneManagement;

public class LootGiver : MonoBehaviour {
    public GameObject[] awards;
    public DialogSystem dialogSystem;
    void Start() {
        //questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
    }
    public void GiveLoot() {
       
        foreach (var award in awards) {
            if (award != null) {
                //            award.SetActive(true);
                award.GetComponent<VisibleScript>().Enable();
            }
        }

    }
}

