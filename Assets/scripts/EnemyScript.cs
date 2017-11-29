using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class EnemyScript : NetworkBehaviour {

	NetDog netDog;
    Rigidbody2D rb;

    float moveSpeed = 5;
    float acceleration = 10; 

	// Use this for initialization
	void Start () {
		netDog = FindObjectOfType<NetDog>();
        rb = GetComponent<Rigidbody2D>();
		if (Network.isClient) {
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer) {
			ServerUpdate();
		}
		LocalUpdate();
	}

    void LocalUpdate()
    {
        float closestD = 100;
        PlayerScript closestP = null;

        foreach (PlayerScript ps in netDog.players)
        {
            float d = (ps.transform.position - transform.position).magnitude;
            if (d < closestD)
            {
                closestP = ps;
            }
        }

        if (closestP != null)
        {
            Vector3 targetVel = (closestP.transform.position - transform.position).normalized * moveSpeed;

            rb.velocity = Vector3.Lerp(rb.velocity, targetVel, acceleration * Time.deltaTime);

            transform.right = closestP.transform.position - transform.position;
        }
    }

	void ServerUpdate () {

	}
}
