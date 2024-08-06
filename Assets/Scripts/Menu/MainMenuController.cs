using UnityEngine;
using UnityEngine.SceneManagement; //Работа со сценами
public class MainMenuController : MonoBehaviour {
    void Update() {
        Object.DontDestroyOnLoad(this.gameObject);
        if (Input.GetKeyDown(KeyCode.Escape)) {

            SceneManager.LoadScene("Menu");
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
