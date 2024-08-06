using UnityEngine;

public class DoorScript : MonoBehaviour {
    public bool isOpened;
    public bool isAccessible = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Open() {
        isOpened = true;
        if (animator != null) {
            animator.SetBool("IsOpened", true);
        }
    }
    public void Close() {
        isOpened = false;
        if (animator != null) {
            animator.SetBool("IsOpened", false);
        }
    }
}
