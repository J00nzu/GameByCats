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
        GetComponent<SpriteRenderer>().color = Color.green;
		var nd = FindObjectOfType<NetDog>();
		nd.SetLocalPlayer(this);
		nd.AddPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BaseAttack (Vector2 position, Vector2 direction) {
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			position + direction*2, 
			Quaternion.identity);

		bullet.transform.right = direction;

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 16;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
