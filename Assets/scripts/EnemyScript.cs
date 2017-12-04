using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class EnemyScript : NetworkBehaviour {

	NetDog netDog;
    Rigidbody2D rb;
    public GameObject pas;

    float moveSpeed = 5;
    float acceleration = 10;

    bool dead = false;

    [SyncVar]
    public float hp = 10f;

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
        if (!dead)
        {
            float closestD = float.MaxValue;
            PlayerScript closestP = null;
            foreach (PlayerScript ps in netDog.players)
            {
                float d = Vector2.Distance(ps.transform.position, transform.position);
                if (d < closestD)
                {
                    closestP = ps;
                    closestD = d;
                }
            }

            if (closestP != null)
            {
                Vector3 targetVel = (closestP.transform.position - transform.position).normalized * moveSpeed;

                rb.velocity = Vector3.Lerp(rb.velocity, targetVel, acceleration * Time.deltaTime);

                transform.right = closestP.transform.position - transform.position;
            }
        }
    }

	void ServerUpdate () {

	}

    [Server]
    public void TakeDamage (float damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            dead = true;
            rb.drag = 25;
            rb.angularDrag = 50;

            var bloodSplatter = (GameObject)Instantiate(
            pas,
            transform.position,
            Quaternion.identity);

            NetworkServer.Spawn(bloodSplatter);
            StartCoroutine(Die());
        }
    }
    [Server]
    IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);

        NetworkServer.Destroy(gameObject);
    }
}
