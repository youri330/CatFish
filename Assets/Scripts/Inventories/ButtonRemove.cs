using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;


public class ButtonRemove : MonoBehaviour {
    private UnityCharacter owner;
    private ItemDescriptionScript descriptor;
    private void Awake() {
        owner = this.transform.parent.parent.Find("CharacterIcon").
            GetComponent<ActivePlayer>().player.GetComponent<UnityCharacter>();
        this.GetComponent<Button>().onClick.AddListener(Remove);
        descriptor = transform.parent.GetComponent<ItemDescriptionScript>();
    }

    private void Remove() {
        if (descriptor.slotNumber == null)
            return;
        if (descriptor.isAboutSpell) {
            owner.selectedSpell = null;
            this.transform.parent.parent.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>().sprite = null;
            (owner.Character as Magician).SpellsList.RemoveSpell((int)descriptor.slotNumber);
            owner.isSpellSelected = false;
            owner.selectedSpellIndex = 0;
            var spellButton =
                GameObject.Find("Canvas").transform.transform.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>();
            spellButton.sprite = null;
            spellButton.color = Color.white;

        } else {
            owner.selectedArtifact = null;
            this.transform.parent.parent.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>().sprite = null;
            owner.Character.Inventory.RemoveArtifact((int)descriptor.slotNumber);
            owner.isArifactSelected = false;
            owner.selectedArtifactIndex = 0;
            var artifactButton =
                GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>();
            artifactButton.sprite = null;
            artifactButton.color = Color.white;
        }
        descriptor.slotNumber = null;
        descriptor.Hide();
    }

}