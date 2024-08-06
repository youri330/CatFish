using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveMenuScript : MonoBehaviour {
    public SaveManager saveManager;
    public TMP_Dropdown dropdown;
    private string fastSaveName = "Быстрое сохранение";
    private void Awake() {
        DontDestroyOnLoad(dropdown.gameObject);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "\\Saves");
        var files = directoryInfo.GetFiles();
        dropdown.AddOptions(new List<string>(new string[] { fastSaveName }));
        foreach (var file in files) {
            string[] names = new string[1];
            names[0] = Path.GetFileNameWithoutExtension(file.Name);
            if (names[0] != "fastSave") {
                dropdown.AddOptions(new List<string>(names));
            }
        }
        
    }
    public void AddSlot() {
        string[] names = new string[1];
        names[0] = Path.GetFileNameWithoutExtension(saveManager.AddSaveSlot());
        dropdown.AddOptions(new List<string>(names));
    }
    public void LoadSlot() {
        string name = dropdown.options[dropdown.value].text;
        if (name == fastSaveName) {
            saveManager.Load();
        } else {
            saveManager.LoadSlot(name);
        }
    }
}
