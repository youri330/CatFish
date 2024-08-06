using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class ButtonCast : MonoBehaviour {
    private UnityCharacter owner;
    private ItemDescriptionScript descriptor;
    public CastActiveArtifact itemSlot;
    public CastActiveSpell spellSlot;
    private void Awake() {        
        this.GetComponent<Button>().onClick.AddListener(Cast);
        descriptor = transform.parent.GetComponent<ItemDescriptionScript>();
    }

    private void Cast() {
        owner = this.transform.parent.parent.Find("CharacterIcon").
            GetComponent<ActivePlayer>().player.GetComponent<UnityCharacter>();
        if (descriptor.isAboutSpell) {
            owner.isSpellSelected = true;
            owner.selectedSpellIndex = (uint) descriptor.slotNumber;
            spellSlot.GetComponent<Image>().color = Color.red;
            owner.selectedSpell = (owner.Character as Magician).SpellsList.Spells[(int)descriptor.slotNumber];
            this.transform.parent.parent.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>().sprite =
                this.transform.parent.parent.Find("SpellBook").Find("Slot (" + descriptor.slotNumber + ")").GetComponent<Image>().sprite;
            
        } else {
            owner.selectedArtifact = owner.Character.Inventory.Artifacts[(int)descriptor.slotNumber];
            owner.isArifactSelected = true;
            itemSlot.GetComponent<Image>().color = Color.red;
            owner.selectedArtifactIndex = (uint) descriptor.slotNumber;
            this.transform.parent.parent.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>().sprite =
                this.transform.parent.parent.Find("Inventory").Find("Slot (" + descriptor.slotNumber + ")").GetComponent<Image>().sprite;
        }
        descriptor.Hide();
    }

}
