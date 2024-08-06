using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatFishScripts.Characters;
using System;

public class ButtonRegive : MonoBehaviour {
    private UnityCharacter owner;
    private ItemDescriptionScript descriptor;
    public CastActiveArtifact itemSlot;
    public CastActiveSpell spellSlot;
    private void Awake() {
        owner = this.transform.parent.parent.Find("CharacterIcon").
            GetComponent<ActivePlayer>().player.GetComponent<UnityCharacter>();
        this.GetComponent<Button>().onClick.AddListener(Regive);
        descriptor = transform.parent.GetComponent<ItemDescriptionScript>();
    }

    private void Regive() {
        if (descriptor.isAboutSpell) {
            // GameObject.Find("DialogSystem").GetComponent<DialogSystem>().Show("");
        } else {
            owner.isToBeRegiven = owner.Character.Inventory.Artifacts[(int)descriptor.slotNumber];
            owner.isRegiving = true;
            owner.RegivingIndex = (int)descriptor.slotNumber;
            descriptor.Hide();
        }
    }

}
