using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class ManaFillingScript : MonoBehaviour {
    public GameObject player;
    private Character character;
    private Image ManaFilling;
    private Text ManaText;
    private bool isGeneral;

    void Start() {
        isGeneral = this.transform.parent.parent.parent.name == "Canvas";
        ManaFilling = this.GetComponent<Image>();
        try {
            ManaText = this.transform.Find("Text").GetComponent<Text>();
        } catch {

        }
        CheckPlayer();
    }

    public void ChangePlayer(GameObject newPlayer) {
        player = newPlayer;
        CheckPlayer();
    }
    void CheckPlayer() {
        character = player.GetComponent<UnityCharacter>().Character;
        this.transform.parent.gameObject.SetActive(character.GetType() == typeof(Magician));
        //this.transform.parent.gameObject.SetActive(this.gameObject.activeSelf);
    }
    // Update is called once per frame
    void Update() {
        if (this.gameObject.activeSelf) {
            ManaFilling.fillAmount = ((float)(character as Magician).Mana) / (character as Magician).MaxMana;
            if (ManaText != null) {
                ManaText.text = (character as Magician).Mana.ToString() + " / " + (character as Magician).MaxMana.ToString();
            }
        }
     //   Debug.Log(isGeneral.ToString() + player.name);
        
    }
    private void FixedUpdate() {
        if (!this.isGeneral) {
            if (player.transform.localScale.x < 0 && this.transform.localScale.x > 0 ||
                player.transform.localScale.x > 0 && this.transform.localScale.x < 0) {
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y);
            }
        }
    }
}
