using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    public string playerName;

    public bool stealthed;

    public GameObject playerLightPrefab;
    GameObject playerLight;

	// Use this for initialization
	void Start () {
        playerName = "player_" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
        var nd = FindObjectOfType<NetDog>();
        nd.AddPlayer(this);
        playerLight = Instantiate(playerLightPrefab, transform.position, Quaternion.identity) as GameObject;
        playerLight.GetComponent<StayOnTopOf>().target = transform;
    }



    public override void OnStartLocalPlayer () {
        //GetComponent<SpriteRenderer>().color = Color.green;
		var nd = FindObjectOfType<NetDog>();
		nd.SetLocalPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
