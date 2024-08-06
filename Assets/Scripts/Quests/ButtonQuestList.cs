using UnityEngine;
using UnityEngine.UI;
public class ButtonQuestList : MonoBehaviour {
    public QuestListDemonstrator demonstrator;
    // Start is called before the first frame update
    void Start() {
        this.GetComponent<Button>().onClick.AddListener(ShowHide);
    }
    void ShowHide() {
        if (demonstrator.gameObject.activeSelf) {
            demonstrator.Hide();
        } else {
            demonstrator.Show();
        }
    }
}
