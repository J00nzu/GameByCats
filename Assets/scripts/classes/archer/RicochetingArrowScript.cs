using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RicochetingArrowScript : PiercingArrowScript {
        
    GameObject nearestEnemy;
    GameObject secondNearestEnemy;
    GameObject previousEnemy;

	// Use this for initialization
	protected new void Start () {
		base.Start();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag != "Arrow" && collider.transform.name != player)
        {
            if (!hitList.Contains(collider.gameObject))
            {
                hitList.Add(collider.gameObject);
            }
            else
            {
                return;
            }

            if (collider.transform.tag == "Enemy")
            {
                float nearestDistance = float.MaxValue;
                float distance;
                previousEnemy = collider.gameObject;

                if (collider.gameObject.GetComponent<EnemyScript>() != null)
                {
					var es = collider.gameObject.GetComponent<EnemyScript>();
					if (es != null) {
						es.TakeDamage(damage);
					}
					damage *= 0.75f;
                }


                Rigidbody2D rb2 = collider.GetComponent<Rigidbody2D>();
                if (rb2 != null)
                {
                    var trb = GetComponent<Rigidbody2D>();
                    rb2.AddForce(trb.velocity * trb.mass, ForceMode2D.Impulse);
                    trb.velocity -= trb.velocity * 0.2f;
                }

				numCollided = hitList.Count;

				if (numCollided >= maxCollisions) {
					AttachTo(collider.transform);
					transform.Find("Trail").GetComponent<TrailRenderer>().enabled = false;
					return;
				}

				Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 50f);
                foreach (Collider2D enemyCol in hitColliders)
                {
                    if (enemyCol.transform.tag == "Enemy" && !hitList.Contains(enemyCol.gameObject))
                    {
                        distance = Vector2.Distance(transform.position, enemyCol.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            secondNearestEnemy = nearestEnemy;
                            nearestEnemy = enemyCol.gameObject;

                        }
                    }
                }

                if (nearestEnemy != previousEnemy && nearestEnemy != null) {
                    transform.right = (nearestEnemy.transform.position - transform.position).normalized;
                }
                else if(secondNearestEnemy != null)
                    transform.right = (secondNearestEnemy.transform.position - transform.position).normalized;
                var rb2d = GetComponent<Rigidbody2D>();
                rb2d.velocity = transform.right * rb2d.velocity.magnitude;
            }
        }

    }

    protected override IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4f);
        if (collided)
            yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}
