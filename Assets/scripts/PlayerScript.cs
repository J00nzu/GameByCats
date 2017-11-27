using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
		
	}

	public override void OnStartLocalPlayer () {
		GetComponent<Renderer>().material.color = Color.green;
		var nd = FindObjectOfType<NetDog>();
		nd.SetLocalPlayer(this);
		nd.AddPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BaseAttack (Vector3 position, Vector3 direction) {
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			position + direction*2, 
			Quaternion.identity);

		bullet.transform.forward = direction;

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 16;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
