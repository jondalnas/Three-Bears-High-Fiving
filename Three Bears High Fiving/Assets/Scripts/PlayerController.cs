using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float jumpSpeed = 10f;
	public float dashSpeed = 15f;
	public float maxGravityPull = 0.5f;

	public GameObject head;

	private bool onWall;
	private bool jump;
	private bool wallDash;
	private bool facingRight;
	private bool isJumping;

	private float gravPull;

	private Vector3 heading;
	private Vector3 vel;

	private CharacterController cc;

	void Start () {
		//Initializing
		cc = GetComponent<CharacterController>();
	}

	void Update() {
		//Pressing Space will make the player jump, or dash if it isn't allready jumping
		if (Input.GetButtonDown("Jump")) {
			if (!isJumping) {
				if (cc.isGrounded)
					jump = true;
				else if (onWall)
					wallDash = true;
			}

			isJumping = true;
		} else
			isJumping = false;
		//If the player bumps its head, then we will reset y velocity
		if (head.GetComponent<HeadCollider>().hitHead) vel.y = 0;

		//Simple gravity, may want to change it
		if (!cc.isGrounded) {
			gravPull += 0.01f;
			if (gravPull >= maxGravityPull) gravPull = maxGravityPull;
			vel.y -= gravPull;
		} else
			vel.y = 0;
	}

	void FixedUpdate() {
		//Moving the chracter by pressing the a/d buttons or by left stick
		cc.Move(Vector3.right*Input.GetAxis("Move")*moveSpeed);

		//If jumping, jump!
		if (jump) {
			vel.y += jumpSpeed;
			jump = false;
		}

		//If on wall and jumps, player dashes
		if (wallDash) {
			//Initilizing faceing vector to left, for convenience
			int faceing = -1;

			//If facing left, faceing vector needs to be left
			if (facingRight)
				faceing = 1;

			//Adding dash and jump force
			vel.x += faceing*dashSpeed;
			vel.y += jumpSpeed;
			wallDash = false;
		}

		//Move player in all directions
		cc.Move(vel);

		//The player wont be able to walk in the z plane
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}

	void OnCollisionStay(Collision col) {
		ContactPoint[] cols = col.contacts;
		foreach (ContactPoint con in cols) {
			//This is where the contact is apearing
			heading = (con.point-transform.position).normalized;

			if (con.thisCollider.CompareTag("Head")) Debug.Log("hello");

			//Collision check
			if (Mathf.Floor(Mathf.Abs(heading.x)*10) >= 7) {
				onWall = true;
				if (heading.x > 0)
					facingRight = false;
				else 
					facingRight = true;
			}
		}
	}

	void OnCollisionExit(Collision col) {
		//If it doesn't collode, it resets ground and wall stuff
		onWall = false;
	}
}