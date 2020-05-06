using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float dashSpeed = 15f;
	public float jumpHeight = 2.0f;
	private float jumpVelocity;

	private bool onWall;
	private bool jump;
	private bool facingRight;
	private bool isGrounded;
	private Vector3 groundTangent;
	public float groundBias = 0.01f;
	
	private float moveing;

	private Vector3 scale;

	private Rigidbody rb;
	private GameObject playerSprite;

	void Start() {
		//Initializing
		rb = GetComponent<Rigidbody>();
		playerSprite = transform.Find("Player Sprite").gameObject;
		scale = playerSprite.transform.localScale;
		jumpVelocity = Mathf.Sqrt(2 * jumpHeight * -Physics.gravity.y);
	}

	void Update() {
		if (!jump) jump = Input.GetButtonDown("Jump");
		moveing = Input.GetAxis("Move");

		//If moveing direction is less then 0, then the player is faceing left
		if (moveing < 0) playerSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
		else if (moveing != 0) playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
	}

	void FixedUpdate() {
		//Calculate if player is on gorund, what andle it is and move player down if it is close to it
		RaycastHit hit;
		if (Physics.BoxCast(GetComponent<BoxCollider>().bounds.center, GetComponent<BoxCollider>().bounds.extents * 0.99f, Vector3.down, out hit, Quaternion.identity, groundBias)) {
			if (checkGround(hit.normal)) {
				transform.position += (hit.distance - GetComponent<BoxCollider>().bounds.extents.y * 0.01f) * Vector3.down;
			}
		} else {
			isGrounded = false;
		}

		//Calculate movement velocity
		if (isGrounded) {
			rb.velocity = moveing * moveSpeed * groundTangent;
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
				rb.velocity += new Vector3(rb.velocity.x, jumpVelocity);

				isGrounded = false;
			} else if (onWall) {
				//Initilizing faceing vector to left, for convenience
				int faceing = -1;

				//If facing left, faceing vector needs to be left
				if (facingRight)
					faceing = 1;

				//Adding dash and jump force
				rb.velocity += new Vector3(faceing * dashSpeed, jumpVelocity, 0);

				onWall = false;
			}
		
			jump = false;
		}

		onWall = false;
	}

	void OnCollisionStay(Collision col) {
		//If on gorund, then player can't be on wall as well
		if (isGrounded) return;

		foreach (ContactPoint con in col.contacts) {
			float slopeAngle = Vector3.Dot(con.normal, Vector3.up);

			Debug.Log(slopeAngle);

			if (slopeAngle < 0.5f && slopeAngle > -0.5f) {
				onWall = true;
			}
		}
	}

	private bool checkGround(Vector3 normal) {
		float slopeAngle = Vector3.Dot(normal, Vector3.up);

		//The player is on ground if the player is colliding with
		//someting that is less then 45 degress steep
		if (slopeAngle > 0.5f) {
			isGrounded = true;
			groundTangent = new Vector2(normal.y, -normal.x);

			return true;
		}

		isGrounded = false;
		return false;
	}
}