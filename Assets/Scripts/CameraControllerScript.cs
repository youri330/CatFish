using UnityEngine;

public class CameraControllerScript : MonoBehaviour {
    public GameObject player;
    public float smooth = 5.0f;

    private Vector3 offset;

    void Start() {
        //player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }


    void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * smooth);
    }
}
