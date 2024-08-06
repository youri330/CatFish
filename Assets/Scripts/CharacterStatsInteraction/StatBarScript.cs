using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class StatBarScript : MonoBehaviour {
    public GameObject player;
    private Character character;
    private Magician magician;
    private Image ManaFilling;
    private Image HpFilling;

    // Start is called before the first frame update
    void Start() {
        character = player.GetComponent<UnityCharacter>().Character;
        HpFilling = this.transform.Find("Hp").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update() {
        HpFilling.fillAmount = character.Hp / character.MaxHp;

    }
}
