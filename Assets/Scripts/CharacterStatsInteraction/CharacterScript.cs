using UnityEngine;

public class CharacterScript : MonoBehaviour {
    public float speed;
    public float distance;
    GameObject activePlayer;
    // public Text hpLabel;

    private bool isFacingRight = true;
    private Vector2 moveVelocity;
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
    void Update() {
        activePlayer = GameObject.Find("Canvas").transform.Find("CharacterIcon").GetComponent<ActivePlayer>().player;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        anim.SetFloat("SpeedX", Mathf.Abs(moveInput.x));
        anim.SetFloat("SpeedY", moveInput.y);
        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight) {
            Flip();
        }
        moveVelocity = moveInput.normalized * speed;
        if (activePlayer.name != this.gameObject.name) {
             if (Vector3.Distance(this.transform.position, activePlayer.transform.position) > distance) {
                 this.transform.position = Vector2.MoveTowards(this.transform.position, activePlayer.transform.position -
                     new Vector3(moveInput.normalized.x, moveInput.normalized.y, 0), speed * Time.deltaTime);
             }
              
        }
        
        
        //hpLabel.text = character.Hp.ToString() + " / " + character.MaxHp.ToString();
    }
    private void Flip() {
        isFacingRight = !isFacingRight;
        rb.transform.localScale = new Vector3(-rb.transform.localScale.x, rb.transform.localScale.y, rb.transform.localScale.z);
    }

    void FixedUpdate() {
        if (activePlayer != null && activePlayer.name == this.gameObject.name) {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }
}
