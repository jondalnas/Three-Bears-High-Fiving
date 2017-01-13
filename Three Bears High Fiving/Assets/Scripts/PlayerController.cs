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
	private bool hitHead;

	private float gravPull;
	private float moveing;

	private Vector3 heading;
	private Vector3 vel;
	private Vector3 scale;

	private CharacterController cc;
	private GameObject playerSprite;

	void Start() {
		//Initializing
		cc = GetComponent<CharacterController>();
		playerSprite = transform.FindChild("Player Sprite").gameObject;
		scale = playerSprite.transform.localScale;
	}

	void Update() {
		jump = Input.GetButtonDown("Jump");
		moveing = Input.GetAxis("Move");

		//If moveing direction is less then 0, then the player is faceing left
		if (moveing < 0) playerSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
		else if (moveing != 0) playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);

		//If the player bumps its head, then we will reset y velocity
		if (hitHead) vel.y = 0;

		hitHead = false;
	}

	void FixedUpdate() {
		//Moving the chracter by pressing the a/d buttons or by left stick and adding jump
		cc.Move(Vector3.right*moveing*moveSpeed+vel);

		//Pressing Space will make the player jump or dash if it is colliding with wall
		if (jump) {
			if (!isJumping) {
				if (cc.isGrounded) {
					vel.y += jumpSpeed;
				} else if (onWall)
					wallDash = true;
			}

			isJumping = true;
		} else
			isJumping = false;

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

		//Simple gravity, may want to change it
		if (!cc.isGrounded) {
			gravPull += 0.01f;
			if (gravPull >= maxGravityPull) gravPull = maxGravityPull;
			vel.y -= gravPull;
		} else {
			gravPull = 0;
			vel.y *= 0.6f;
		}

		//The player won't be able to walk in the z plane
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}

	void OnControllerColliderHit(ControllerColliderHit col) {
		//ContactPoint[] cols = col.points;
		//TODO: DEBUG, REMOVE WHEN DONE
		Debug.DrawRay(col.point, col.normal, Color.red);

		/*
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
		}*/

		//If Object collides with head, then hitHead equals true
		if (!col.transform.CompareTag("Player"))
			hitHead = true;
	}

	void OnCollisionExit(Collision col) {
		//If it doesn't collode, it resets ground and wall stuff
		onWall = false;
	}
}