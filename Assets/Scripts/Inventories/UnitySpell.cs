using CatFishScripts.Spells;
using UnityEngine;
using System.Collections.Generic;
public class UnitySpell : MonoBehaviour {
    public string spellType;
    public string spellName, description;
    public int power;
    public Sprite spellSprite;
    public Spell Behavior {
        get; set;
    }
    public bool isFightOnly;
    public bool used = false;
    private void Awake() {
        Behavior = StringToSpell(spellType);
        Behavior.Name = spellName;
        Behavior.Description = description;
        Behavior.SpellSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        Behavior.unityShell = this;
        Behavior.IsFightOnly = isFightOnly;
        Behavior.Power = power;
        Behavior.Used = used;
    }
    public static Dictionary<string, string> TypeToString = new Dictionary<string, string>() {
        {typeof(AddHealth).ToString(), "AddHealth" },
        {typeof(Antidote).ToString(), "Antidote" },
        {typeof(Armor).ToString(), "Armor" },
        {typeof(DieOff).ToString(), "DieOff" },
        {typeof(Revive).ToString(), "Revive" },


    };
    static public Spell StringToSpell(string spellType) {
        Spell behavior = null;
        switch (spellType) {
            case "AddHealth":
                behavior = new AddHealth();
                break;
            case "Antidote":
                behavior = new Antidote();
                break;
            case "Armor":
                behavior = new Armor();
                break;
            case "DieOff":
                behavior = new DieOff();
                break;
            case "Revive":
                behavior = new Revive();
                break;
        }
        return behavior;
    }
}
