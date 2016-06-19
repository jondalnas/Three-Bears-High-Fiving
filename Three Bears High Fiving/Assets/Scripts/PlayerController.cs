using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float jumpSpeed = 10f;
    public float gravity = 9.81f;
    public float boostSpeed = 15f;
    public float radius = 0.66f;
    public GameObject center;

    private float vSpeed;
    private float wSpeed;

    private bool onWall;

    private CharacterController cc;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
    void Update () {
        Vector3 vel = Vector3.right*Input.GetAxis("Horizontal") * moveSpeed;

        Collider[] cols = Physics.OverlapSphere(center.transform.position, radius); //Make the collision box smaler or something, It's hitting the slope

        onWall = false;
        Collider wall = null;
        foreach (Collider col in cols) {
            if (col != gameObject) {
                wall = col;
                onWall = true;
                break;
            }
        }

        bool onWall0 = onWall;
        if (cc.isGrounded) onWall0 = false;

        if (cc.isGrounded || onWall0) {
            if (cc.isGrounded) vSpeed = 0;

            if (Input.GetKeyDown("space")) {
                if (!onWall0) vSpeed = jumpSpeed;
                else vSpeed = jumpSpeed / 2;

                if (onWall0) {
                    Vector3 heading = wall.gameObject.transform.position-transform.position;

                    float dir;

                    if (heading.x > 0) dir = -1;
                    else dir = 1;

                    wSpeed = dir*boostSpeed;
                }
            }
        }

        if (!onWall) vSpeed -= gravity * Time.deltaTime;

        if (onWall) vSpeed *= 0.95f;

        wSpeed *= 0.95f;

        vel.y = vSpeed;

        vel.x += wSpeed;

        cc.Move(vel*Time.deltaTime);
    }
}
