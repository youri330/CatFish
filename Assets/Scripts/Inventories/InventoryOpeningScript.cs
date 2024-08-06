using UnityEngine;
using UnityEngine.UI;
public class InventoryOpeningScript : MonoBehaviour {
    private Button button;
    public UnityInventory inventory;
    public UnitySpellbook spellbook;

    private void Awake() {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        inventory = this.transform.parent.parent.Find("Inventory").GetComponent<UnityInventory>();
    }
    void TaskOnClick() {
        if (inventory.gameObject.activeSelf) {
            inventory.Hide();
        } else {
            spellbook.Hide();
            inventory.Show();
        }
    }
}
