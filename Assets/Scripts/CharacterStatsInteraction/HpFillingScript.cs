using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class HpFillingScript : MonoBehaviour {
    public GameObject player;
    private Character character;
    private Image HpFilling;
    private Text HpText;
    private bool isGeneral;
    void Start() {

        isGeneral = this.transform.parent.parent.parent.name == "Canvas";
        character = player.GetComponent<UnityCharacter>().Character;
        HpFilling = this.GetComponent<Image>();
        try {
            HpText = this.transform.Find("Text").GetComponent<Text>();
        } catch {

        }
    }

    public void ChangePlayer(GameObject newPlayer) {
        player = newPlayer;
        character = player.GetComponent<UnityCharacter>().Character;

    }

    // Update is called once per frame
    void Update() {
        HpFilling.fillAmount = ((float)character.Hp) / character.MaxHp;
        if (HpText != null) {
            HpText.text = character.Hp.ToString() + " / " + character.MaxHp.ToString();
        }
        if (character.Condition == Character.ConditionType.poisoned) {
            this.GetComponent<Image>().color = Color.green;
        } else {
            this.GetComponent<Image>().color = Color.red;
        }
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
