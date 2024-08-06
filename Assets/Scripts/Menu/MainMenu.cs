using UnityEngine;
using UnityEngine.SceneManagement; //Работа со сценами


public class MainMenu : MonoBehaviour {
    public SaveManager saveManager;
    private void Awake() {


        saveManager = GameObject.Find("Saving").GetComponent<SaveManager>();
    }
    public void PlayPressed() {
        saveManager.isGameStarted = false;
        saveManager.Save();
        saveManager.LoadFirstScene();
    }
    public void ContinuePressed() {
        GameObject.Find("Saving").GetComponent<SaveManager>().Load();
        //SceneManager.LoadScene("SampleScene");
        //this.SetActive(false);
    }

    public void ExitPressed() {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
