using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleLoading : MonoBehaviour {
    bool dialogFinished = false;
    GameObject knight1, knight2;
    Animator anim1, anim2;
    public DialogSystem dialogSystem;
    public SaveManager saveManager;

    // Start is called before the first frame update
    void Start() {
        knight1 = GameObject.Find("Knight");
        knight2 = GameObject.Find("Knight (1)");
        anim1 = knight1.GetComponent<Animator>();
        anim2 = knight2.GetComponent<Animator>();
        dialogSystem.Show("03StartDialog.txt");
        GameObject.Find("Player").GetComponent<CharacterScript>().enabled = false;
        dialogSystem.DialogEndedEvent += goToVillage;
    }
    void goToVillage(object sender, DialogEventArgs args) {
        dialogFinished = true;

    }
    private void Update() {
        var target = GameObject.Find("Player").transform;
        if (dialogFinished) {
            var speed = new Vector2(knight1.transform.position.x, knight1.transform.position.y)
                - (Vector2.MoveTowards(knight1.transform.position,
                (target.transform.position), 2.5f * Time.deltaTime));
            anim1.SetFloat("SpeedX", Mathf.Abs(speed.x));
            anim1.SetFloat("SpeedY", speed.y);
            anim2.transform.localScale = new Vector3(-1, 1, 1);
            anim2.SetFloat("SpeedX", Mathf.Abs(speed.x));
            anim2.SetFloat("SpeedY", speed.y);
            Debug.Log(speed);

            knight1.transform.position = Vector2.MoveTowards(knight1.transform.position,
                        (target.transform.position), 2.5f * Time.deltaTime);
            knight2.transform.position = Vector2.MoveTowards(knight2.transform.position,
                (target.transform.position), 2.5f * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "ExitBlocker") {
            saveManager.Save();
            SceneManager.LoadScene("04-Village");
        }
    }
}
