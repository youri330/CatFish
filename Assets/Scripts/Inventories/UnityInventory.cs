using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class UnityInventory : MonoBehaviour {
    public Character owner;
    private Sprite basicCell;
    private void Start() {
        basicCell = this.transform.Find("Slot (0)").gameObject.GetComponent<Image>().sprite;
    }
    public void Show() {
        this.gameObject.SetActive(true);
    }
    public void Update() {
        if (this.gameObject.activeSelf) {

            GameObject slot;
            for (int i = 0; i < owner.Inventory.Artifacts.Count; i++) {
                slot = this.transform.Find("Slot (" + i.ToString() + ")").gameObject;
                slot.GetComponent<Image>().sprite = owner.Inventory.Artifacts[i].ArtifactSprite;
                if (owner.Inventory.Artifacts[i].HasPower) {
                    slot.transform.Find("PowerInfo").GetComponent<Text>().text = owner.Inventory.Artifacts[i].Power.ToString();
                } else {
                    slot.transform.Find("PowerInfo").GetComponent<Text>().text = "";
                }
            }
            for (int i = owner.Inventory.Artifacts.Count; i < 25; i++) {
                slot = this.transform.Find("Slot (" + i.ToString() + ")").gameObject;
                slot.GetComponent<Image>().sprite = basicCell;

                slot.transform.Find("PowerInfo").GetComponent<Text>().text = "";
            }
        }
    }
    public void Hide() {
        this.gameObject.SetActive(false);

        this.transform.parent.Find("InventoryDescriptor").GetComponent<ItemDescriptionScript>().Hide();
    }

}
