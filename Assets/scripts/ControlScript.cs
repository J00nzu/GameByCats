using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerScript))]

public class ControlScript : NetworkBehaviour {

	Rigidbody2D rb;
	PlayerScript player;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		if (rb == null) {
			rb = gameObject.AddComponent<Rigidbody2D>();
		}
		player = GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer || player.ouchGoing) {
			return;
		}
        
        Vector2 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
		var targetVel = moveDir*5;
		rb.velocity = Vector3.Lerp(rb.velocity, targetVel, 4 * Time.deltaTime);


		Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = mouseScreenPosition - (Vector2) transform.position;        

		
		/*
        if (Input.GetKey(KeyCode.Alpha1))
        {
            player.Cmd_ChangeArrowType("Normal");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            player.Cmd_ChangeArrowType("Ricocheting");
        }*/
    }
}
