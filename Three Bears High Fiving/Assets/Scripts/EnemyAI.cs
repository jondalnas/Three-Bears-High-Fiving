using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public GameObject player;

	[HideInInspector] public bool isAgro;

	void Start () {
	}

	void Update () {
		//If engine can draw a line between character and enemy, enemy gets agro
		if (Physics.Linecast(transform.position, player.transform.position)) isAgro = true;

		Debug.Log(Physics.Linecast(transform.position, player.transform.position));
	}
}
