using UnityEngine;

public class TreeScript : MonoBehaviour {
    public SpriteRenderer TreeSprite { get; set; }
    private Color defaultColor, fadedColor;
    // Start is called before the first frame update
    void Start() {
        TreeSprite = this.GetComponent<SpriteRenderer>();
        if (TreeSprite != null) {
            defaultColor = TreeSprite.color;
            fadedColor = defaultColor;
            fadedColor.a *= 0.7f;
        }
    }

    public void FadeIn() {
        if (TreeSprite != null) {
            TreeSprite.color = fadedColor;
        }
    }
    public void FadeOut() {
        if (TreeSprite != null) {
            TreeSprite.color = defaultColor;
        }
    }
}
