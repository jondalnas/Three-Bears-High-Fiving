using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {
	public GameObject particalsystem;

	private ParticleSystem ps;

	void Start() {
		ps = particalsystem.GetComponent<ParticleSystem>();
	}

	void OnCollisionEnter(Collision col) {
		Vector3 dir = transform.position-col.transform.position;

		Vector3 distance;

		if (dir.x > 0) {
			distance = new Vector3(20, 0, 0);
		} else {
			distance = new Vector3(-20, 0, 0);
		}

		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
		int count = ps.GetParticles(particles);

		for (int i = 0; i < count; i++) {
			particles[i].velocity = (particles[i].lifetime / particles[i].startLifetime) * distance;
		}

		ps.SetParticles(particles, count);

		Debug.Log(count);

		ps.Play();

		GetComponent<BoxCollider>().enabled = false;
	}
}
