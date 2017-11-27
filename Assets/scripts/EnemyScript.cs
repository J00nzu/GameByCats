using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyScript : NetworkBehaviour {

	NetDog netDog;
	NavMeshAgent nav;


	// Use this for initialization
	void Start () {
		netDog = FindObjectOfType<NetDog>();
		nav = GetComponent<NavMeshAgent>();
		if (Network.isClient) {
			nav.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer) {
			ServerUpdate();
		}
		LocalUpdate();
	}

	void LocalUpdate () {
		float closestD = 100;
		PlayerScript closestP = null;

		foreach (PlayerScript ps in netDog.players) {
			float d = (ps.transform.position - transform.position).magnitude;
			if (d < closestD) {
				closestP = ps;
			}
		}

		if (closestP != null) {
			nav.SetDestination(closestP.transform.position);
		}
	}

	void ServerUpdate () {

	}
}
