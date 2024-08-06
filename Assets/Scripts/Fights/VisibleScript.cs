using UnityEngine;

public class VisibleScript : MonoBehaviour {
    // Start is called before the first frame update

    public bool isVisible;
    private void Start() {
        Recheck(isVisible);
    }
    private void Recheck(bool enabled) {
        this.GetComponent<SpriteRenderer>().enabled = enabled;
        foreach (var collision in this.GetComponents<Collider2D>()) {
            collision.enabled = enabled;
        }
        if (this.gameObject.tag == "Enemy") {
            var fighting = this.transform.Find("FightZone").GetComponent<Collider2D>();
            if (fighting != null)
                fighting.enabled = enabled;
        }
        if (GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player.name !=
            this.gameObject.name) {
            var canvas = this.gameObject.transform.Find("CharacterCanvas");
            if (canvas != null) {
                canvas.gameObject.SetActive(enabled);
            }
        }
        var moveScript = this.GetComponent<CharacterScript>();
        if (moveScript != null) {
            moveScript.enabled = enabled;
        }

    }

    public void Enable() {
        isVisible = true;
        Recheck(isVisible);
    }
    public void Disable() {
        isVisible = false;
        Recheck(isVisible);
    }
    public void SetVisible(bool enabled) {
        Recheck(enabled);
    }
}
