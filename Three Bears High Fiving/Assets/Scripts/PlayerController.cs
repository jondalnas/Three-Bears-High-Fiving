using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float jumpSpeed = 10f;
	public float boostSpeed = 15f;

	public Transform groundCheck;

	private bool onWall;
	private bool onGround;
	private bool jump;

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

		//Pressing Space will make the player jump
		if (onGround) {
			if (Input.GetAxis ("Jump") > 0)
				jump = true;
		}
	}

	void FixedUpdate() {
		//Creating velocity vector, this will be used to make the character walk
		Vector3 vel = Vector3.zero;

		//Moving the chracter by pressing the a/d buttons
		vel.x += Input.GetAxis("Horizontal");

		//Not applying y velocity, because y is gravity and we don't want to mess with that, except when it's on the wall, that means it shouldn't fall down
		if (!onWall) rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));

		//Move player by vel*moveSpeed
		rb.AddForce(Vector3.right*vel.x*moveSpeed);

		//If jumping, jump!
		if (jump) {
			rb.AddForce(Vector3.up*vel.y*jumpSpeed);
			jump = false;
		}
	}

	public void OnCollisionEnter(Collision col) {
	    ContactPoint[] cols = col.contacts;
	    foreach (ContactPoint con in cols) {
	        heading = Vector3.RotateTowards(transform.position, con.point, Mathf.Infinity, 0.0f).normalized;

	        if (heading.x > heading.y) {
	            onWall = true;
	        }
	        else if (heading.y > 0) {
	            onGround = true;
	        }
	    }
	}

	public void OnCollisionExit(Collision col) {
		ContactPoint[] cols = col.contacts;
		foreach (ContactPoint con in cols) {
			heading = Vector3.RotateTowards(transform.position, con.point, Mathf.Infinity, 0.0f).normalized;

			if (heading.x > heading.y) {
				onWall = false;
			}
			else if (heading.y > 0) {
				onGround = true;
			}
		}
	}
}