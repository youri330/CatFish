using CatFishScripts.Characters;
using CatFishScripts.Inventory;
using System;
using UnityEngine;
using UnityEngine.UI;
public class SpellSlotScript : MonoBehaviour {
    ItemDescriptionScript descriptor;
    SpellsList spellbook;
    void Awake() {
        this.GetComponent<Button>().onClick.AddListener(ShowDescription);
        descriptor = this.transform.parent.parent.Find("InventoryDescriptor").GetComponent<ItemDescriptionScript>();
    }
    public void ShowDescription() {
        if (descriptor.gameObject.activeSelf) {
            descriptor.Hide();
        } else {
            descriptor.isAboutSpell = true;
            int index = GetSlotNumber();
            descriptor.slotNumber = index;
            spellbook = (this.transform.parent.GetComponent<UnitySpellbook>().owner as Magician).SpellsList;
            
            if (index < spellbook.Spells.Count) {
                var spell = spellbook.Spells[index];
                descriptor.Show(spell.Name, spell.Description,
                    (spell.HasPower ? "Мощность: " + spell.Power.ToString() + "\n" : "") +
                    "Стоимость: " + spell.Cost);
            }
        }
    }
    private int GetSlotNumber() {
        return Int32.Parse(this.name.Split('(', ')')[1]);
    }
}
