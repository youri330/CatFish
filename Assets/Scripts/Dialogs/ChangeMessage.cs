using UnityEngine;
using UnityEngine.UI;

public class ChangeMessage : MonoBehaviour {
    private Text textComponent;
    private void Awake() {
        textComponent = this.transform.Find("Message").GetComponent<Text>();
    }
    public void Show(string msg) {
        this.gameObject.SetActive(true);
        textComponent.text = msg;
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}
