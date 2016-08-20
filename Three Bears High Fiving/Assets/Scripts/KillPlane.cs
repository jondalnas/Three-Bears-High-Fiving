using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillPlane : MonoBehaviour {
	public List<GameObject> blackList = new List<GameObject>();

	void OnTriggerEnter(Collider col) {
		if (!blackList.Contains(col.gameObject)) Destroy(col.gameObject);
	}
}
