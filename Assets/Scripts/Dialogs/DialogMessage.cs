using UnityEngine;
using UnityEngine.UI;
public class DialogMessage : MonoBehaviour {
    private Text textComponent;
    private void Awake() {
        textComponent = this.transform.Find("Message").GetComponent<Text>();
    }


    public void Show(Speech msg) {

        if (!this.gameObject.activeSelf) {
            this.gameObject.SetActive(true);
        }
        textComponent.color = msg.Color;
        textComponent.text = msg.Message;
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}

