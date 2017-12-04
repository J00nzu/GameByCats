using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    public string playerName;

	// Use this for initialization
	void Start () {
        playerName = "player_" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
        var nd = FindObjectOfType<NetDog>();
        nd.AddPlayer(this);
    }



    public override void OnStartLocalPlayer () {
        Debug.Log("bleep");
        GetComponent<SpriteRenderer>().color = Color.green;
		var nd = FindObjectOfType<NetDog>();
		nd.SetLocalPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
