using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {
	public GameObject particalsystem;
    public GameObject brokenWindow;

	public float explotionSize = 1;
	public float explotionPower = 4;

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

		for (int i = 0; i < transform.Find("Broken Window Pieces").transform.childCount; i++) {
			transform.Find("Broken Window Pieces").transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(explotionPower, col.transform.position, explotionSize);
		}
        
		ps.Play();

		GetComponent<BoxCollider>().enabled = false;
	}
}
