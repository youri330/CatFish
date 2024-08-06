using UnityEngine;
using UnityEngine.UI;
public class SpellbookOpeningScript : MonoBehaviour {
    private Button button;
    public UnitySpellbook spellbook;
    public UnityInventory inventory;

    private void Awake() {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        spellbook = this.transform.parent.parent.Find("SpellBook").GetComponent<UnitySpellbook>();
    }
    void TaskOnClick() {
        if (spellbook.gameObject.activeSelf) {
            spellbook.Hide();
        } else {
            inventory.Hide();
            spellbook.Show();
        }
    }
}