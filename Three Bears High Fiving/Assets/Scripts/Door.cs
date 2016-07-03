using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public GameObject destroyedDoor;
	public GameObject player;

	public float explotionSize;
	public float explotionPower;

	private bool isDestroyed;

	void OnTriggerEnter(Collider col) {
		//If the door is allready destryed, don't destroy it
		if (!isDestroyed) {
			//If it isn't player colliding, then don't do anything
			if (col.gameObject.CompareTag("Player")) {
				//Removeing all children
				foreach (Transform child in transform) {
					Destroy(child.gameObject);
				}

				//Creating new child object
				GameObject childObject = Instantiate(destroyedDoor, transform.position, transform.rotation) as GameObject;

				//Setting the child object's parrent to this object
				childObject.transform.parent = transform;

				//Getting all child objects of childObject and adding an explotion force
				for (int i = 0; i < childObject.transform.GetChildCount(); i++) {
					childObject.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(explotionPower, player.transform.position, explotionSize);
				}

				isDestroyed = true;
			}
		}
	}
}
