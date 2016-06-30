using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float jumpSpeed = 10f;
	public float dashSpeed = 15f;

	public Transform groundCheck;

	private bool onWall;
	private bool onGround;
	private bool jump;
	private bool wallDash;
	private bool facingRight;
	private bool isJumping;

	private Vector3 heading;
	private Vector3 vel;

	private Rigidbody rb;

	void Start () {
		//Initializing
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		//Find out wheteher player is grounded or not
		onGround = Physics.Linecast(transform.position, groundCheck.position);

		//Pressing Space will make the player jump, or dash if it isn't allready jumping
		if (Input.GetButtonDown("Jump")) {
			if (!isJumping) {
				if (onGround)
					jump = true;
				else if (onWall)
					wallDash = true;
			}

			isJumping = true;
		} else
			isJumping = false;
	}

	void FixedUpdate() {
		//Creating velocity vector, this will be used to make the character walk
		Vector3 vel = Vector3.zero;

		//Moving the chracter by pressing the a/d buttons
		vel.x += Input.GetAxis("Move");

		//Move player by vel*moveSpeed
		rb.AddForce(Vector3.right*vel.x*moveSpeed);

		//If jumping, jump!
		if (jump) {
			rb.AddForce(Vector3.up*jumpSpeed);
			jump = false;
		}

		//If on wall and jumps, player dashes
		if (wallDash) {
			//Initilizing faceing vector to left, for convenience
			Vector3 faceing = Vector3.left;

			//If facing left, faceing vector needs to be left
			if (facingRight)
				faceing = Vector3.right;

			//Adding dash and jump force
			rb.AddForce(faceing*jumpSpeed);
			rb.AddForce(Vector3.up*jumpSpeed);
			wallDash = false;
		}
	}

	void OnCollisionStay(Collision col) {
	    ContactPoint[] cols = col.contacts;
		foreach (ContactPoint con in cols) {
			//This is where the contact is apearing
			heading = (con.point-transform.position).normalized;

			//Collision check
			if (Mathf.Floor(Mathf.Abs(heading.x)*10) >= 7) {
	            onWall = true;
				if (heading.x > 0)
					facingRight = false;
				else 
					facingRight = true;
	        }
			if (Mathf.Abs(heading.y) > 0)
	            onGround = true;
	    }
	}

	void OnCollisionExit(Collision col) {
		//If it doesn't collode, it resets ground and wall stuff
		onWall = false;
		onGround = false;
	}
}