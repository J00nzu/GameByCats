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

	[SyncVar]
	float stunTimer = 0;

	protected float damage = 5;

	float attackCooldown = 2f;
	float atkcd = 0f;

	public GameObject attackEffectPrefab;

	// Use this for initialization
	protected void Start () {
		netDog = FindObjectOfType<NetDog>();
        rb = GetComponent<Rigidbody2D>();
        netDog.AddEnemy(this);
        max_hp = hp;
    }
	
	// Update is called once per frame
	void Update () {
		ServerUpdate();
		LocalUpdate();
	}

	protected virtual IEnumerable<Transform> GetTargets () {
		foreach (PlayerScript ps in netDog.players) {
			if(!ps.stealthed)
				yield return ps.transform;
		}
		foreach (EnemyScript es in netDog.enemies) {
			if (es.tag == "Skellington") {
				yield return es.transform;
			}
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

	[Server]
	void ServerUpdate () {
		if (dead) return;

		atkcd -= Time.deltaTime;
	}

	void OnCollisionEnter2D (Collision2D coll) {
		ServerCollision2D(coll);
	}

	[Server]
	void ServerCollision2D(Collision2D coll) {

		if (atkcd > 0)
			return;

		Collider2D c = coll.collider;

		List<Transform> l = new List<Transform>();
		l.AddRange(GetTargets());
		if (l.Contains(c.transform)) {

			Rpc_AttackAnim(c.transform.position);
			var es = c.GetComponent<EnemyScript>();
			var ps = c.GetComponent<PlayerScript>();

			if (es != null) {
				es.TakeDamage(damage);
			} else if (ps != null && !ps.ouchGoing && !ps.stealthed) {
				ps.Rpc_TakeDamage(damage);
			} else {
				return;
			}

			Rigidbody2D rb2 = c.GetComponent<Rigidbody2D>();
			if (rb2 != null) {
				var dir = (c.transform.position - transform.position).normalized;
				rb2.AddForce(dir * 15, ForceMode2D.Impulse);
			}

			atkcd = attackCooldown;
		}
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

			Rpc_RemoveFromList();

			NetworkServer.Spawn(bloodSplatter);
            StartCoroutine(Die());
        }
    }

	[ClientRpc]
	protected void Rpc_RemoveFromList () {
		netDog.RemoveEnemy(this);
	}
	[Server]
    protected virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);

        NetworkServer.Destroy(gameObject);
    }

	[ClientRpc]
	void Rpc_AttackAnim (Vector3 TargetPosition) {

		return; // Unimplemented

		if (attackEffectPrefab == null) {
			Debug.Log("Attack effect prefab missing!");
			return;
		}
		Vector3 dirVec = (TargetPosition - transform.position).normalized;
		var effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity, transform);

		effect.transform.localPosition = new Vector3(0, 0, -2);
		effect.transform.right = dirVec;

		Destroy(effect, 2);
	}
}
