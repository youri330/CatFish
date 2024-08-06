using System;
using UnityEngine;

[Serializable]
public class GameSaveData {
    public string level, activePlayerGameName;
}
[Serializable]
public class PlayerSaveData {
    public string name, gameName, tag;
    public int hp, maxHp, mana, maxMana, selectedArtifactIndex, selectedSpellIndex;
    public bool isTalkable, isMovable, isArtifactSelected, isSpellSelected, isVisible;
    public float posX, posY;
    public string[] satelliteGameNames;
    public string condition;
    public ArtifactSaveData[] inventory;
    public SpellSaveData[] spellbook;
};

[Serializable]
public class QuestSaveData {
    public string tag, description;
    public bool isFinished;

    public QuestSaveData(string tag, string description, bool isFinished) {
        this.tag = tag;
        this.description = description;
        this.isFinished = isFinished;
    }
};
[Serializable]
public class PickUpSaveData {
    public string tag, gameName;
    public bool isDestroyed;
    public ArtifactSaveData artifact;

    public PickUpSaveData(string tag, string name, bool isDestroyed, ArtifactSaveData artifact) {
        this.tag = tag;
        this.gameName = name;
        this.isDestroyed = isDestroyed;
        this.artifact = artifact;
    }
};
[Serializable]
public class PickUpSpellSaveData {
    public string tag, gameName;
    public bool isDestroyed;
    public SpellSaveData spell;

    public PickUpSpellSaveData(string tag, string name, bool isDestroyed, SpellSaveData spell) {
        this.tag = tag;
        this.gameName = name;
        this.isDestroyed = isDestroyed;
        this.spell = spell;
    }
};
[Serializable]
public class ArtifactSaveData {
    public string type, name, gameName, description;
    public byte[] spriteBytes;
    public int spriteX, spriteY;
    public int power;
    public bool hasPower, isFightOnly, used;
};

[Serializable]
public class SpellSaveData : ArtifactSaveData {
    public int cost;
};

[Serializable]
public class DoorSaveData {
    public string tag, gameName;
    public bool isAccessible, isOpened;

    public DoorSaveData(string tag, string name, bool isAccessible, bool isOpened) {
        this.tag = tag;
        this.gameName = name;
        this.isAccessible = isAccessible;
        this.isOpened = isOpened;
    }
};