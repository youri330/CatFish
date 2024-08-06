using CatFishScripts.Inventory;
using System;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlotScript : MonoBehaviour {
    ItemDescriptionScript descriptor;
    Inventory inventory;
    void Awake() {
        this.GetComponent<Button>().onClick.AddListener(ShowDescription);
        descriptor = this.transform.parent.parent.Find("InventoryDescriptor").GetComponent<ItemDescriptionScript>();
    }
    public void ShowDescription() {
        if (descriptor.gameObject.activeSelf) {
            descriptor.Hide();
        } else {
            descriptor.isAboutSpell = false;
            int index = GetSlotNumber();
            descriptor.slotNumber = index;
            inventory = this.transform.parent.GetComponent<UnityInventory>().owner.Inventory;

            if (index < inventory.Artifacts.Count) {
                if (inventory.Artifacts[index].HasPower) {
                    descriptor.Show(inventory.Artifacts[index].Name, inventory.Artifacts[index].Description,
                       "Мощность: " + inventory.Artifacts[index].Power.ToString());
                } else {
                    descriptor.Show(inventory.Artifacts[index].Name, inventory.Artifacts[index].Description);
                }
            }
        }
    }
    private int GetSlotNumber() {
        return Int32.Parse(this.name.Split('(', ')')[1]);
    }
}
