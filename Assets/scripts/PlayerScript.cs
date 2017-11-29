using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	public GameObject bulletPrefab;
    public string playerName;

	// Use this for initialization
	void Start () {
        playerName = "player_" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
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

    [Command]
	public void Cmd_BaseAttack (Vector2 position, Vector2 direction) {
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			position + direction*1, 
			Quaternion.identity);

		bullet.transform.right = direction;

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 16;

        BulletScript bscript = bullet.GetComponent<BulletScript>();
        bscript.player = playerName;
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
	}
}
