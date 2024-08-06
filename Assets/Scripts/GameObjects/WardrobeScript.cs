using UnityEngine;

public class WardrobeScript : MonoBehaviour {
    public bool isOpened = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }
    /* private void OnMouseEnter() {
         isOpened = true;
         animator.SetBool("IsOpened", true);
     }*/
}
