using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerScript))]

public class ControlScript : NetworkBehaviour {

	Rigidbody rb;
	PlayerScript player;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		if (rb == null) {
			rb = gameObject.AddComponent<Rigidbody>();
		}
		player = GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		rb.velocity = moveDir*5;

		var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(mouseRay, out hit, 999)) {
			Vector3 hitpoint = hit.point;
			hitpoint.y = transform.position.y;
			transform.LookAt(hitpoint, Vector3.up);
		}

		if (Input.GetMouseButtonDown(0)) {
			player.BaseAttack(transform.position, transform.forward);
		}

	}

	void LocalUpdate () {

	}

	void FixedUpdate () {

	}
}
