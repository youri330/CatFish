using CatFishScripts.Artifacts;
using CatFishScripts.Characters;
using CatFishScripts.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class AllGameData {
    public GameSaveData game;
    public PlayerSaveData[] players;
    public PlayerSaveData[] enemies;
    public QuestSaveData[] quests;
    public PickUpSaveData[] pickUps;
    public PickUpSpellSaveData[] pickUpsSpells;
    public DoorSaveData[] doors;
    public bool isGameStarted = true;

    public AllGameData(GameSaveData game, PlayerSaveData[] players, PlayerSaveData[] enemies,
        QuestSaveData[] quests, PickUpSaveData[] pickUps, PickUpSpellSaveData[] pickUpsSpells,
        DoorSaveData[] doors, bool isGameStarted) {
        this.game = game;
        this.players = players;
        this.enemies = enemies;
        this.quests = quests;
        this.pickUps = pickUps;
        this.doors = doors;
        this.pickUpsSpells = pickUpsSpells;
        this.isGameStarted = isGameStarted;
    }
}
public class SaveManager : MonoBehaviour {
    private AllGameData allData;
    private GameSaveData game;
    private PlayerSaveData[] players;
    private PlayerSaveData[] enemies;
    private QuestSaveData[] quests;
    private PickUpSaveData[] pickUps;
    private PickUpSpellSaveData[] pickUpsSpells;
    private DoorSaveData[] doors;
    public bool isGameStarted = true;
    private void Start() {
        isAboutToStart = false;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += SetOtherGameData;
    }
    public void LoadFirstScene() {
        SceneManager.sceneLoaded -= SetOtherGameData;
        SceneManager.LoadScene("01-Flat");
        SceneManager.sceneLoaded += SetOtherGameData;
    }
    public string AddSaveSlot() {
        string slotName = DateTime.Now.ToString();
        slotName = slotName.Replace(' ', '_').Replace(':', '.');

        slotName += ".dat";
        Debug.Log(slotName);
        File.Copy(Application.persistentDataPath + "\\Saves\\fastSave.dat",
            Application.persistentDataPath + "\\Saves\\" + slotName, true);
        return slotName;
    }
    public void LoadSlot(string slotName) {
        File.Copy(Application.persistentDataPath + "\\Saves\\" + slotName + ".dat",
            Application.persistentDataPath + "\\Saves\\fastSave.dat", true);
        Load();
    }
    public void Save() {
        
        GatherData();
        SerializeFromTmpArrays();
    }
    public void Load() {
       
        if (!isGameStarted) {
            SceneManager.LoadScene("01-Flat");
        }
        DeserializeToTmpArrays();
        SetObjectsData();
    }
    private void GatherData() {
        GatherGameData();
        if (game.level == "Menu") {
            return;
        }
        GatherPlayersData();
        GatherEnemiesData();
        GatherQuestsData();
        GatherPickUpsData();
        GatherPickUpsSpellsData();
        GatherDoorsData();
    }
    private void GatherGameData() {
        game = new GameSaveData();
        game.level = SceneManager.GetActiveScene().name;
        if (game.level == "Menu") {
            return;
        }
        game.activePlayerGameName = GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player.name;
    }
    private void GatherPlayersData() {
        var playersInGame =  FindAllObjectsByTag("Player");
        players = new PlayerSaveData[playersInGame.Length];
        for (int i = 0; i < players.Length; i++) {
            players[i] = PlayerToSaveData(playersInGame[i]);
        }
    }
    private void GatherEnemiesData() {
        var enemiesInGame = FindAllObjectsByTag("Enemy");
        enemies = new PlayerSaveData[enemiesInGame.Length];
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i] = PlayerToSaveData(enemiesInGame[i]);
        }
    }
    private void GatherQuestsData() {
        QuestSystem questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
        var gameQuests = questSystem.GetQuestsList();
        quests = new QuestSaveData[gameQuests.Count];
        for (int i = 0; i < quests.Length; i++) {
            quests[i] = new QuestSaveData(gameQuests[i].Tag, gameQuests[i].Description, gameQuests[i].IsFinished);
        }
    }
    private void GatherPickUpsData() {
        var notUnique = GameObject.FindGameObjectsWithTag("PickUp");
        var unique = GameObject.FindGameObjectsWithTag("PickUpUnique");
        var _pickUps = new GameObject[notUnique.Length + unique.Length];
        notUnique.CopyTo(_pickUps, 0);
        unique.CopyTo(_pickUps, notUnique.Length);

        pickUps = new PickUpSaveData[_pickUps.Length];
        for (int i = 0; i < pickUps.Length; i++) {
            pickUps[i] = new PickUpSaveData(_pickUps[i].tag, _pickUps[i].name, _pickUps[i].GetComponent<Collider2D>().enabled,
                ArtifactToSaveData(_pickUps[i].GetComponent<UnityArtifact>().Behavior));
        }
    }
    private void GatherPickUpsSpellsData() {
        var notUnique = GameObject.FindGameObjectsWithTag("PickUpSpell");
        
        pickUpsSpells = new PickUpSpellSaveData[notUnique.Length];
        for (int i = 0; i < pickUpsSpells.Length; i++) {
            pickUpsSpells[i] = new PickUpSpellSaveData(notUnique[i].tag, notUnique[i].name, notUnique[i].GetComponent<Collider2D>().enabled,
                SpellToSaveData(notUnique[i].GetComponent<UnitySpell>().Behavior));
        }
    }
    private void GatherDoorsData() {
        QuestSystem questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
        var gameDoors = GameObject.FindGameObjectsWithTag("Door");
        doors = new DoorSaveData[gameDoors.Length];
        for (int i = 0; i < doors.Length; i++) {
            doors[i] = new DoorSaveData(gameDoors[i].tag, gameDoors[i].name, gameDoors[i].GetComponent<DoorScript>().isAccessible,
                gameDoors[i].GetComponent<DoorScript>().isOpened);
        }
    }
    private void SerializeFromTmpArrays() {
        allData = new AllGameData(game, players, enemies, quests, pickUps, pickUpsSpells, doors, isGameStarted);
        using (FileStream fs = new FileStream(Application.persistentDataPath + "\\Saves\\fastSave.dat", FileMode.Create)) {
            new BinaryFormatter().Serialize(fs, allData);
        }
        //Debug.Log(Application.persistentDataPath + "\\fastSave.dat");
    }
    private void DeserializeToTmpArrays() {

        
        using (FileStream fs = new FileStream(Application.persistentDataPath + "\\Saves\\fastSave.dat", FileMode.Open)) {
            allData = (AllGameData)new BinaryFormatter().Deserialize(fs);
        }
    }
    private void SetObjectsData() {
        
        SetGameData();

    }
    bool isAboutToStart;
    private void SetOtherGameData(Scene scene, LoadSceneMode mode) {
        if (!isGameStarted) {
            isGameStarted = true;
            return;
        }
        if (scene.name == "Menu") {
            return;
        }
        if (!File.Exists(Application.persistentDataPath + "\\Saves\\fastSave.dat")) {
            return;
        }
        
        if (allData == null || scene.name != allData.game.level) {
            DeserializeToTmpArrays();
            GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().
            ChangePlayer(GameObject.Find(allData.game.activePlayerGameName));
        }
        SetPlayersData();
        SetQuestsData();
        if (scene.name == allData.game.level) {
            SetEnemiesData();
            SetPickUpsData();
            SetPickUpsSpellsData();
            SetDoorsData();
        } else {
            Save();
        }
    }
    private void SetGameData() {
        if (SceneManager.GetActiveScene().name != allData.game.level) {
            SceneManager.LoadSceneAsync(allData.game.level);
            
        } else {           
            SetOtherGameData(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        
    }
    private void SetPlayersData() {
        GameObject.Find("Canvas").transform.Find("Buttons").Find("InventoryButton").Find("Slot").GetComponent<Image>().sprite = null;

        GameObject.Find("Canvas").transform.Find("Buttons").Find("SpellbookButton").Find("Slot").GetComponent<Image>().sprite = null;
        var playersInGame = new List<GameObject>(FindAllObjectsByTag("Player"));
        for (int i = 0; i < allData.players.Length; i++) {
            SetDataToPlayer(allData.players[i], playersInGame.Find(a => a.name == allData.players[i].gameName));
        }
    }
    private void SetEnemiesData() {
        var playersInGame = new List<GameObject>(FindAllObjectsByTag("Enemy"));
        for (int i = 0; i < allData.enemies.Length; i++) {
            SetDataToEnemy(allData.enemies[i], playersInGame.Find(a => a.name == allData.enemies[i].gameName));
        }
    }
    private PlayerSaveData PlayerToSaveData(GameObject playerInGame) {
        var player = new PlayerSaveData {
            gameName = playerInGame.name,
            isVisible = playerInGame.GetComponent<VisibleScript>().isVisible,

            tag = playerInGame.tag,
            posX = playerInGame.transform.position.x,
            posY = playerInGame.transform.position.y
        };
        var gamePlayerInfo = playerInGame.GetComponent<UnityCharacter>();
        player.name = gamePlayerInfo.Character.Name;
        player.hp = (int)gamePlayerInfo.Character.Hp;
        player.maxHp = (int)gamePlayerInfo.Character.MaxHp;
        if (gamePlayerInfo.Character.GetType() == typeof(Magician)) {
            player.mana = (int)(gamePlayerInfo.Character as Magician).Mana;
            player.maxMana = (int)(gamePlayerInfo.Character as Magician).MaxMana;
        }
        player.selectedArtifactIndex = (int)gamePlayerInfo.selectedArtifactIndex;
        player.selectedSpellIndex = (int)gamePlayerInfo.selectedSpellIndex;
        player.isTalkable = gamePlayerInfo.Character.IsTalkable;
        player.isMovable = gamePlayerInfo.Character.IsMovable;
        player.isArtifactSelected = gamePlayerInfo.isArifactSelected;
        player.isSpellSelected = gamePlayerInfo.isSpellSelected;
        player.satelliteGameNames = new string[gamePlayerInfo.satellites.Count];
        for (int i = 0; i < gamePlayerInfo.satellites.Count; i++) {
            player.satelliteGameNames[i] = gamePlayerInfo.satellites[i].gameObject.name;
        }
        player.condition = gamePlayerInfo.Character.Condition.ToString();
        player.inventory = new ArtifactSaveData[gamePlayerInfo.Character.Inventory.Artifacts.Count];
        for (int i = 0; i < player.inventory.Length; i++) {
            player.inventory[i] = ArtifactToSaveData(gamePlayerInfo.Character.Inventory.Artifacts[i]);
        }
        if (gamePlayerInfo.Character.GetType() == typeof(Magician)) {
            player.spellbook = new SpellSaveData[(gamePlayerInfo.Character as Magician).SpellsList.Spells.Count];
            for (int i = 0; i < player.spellbook.Length; i++) {
                player.spellbook[i] = SpellToSaveData((gamePlayerInfo.Character as Magician).SpellsList.Spells[i]);
            }
        }
        return player;
    }
    private void SetDataToEnemy(PlayerSaveData enemy, GameObject enemyInGame) {
        SetDataToPlayer(enemy, enemyInGame);
        if (!enemy.isVisible) {
            return;
        }
        if (enemy.condition.ToLower() == "dead") {
            var zone = enemyInGame.transform.Find("FightZone").gameObject;
            if (zone != null) {
                Destroy(zone);
            }
        }
    }
    private void SetDataToPlayer(PlayerSaveData player, GameObject playerInGame) {
        if (playerInGame == null || player.hp == 0 ) {
            return;
        }
        playerInGame.GetComponent<VisibleScript>().SetVisible(player.isVisible);
        
        if (SceneManager.GetActiveScene().name == allData.game.level) {
            playerInGame.transform.position = new Vector2(player.posX, player.posY);
        }
        var gamePlayerInfo = playerInGame.GetComponent<UnityCharacter>();
        gamePlayerInfo.Character.Hp = (uint)player.hp;
        gamePlayerInfo.Character.MaxHp = (uint)player.maxHp;
        if (gamePlayerInfo.Character.GetType() == typeof(Magician)) {
            (gamePlayerInfo.Character as Magician).Mana = (uint)player.mana;
            (gamePlayerInfo.Character as Magician).MaxMana = (uint)player.maxMana;
        }
        gamePlayerInfo.Character.Inventory.Artifacts = new List<Artifact>();
        for (int i = 0; i < player.inventory.Length; i++) {
            gamePlayerInfo.Character.Inventory.AddArtifact(SaveDataToArtifact(player.inventory[i]));
        }
        if (gamePlayerInfo.Character.GetType() == typeof(Magician)) {
            (gamePlayerInfo.Character as Magician).SpellsList.Spells = new List<Spell>();
            for (int i = 0; i < player.spellbook.Length; i++) {
                (gamePlayerInfo.Character as Magician).SpellsList.Spells.Add(SaveDataToSpell(player.spellbook[i]));
            }
        }
        gamePlayerInfo.isArifactSelected = false;
        gamePlayerInfo.isSpellSelected = false;
        gamePlayerInfo.selectedArtifact = null;
        gamePlayerInfo.selectedSpell = null;
        
        gamePlayerInfo.selectedArtifactIndex = 0;
        gamePlayerInfo.selectedSpellIndex = 0;
        gamePlayerInfo.Character.IsTalkable = player.isTalkable;
        gamePlayerInfo.Character.IsMovable = player.isMovable;
        gamePlayerInfo.satellites = new List<UnityCharacter>();
        for (int i = 0; i < player.satelliteGameNames.Length; i++) {
            if (GameObject.Find(player.satelliteGameNames[i]) != null) {
                gamePlayerInfo.satellites.Add(GameObject.Find(player.satelliteGameNames[i]).GetComponent<UnityCharacter>());
            } 
        }
        gamePlayerInfo.Character.Condition = (Character.ConditionType)Enum.Parse(typeof(Character.ConditionType), player.condition, true);
        if (gamePlayerInfo.Character.Condition == Character.ConditionType.dead) {
            gamePlayerInfo.GetComponent<VisibleScript>().SetVisible(false);
        }
    }
    private ArtifactSaveData ArtifactToSaveData(Artifact artifact) {
        if (artifact == null) {
            return null;
        }
        var artifactInfo = new ArtifactSaveData();
        artifactInfo.type = artifact.GetType().ToString();
        Texture2D texture = artifact.ArtifactSprite.texture;
        artifactInfo.spriteBytes = ImageConversion.EncodeToPNG(texture);
        artifactInfo.spriteX = artifact.ArtifactSprite.texture.width;
        artifactInfo.spriteY = artifact.ArtifactSprite.texture.height;
        if (artifact.unityShell != null) {
            artifactInfo.gameName = artifact.unityShell.gameObject.name;
        }
        artifactInfo.name = artifact.Name;
        artifactInfo.description = artifact.Description;
        artifactInfo.hasPower = artifact.HasPower;
        artifactInfo.power = (int)artifact.Power;
        if (artifact.GetType().IsSubclassOf(typeof(Bottle))) {
            artifactInfo.power = (int)(artifact as Bottle).Volume;
        }
        artifactInfo.isFightOnly = artifact.IsFightOnly;
        artifactInfo.used = artifact.Used;
        return artifactInfo;
    }
    private Artifact SaveDataToArtifact(ArtifactSaveData artifactInfo) {
        if (artifactInfo == null) {
            return null;
        }

        var artifact = UnityArtifact.StringToArtifact(UnityArtifact.TypeToString[artifactInfo.type],
            artifactInfo.name, artifactInfo.description, artifactInfo.power);
        /* if (artifact.unityShell != null)
             artifact.unityShell = new UnityArtifact();*/

        artifact.IsFightOnly = artifactInfo.isFightOnly;
        artifact.Used = artifactInfo.used;
        
        Texture2D tex = new Texture2D(artifactInfo.spriteX, artifactInfo.spriteY);
        ImageConversion.LoadImage(tex, artifactInfo.spriteBytes);
        Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.one);
        artifact.ArtifactSprite = mySprite;


        // artifactInfo.gameName = artifact.unityShell.gameObject.name;

        return artifact;
    }
    private SpellSaveData SpellToSaveData(Spell spell) {
        var spellInfo = new SpellSaveData();
        spellInfo.type = spell.GetType().ToString();
        Texture2D texture = spell.SpellSprite.texture;
        spellInfo.spriteBytes = ImageConversion.EncodeToPNG(texture);

        spellInfo.spriteX = spell.SpellSprite.texture.width;
        spellInfo.spriteY = spell.SpellSprite.texture.height;
        if (spell.unityShell != null) {
            spellInfo.gameName = spell.unityShell.gameObject.name;
        }
        spellInfo.name = spell.Name;
        spellInfo.description = spell.Description;
        spellInfo.hasPower = spell.HasPower;
        spellInfo.power = (int)spell.Power;
        spellInfo.isFightOnly = spell.IsFightOnly;
        spellInfo.used = spell.Used;
        spellInfo.cost = (int)spell.Cost;
        return spellInfo;
    }
    private Spell SaveDataToSpell(SpellSaveData spellInfo) {
        if (spellInfo == null) {
            return null;
        }
        var spell = UnitySpell.StringToSpell(UnitySpell.TypeToString[spellInfo.type]);
        spell.Name = spellInfo.name;
        spell.Description = spellInfo.description;
        spell.Power = spellInfo.power;
        Texture2D tex = new Texture2D(spellInfo.spriteX, spellInfo.spriteY);
        ImageConversion.LoadImage(tex, spellInfo.spriteBytes);
        Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.one);
        spell.SpellSprite = mySprite;

        // spell.SpellSprite = Resources.Load<Sprite>(spellInfo.spritePath);
        // artifactInfo.gameName = artifact.unityShell.gameObject.name;
        spell.IsFightOnly = spellInfo.isFightOnly;
        spell.Used = spellInfo.used;
        return spell;
    }
    GameObject[] FindAllObjectsByTag(string tag) {
        List<GameObject> gameObjects = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].hideFlags == HideFlags.None) {
                if (objs[i].tag == tag) {
                    gameObjects.Add(objs[i].gameObject);
                }
            }
        }

        return gameObjects.ToArray();
    }
    private void SetQuestsData() {
        QuestSystem questSystem = GameObject.Find("QuestSystem").GetComponent<QuestSystem>();
        questSystem.RemoveAllQuests();
        for (int i = 0; i < allData.quests.Length; i++) {
                questSystem.Add(new CatFishScripts.Quest(allData.quests[i].tag, null, allData.quests[i].description,
                    allData.quests[i].isFinished));
        }
    }
    private void SetPickUpsData() {
        var notUnique = FindAllObjectsByTag("PickUp");
        var unique = FindAllObjectsByTag("PickUpUnique");
        var _pickUps = new GameObject[notUnique.Length + unique.Length];
        notUnique.CopyTo(_pickUps, 0);
        unique.CopyTo(_pickUps, notUnique.Length);
        var inGamePickUps = new List<GameObject>(_pickUps);
        for (int i = 0; i < allData.pickUps.Length; i++) {
            var pickUp = inGamePickUps.Find(a => a.name == allData.pickUps[i].gameName);
            pickUp.GetComponent<Collider2D>().enabled = allData.pickUps[i].isDestroyed;
            pickUp.GetComponent<SpriteRenderer>().enabled = allData.pickUps[i].isDestroyed;
        }
    }
    private void SetPickUpsSpellsData() {
        var notUnique = FindAllObjectsByTag("PickUpSpell");
        var inGamePickUps = new List<GameObject>(notUnique);
        for (int i = 0; i < allData.pickUpsSpells.Length; i++) {
            var pickUp = inGamePickUps.Find(a => a.name == allData.pickUpsSpells[i].gameName);
            pickUp.GetComponent<Collider2D>().enabled = allData.pickUpsSpells[i].isDestroyed;
            pickUp.GetComponent<SpriteRenderer>().enabled = allData.pickUpsSpells[i].isDestroyed;
        }
    }
    private void SetDoorsData() {
        var gameDoors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));
        for (int i = 0; i < allData.doors.Length; i++) {
            var door = gameDoors.Find(a => a.name == allData.doors[i].gameName).GetComponent<DoorScript>();
            door.isAccessible = allData.doors[i].isAccessible;
            if (allData.doors[i].isOpened) {
                door.Open();
            } else {
                door.Close();
            }
        }
    }
}
