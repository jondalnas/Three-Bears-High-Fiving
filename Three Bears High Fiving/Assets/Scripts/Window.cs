using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		Vector3 dir = transform.position-col.transform.position;
		Debug.Log(dir);
	}
}
