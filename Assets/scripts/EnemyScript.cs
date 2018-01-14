using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class EnemyScript : NetworkBehaviour {

	protected NetDog netDog;
    Rigidbody2D rb;
    public GameObject bloodSplatterPrefab;

	[SyncVar]
	public float moveSpeed = 4;
	[SyncVar]
	public float acceleration = 8;

    bool dead = false;

    [SyncVar]
    public float hp = 10f;
	[SyncVar]
    public float max_hp;

	public float tenacity = 1f;

	float stunTimer = 0;

	// Use this for initialization
	protected void Start () {
		netDog = FindObjectOfType<NetDog>();
        rb = GetComponent<Rigidbody2D>();
        netDog.AddEnemy(this);
        if (Network.isClient) {
		}
        max_hp = hp;
    }
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer) {
			ServerUpdate();
		}
		LocalUpdate();
	}

	protected virtual IEnumerable<Transform> GetTargets () {
		foreach (PlayerScript ps in netDog.players) {
			if(!ps.stealthed)
				yield return ps.transform;
		}
	}

	void LocalUpdate()
    {
        if (!dead)
        {
			if (stunTimer > 0) {
				stunTimer -= Time.deltaTime;
				return;
			}
            float closestD = float.MaxValue;
            Transform closestP = null;
            foreach (Transform t in GetTargets())
            {
               
                float d = Vector2.Distance(t.position, transform.position);
                if (d < closestD)
                {
                    closestP = t;
                    closestD = d;
                }
            }

            if (closestP != null)
            {
                Vector3 targetVel = (closestP.position - transform.position).normalized * moveSpeed;

                rb.velocity = Vector3.Lerp(rb.velocity, targetVel, acceleration * Time.deltaTime);

                transform.right = closestP.position - transform.position;
            }
        }
    }

	void ServerUpdate () {

	}

    [Server]
    public void TakeDamage (float damage)
    {
        hp -= damage;

		stunTimer += damage / max_hp / tenacity * 2f;

        if(hp < 0 && !dead)
        {
            dead = true;
            rb.drag = 25;
            rb.angularDrag = 50;

            var bloodSplatter = (GameObject)Instantiate(
            bloodSplatterPrefab,
            transform.position,
            Quaternion.identity);

            bloodSplatter.transform.localScale = transform.localScale;

            netDog.RemoveEnemy(this);

            NetworkServer.Spawn(bloodSplatter);
            StartCoroutine(Die());
        }
    }
    [Server]
    protected virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);

        NetworkServer.Destroy(gameObject);
    }
}
