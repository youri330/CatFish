using UnityEngine;
using UnityEngine.UI;

public class CastActiveArtifact : MonoBehaviour {
    UnityCharacter activeCharacter;
    ChangeMessage message;
    void Start() {
        activeCharacter = this.transform.parent.parent.parent.Find("CharacterIcon").
            GetComponent<ActivePlayer>().player.GetComponent<UnityCharacter>();
        message = this.transform.parent.parent.parent.Find("MessageBox").GetComponent<ChangeMessage>();
        this.GetComponent<Button>().onClick.AddListener(Cast);
    }
    void Cast() {
        if (activeCharacter.selectedArtifact != null) {
            activeCharacter.isArifactSelected = !activeCharacter.isArifactSelected;
            
            if (this.GetComponent<Image>() != null && activeCharacter.isArifactSelected) {
                // message.Show("Вы взяли артефакт в руки!");
                this.GetComponent<Image>().color = Color.red;
            } else {
                //  message.Show("Вы положили артефакт обратно в сумку");
                this.GetComponent<Image>().color = Color.white;
            }
        }
    }

}
