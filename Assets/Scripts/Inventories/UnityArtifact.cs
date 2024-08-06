using CatFishScripts.Artifacts;
using System.Collections.Generic;
using UnityEngine;

public class UnityArtifact : MonoBehaviour {
    public string artifactType;
    public string artifactName, description;
    public int power;
    public bool isFightOnly;
    public bool used = false;
    public Artifact Behavior {
        get; set;
    }

    private void Awake() {
        Behavior = StringToArtifact(artifactType, artifactName, description, power);
      
        Behavior.IsFightOnly = this.isFightOnly;
        Behavior.Used = this.used;

        Behavior.ArtifactSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        Behavior.unityShell = this;
    }
    public static Dictionary<string, string> TypeToString = new Dictionary<string, string>() {
        {typeof(BasiliskEye).ToString(), "BasiliskEye" },
        {typeof(DeadWaterBottle).ToString(), "DeadWaterBottle" },
        {typeof(Decoctum).ToString(), "Decoctum" },
        {typeof(Lightning).ToString(), "Lightning" },
        {typeof(LivingWaterBottle).ToString(), "LivingWaterBottle" },
        {typeof(PoisonousSaliva).ToString(), "PoisonousSaliva" },
        {typeof(Slipper).ToString(), "Slipper" },
        {typeof(EmptyArtifact).ToString(), "None" }

    };
    static public Artifact StringToArtifact(string _artifactType, string artifactName, string description, int power) {
        Artifact behavior = null;
        switch (_artifactType) {
            case "BasiliskEye":
                behavior = new BasiliskEye(artifactName, description);
                break;
            case "DeadWaterBottle":
                behavior = new DeadWaterBottle(artifactName, description, (Bottle.VolumeType)power);
                break;
            case "Decoctum":
                behavior = new Decoctum(artifactName, description);
                break;
            case "Lightning":
                behavior = new Lightning(artifactName, description, (uint)power);
                break;
            case "LivingWaterBottle":
                behavior = new LivingWaterBottle(artifactName, description, (Bottle.VolumeType)power);
                break;
            case "PoisonousSaliva":
                behavior = new PoisonousSaliva(artifactName, description, (uint)power);
                break;
            case "Slipper":
                behavior = new Slipper(artifactName, description, (uint)power);
                break;
            case "None":
            case "EmptyArifact":
                behavior = new EmptyArtifact(artifactName, description);
                break;
        }
        return behavior;
    }
}
