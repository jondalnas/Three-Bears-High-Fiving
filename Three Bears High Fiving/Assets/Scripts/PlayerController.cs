using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float dashSpeed = 15f;
	public float jumpHeight = 2.0f;
	public float gravity = 9.81f;

	private bool onWall;
	private bool jump;
	private bool facingRight;
	private bool isGrounded;
	private Vector3 groundNormal;
	
	private float moveing;

	private Vector3 scale;

	private Rigidbody rb;
	private GameObject playerSprite;

	void Start() {
		//Initializing
		rb = GetComponent<Rigidbody>();
		playerSprite = transform.Find("Player Sprite").gameObject;
		scale = playerSprite.transform.localScale;
	}

	void Update() {
		if (!jump) jump = Input.GetButtonDown("Jump");
		moveing = Input.GetAxis("Move");

		//If moveing direction is less then 0, then the player is faceing left
		if (moveing < 0) playerSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
		else if (moveing != 0) playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
	}

	void FixedUpdate() {
		//Calculate movement velocity
		if (isGrounded) {
			rb.velocity = moveing * moveSpeed * Vector3.right;
		} else {
			//If player is in air, then add 10% of movement speed to velocity and cap it at 100%, so air controlls feel more "air-y"
			rb.velocity += moveing * moveSpeed * Vector3.right * 0.1f;
			if (rb.velocity.x > moveSpeed) {
				rb.velocity = moveSpeed * Vector3.right + rb.velocity.y * Vector3.up;
			}

			if (rb.velocity.x < -moveSpeed) {
				rb.velocity = moveSpeed * Vector3.left + rb.velocity.y * Vector3.up;
			}
		}

		//Pressing Space will make the player jump or dash if it is colliding with wall
		if (jump) {
			if (isGrounded) {
				rb.velocity += new Vector3(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity));

				isGrounded = false;
			} else if (onWall) {
				//Initilizing faceing vector to left, for convenience
				int faceing = -1;

				//If facing left, faceing vector needs to be left
				if (facingRight)
					faceing = 1;

				//Adding dash and jump force
				rb.velocity += new Vector3(faceing*dashSpeed, Mathf.Sqrt(2 * jumpHeight * gravity), 0);

				onWall = false;
			}
		
			jump = false;
		}
	}

	void OnCollisionEnter(Collision col) {
		foreach (ContactPoint contact in col.contacts) {
			float slopeAngle = Vector3.Dot(contact.normal, Vector3.up);

			//The player is on ground if the player is colliding with
			//someting that is less then 45 degress steep
			if (slopeAngle > 0.5f) {
				isGrounded = true;
				groundNormal = contact.normal;
				return;
			}

			if (slopeAngle < 0.5f && slopeAngle > -0.5f) {
				onWall = true;
			}
		}
	}
}