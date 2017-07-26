using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float dashSpeed = 15f;
	public float jumpHeight = 2.0f;
	public float gravity = 9.81f;

	private bool onWall;
	private bool jump;
	private bool wallDash;
	private bool facingRight;
	private bool isJumping;
	private bool isGrounded;
	
	private float moveing;
	private float secInAir;

	private Vector3 heading;
	private Vector3 scale;

	private Rigidbody rb;
	private GameObject playerSprite;

	void Start() {
		//Initializing
		rb = GetComponent<Rigidbody>();
		playerSprite = transform.FindChild("Player Sprite").gameObject;
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
		//Calculate character speed
		Vector3 playerVel = Vector3.right * moveing * moveSpeed;

		//Calculate actual velocity
		rb.AddForce(playerVel, ForceMode.Force);

		//Pressing Space will make the player jump or dash if it is colliding with wall
		if (jump) {
			if (!isJumping) {
				if (isGrounded) {
					rb.AddForce(new Vector3(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), rb.velocity.z), ForceMode.Force);
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
			Vector3 vel = new Vector3(faceing*dashSpeed, Mathf.Sqrt(2 * jumpHeight * gravity), 0);
			rb.AddForce(vel, ForceMode.Force);

			wallDash = false;
		}

		if (!isGrounded) {
			Debug.Log(rb.velocity + ", " + secInAir);
			secInAir += Time.deltaTime;
		}
		else
			secInAir = 1;

		rb.AddForce(new Vector3(0, CalculateGravity(secInAir), 0), ForceMode.Acceleration);

		isGrounded = false;
		onWall = false;
		jump = false;
	}

	float CalculateGravity(float secInAir) {
		return 0.5f * -gravity * (secInAir*secInAir);
	}

	void OnCollisionStay(Collision col) {
		foreach (ContactPoint contact in col.contacts) {
			//FIX ME!!!
			//The player is on ground if the player is colliding with
			//someting that is less then 45 degress steep
			if ((-contact.normal).x<=0.6f && (-contact.normal).x>=-0.6f) {
				if ((-contact.normal).y <= -0.6f) {
					isGrounded = true;
					return;
				}
			}

			if ((-contact.normal).y >= -0.6f && (-contact.normal).y <= 0.6f) {
				onWall = true;
			}
		}
	}
}