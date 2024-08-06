using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class UnitySpellbook : MonoBehaviour {
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
            if (owner.GetType() != typeof(Magician)) {
                return;
            }
            for (int i = 0; i < (owner as Magician).SpellsList.Spells.Count; i++) {
                slot = this.transform.Find("Slot (" + i.ToString() + ")").gameObject;
                slot.GetComponent<Image>().sprite = (owner as Magician).SpellsList.Spells[i].SpellSprite;
            }
            for (int i = (owner as Magician).SpellsList.Spells.Count; i < 25; i++) {
                slot = this.transform.Find("Slot (" + i.ToString() + ")").gameObject;
                slot.GetComponent<Image>().sprite = basicCell;
            }
        }
    }
    public void Hide() {
        this.gameObject.SetActive(false);

        this.transform.parent.Find("InventoryDescriptor").GetComponent<ItemDescriptionScript>().Hide();
    }

}
