using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuAwake : MonoBehaviour
{
    public SaveManager saveManager;
    // Start is called before the first frame update
    void Awake()
    {
        saveManager = GameObject.Find("Saving").GetComponent<SaveManager>();
        if (new DirectoryInfo(Application.persistentDataPath + "\\Saves").GetFiles().Length == 0
            || !saveManager.isGameStarted) {
            this.transform.Find("ContinueButton").gameObject.SetActive(false);
            this.transform.Find("SaveButton").gameObject.SetActive(false);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
