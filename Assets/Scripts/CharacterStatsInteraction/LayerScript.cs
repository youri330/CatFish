using System.Collections.Generic;
using UnityEngine;

public class LayerScript : MonoBehaviour {
    private int startingOrder;
    private SpriteRenderer parentSprite;
    private List<TreeScript> trees = new List<TreeScript>();
    private Color defaultColor;
    private Color fadedColor;
    // Start is called before the first frame update
    void Start() {
        parentSprite = this.transform.parent.GetComponent<SpriteRenderer>();
        startingOrder = parentSprite.sortingOrder;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Tree") {
            var tree = collision.GetComponent<TreeScript>();
            if (trees.Count == 0) {
                RecheckLayer(tree);
            }

            trees.Add(tree);
        }
    }
    private void Update() {
        if (trees.Count != 0) {
            RecheckLayer(trees[0]);
        }
    }
    private void RecheckLayer(TreeScript tree) {
        if (tree.transform.position.y < this.transform.parent.position.y) {
            parentSprite.sortingOrder = tree.TreeSprite.sortingOrder - 1;
            tree.FadeIn();
        } else {
            parentSprite.sortingOrder = tree.TreeSprite.sortingOrder + 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Tree") {
            var tree = collision.GetComponent<TreeScript>();
            tree.FadeOut();
            trees.Remove(tree);
            if (trees.Count == 0) {
                parentSprite.sortingOrder = startingOrder;
            } else {
                trees.Sort((a, b) => a.TreeSprite.sortingOrder.CompareTo(b.TreeSprite.sortingOrder));
                RecheckLayer(trees[0]);
            }
        }
    }
}
