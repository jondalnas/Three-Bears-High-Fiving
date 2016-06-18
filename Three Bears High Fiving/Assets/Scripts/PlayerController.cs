using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float jumpSpeed = 100f;

    private float distToGround;

    private CharacterController cc;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
    void FixedUpdate () {
        float x = Input.GetAxis("Horizontal") * moveSpeed;

        if (Input.GetKeyDown("space")&&isGrounded()) {
            cc.Move(Vector3.up*jumpSpeed);
        }

        cc.Move(new Vector3(x, 0, 0));
    }

    bool isGrounded() {
        distToGround = GetComponent<CharacterController>().bounds.extents.y + 0.1f;
        Debug.Log(distToGround);
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
}
