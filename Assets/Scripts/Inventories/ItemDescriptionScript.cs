using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionScript : MonoBehaviour {
    Text itemName, itemDescription, itemPower;
    GameObject btRegive;
    public bool isAboutSpell = false;
    public int? slotNumber;
    private void Awake() {
        itemName = this.transform.Find("ItemName").GetComponent<Text>();
        itemDescription = this.transform.Find("ItemDescription").GetComponent<Text>();
        itemPower = this.transform.Find("ItemPower").GetComponent<Text>();
        btRegive = this.transform.Find("ButtonRegive").gameObject;
    }
    public void Show(string name, string description, string power) {

        this.gameObject.SetActive(true);
        btRegive.SetActive(!isAboutSpell);
        itemDescription.text = description;
        itemName.text = name;
        itemPower.text = power;
    }
    public void Show(string name, string description) {
        Show(name, description, "");
    }
    public void Hide() {
        this.gameObject.SetActive(false);
    }


}
