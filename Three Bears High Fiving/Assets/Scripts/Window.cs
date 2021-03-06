﻿using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {
	public GameObject particalsystem;
    public GameObject brokenWindow;

	private ParticleSystem ps;

	void Start() {
		ps = particalsystem.GetComponent<ParticleSystem>();
	}

	void OnCollisionEnter(Collision col) {
        transform.Find("Window Object").gameObject.SetActive(false);
        transform.Find("Broken Window Object").gameObject.SetActive(true);
        transform.Find("Broken Window Pieces").gameObject.SetActive(true);

        Vector3 dir = transform.position-col.transform.position;

		Vector3 distance;

		if (dir.x > 0) {
			distance = new Vector3(20, 0, 0);
		} else {
			distance = new Vector3(-20, 0, 0);
		}
        
		ps.Play();

		GetComponent<BoxCollider>().enabled = false;
	}
}
