using UnityEngine;
using System.Collections;

public class HeadCollider : MonoBehaviour {
	[HideInInspector]
	public bool hitHead;

	void Update() {
		hitHead = false;
	}

	void OnCollisionStay(Collision col) {
		Debug.Log(col.gameObject);
		//If Object collides with head, then hitHead equals true
		if (!col.gameObject.CompareTag("Player"))
			hitHead = true;
	}
}
