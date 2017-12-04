using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	public GameObject ArrowPrefab;
    public GameObject RicochetingArrowPrefab;
    public GameObject SelectedArrow;
    public string playerName;

	// Use this for initialization
	void Start () {
        playerName = "player_" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
        SelectedArrow = ArrowPrefab;
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
    [Command]
    public void Cmd_ChangeArrowType(string arrowName)
    {
        if(arrowName == "Normal")
        {
            SelectedArrow = ArrowPrefab;
        }
        else if(arrowName == "Ricocheting")
        {
            SelectedArrow = RicochetingArrowPrefab;
        }
    }

    [Command]
	public void Cmd_BaseAttack (Vector2 position, Vector2 direction, float damageMultiplier, float speedMultiplier, bool piercing) {
		
        // Create the Arrow from the Arrow Prefab
		var Arrow = (GameObject)Instantiate(
			SelectedArrow,
			position + direction*1, 
			Quaternion.identity);

		Arrow.transform.right = direction;

		// Add velocity to the Arrow
		Arrow.GetComponent<Rigidbody2D>().velocity = Arrow.transform.right * 16 * speedMultiplier;
        
        var bscript = Arrow.GetComponent<ArrowScript>();
        bscript.player = playerName;
        bscript.damage *= damageMultiplier;
        if (piercing) {
            bscript.SetPiercing(piercing);
        }
        NetworkServer.Spawn(Arrow);
	}
}
