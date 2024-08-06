using CatFishScripts.Characters;
using UnityEngine;
using UnityEngine.UI;
public class ActivePlayer : MonoBehaviour {
    public GameObject player;
    private GameObject Mana, Hp, Face, NameText, SpellBook, SpellBookButton, Inventory, MainCamera;
    private void Start() {
        ChangePlayer(player);
    }

    private void EstablishComponents() {
        Mana = this.transform.Find("ManaBar").Find("Mana").gameObject;
        Hp = this.transform.Find("HpBar").Find("Hp").gameObject;
        Face = this.transform.Find("Face").gameObject;
        NameText = this.transform.Find("NameTextBox").Find("Message").gameObject;
        SpellBookButton = this.transform.parent.Find("Buttons").Find("SpellbookButton").gameObject;
        Inventory = this.transform.parent.Find("Inventory").gameObject;
        SpellBook = this.transform.parent.Find("SpellBook").gameObject;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void ChangePlayer(GameObject newPlayer) {
        if (newPlayer == null) {
            return;
        }
        EstablishComponents();
        var oldUnityCharacter = player.GetComponent<UnityCharacter>();
        oldUnityCharacter.isArifactSelected = false;
        oldUnityCharacter.GetComponent<UnityCharacter>().selectedArtifactIndex = 0;
        var artifactButton =
            GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>();
        artifactButton.sprite = null;
        artifactButton.color = Color.white;
        oldUnityCharacter.isSpellSelected = false;
        oldUnityCharacter.selectedSpellIndex = 0;
        var spellButton =
            GameObject.Find("Canvas").transform.transform.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>();
        spellButton.sprite = null;
        spellButton.color = Color.white;
        if (player != null) {
            player.transform.Find("CharacterCanvas").gameObject.SetActive(true);
        }
        player = newPlayer;
        player.transform.Find("CharacterCanvas").gameObject.SetActive(false);
        Mana.GetComponent<ManaFillingScript>().ChangePlayer(player);
        Hp.GetComponent<HpFillingScript>().ChangePlayer(player);
        Face.GetComponent<Image>().sprite = player.GetComponent<SpriteRenderer>().sprite;
        var character = player.GetComponent<UnityCharacter>().Character;
        NameText.GetComponent<Text>().text = character.Name;
        SpellBookButton.SetActive(character.GetType() == typeof(Magician));
        Inventory.GetComponent<UnityInventory>().owner = character;
        SpellBook.GetComponent<UnitySpellbook>().owner = character;
        MainCamera.transform.position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -1000);
        MainCamera.GetComponent<CameraControllerScript>().player = player;
        Inventory.GetComponent<UnityInventory>().Hide();
        SpellBook.GetComponent<UnitySpellbook>().Hide();
    }
}
