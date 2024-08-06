using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tomenuContinue : MonoBehaviour
{
    private SaveManager saveManager;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
        saveManager = GameObject.Find("Saving").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void OnClick()
    {
        saveManager.Load();

        GameObject.Find("Canvas").transform.Find("YouDead").gameObject.SetActive(false);
    }
}
