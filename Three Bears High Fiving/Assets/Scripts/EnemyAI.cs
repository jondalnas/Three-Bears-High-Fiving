using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public GameObject player;

	public Transform camPos;
	public Transform head;
	public Transform[] checkpoints;

	public float[] wait;
	public float speed;

	[HideInInspector] public bool isAgro;

	private float waitingTimer;

	private int checkpoint;

	private bool walking;

	private Vector3 direction;

	void Update () {
		//If engine can draw a line between character and enemy witout hitting anything (except player), enemy gets agro
		RaycastHit raycastHit;
		if (Physics.Raycast(camPos.position, (player.transform.position-camPos.position).normalized, out raycastHit) && raycastHit.collider.CompareTag("Player")) isAgro = true;

		//If not agro, the enemy will walk between checkpoints
		if (!isAgro) {
			//Making the enemy wait until walking time has hit timer, then rotate
			if (waitingTimer >= wait [checkpoint]&&!walking) {
				walking = true;
				transform.Rotate(new Vector3(0, 180, 0));
			} else waitingTimer += Time.deltaTime;

			if (walking) {
				//Initilizing direction
				if (direction == Vector3.zero)
					direction = getNextCheckpointDirection();

				//Moving player in set direction
				transform.position += direction*speed*Time.deltaTime;

				//Checking direction
				if (direction == Vector3.right) {
					//If collides or is grater then checkpoint, then reset everything and turn around
					if (transform.position.x >= getNextCheckpoint().position.x) {
						walking = false;
						waitingTimer = 0;
						direction = Vector3.zero;
						checkpoint++;
						if (checkpoint >= checkpoints.Length)
							checkpoint = 0;
					}
				} else if (direction == Vector3.left) {
					if (transform.position.x <= getNextCheckpoint().position.x) {
						walking = false;
						waitingTimer = 0;
						direction = Vector3.zero;
						checkpoint++;
						if (checkpoint >= checkpoints.Length)
							checkpoint = 0;
					}
				}
			}
		} else {
			Vector3 dir = player.transform.position - transform.position;
			head.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
		}
	}

	Vector3 getNextCheckpointDirection() {
		//Creating direction
		Vector3 result = (getNextCheckpoint().position-checkpoints[checkpoint].position);

		//Initilizing direction
		result.y = 0;
		result.z = 0;

		result.Normalize();

		return result;
	}

	Transform getNextCheckpoint() {
		//Returning the next checkpoint
		if (checkpoint+1 >= checkpoints.Length)
			return checkpoints[checkpoint+1-checkpoints.Length];
		return checkpoints[checkpoint+1];
	}
}
