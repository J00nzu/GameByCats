using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RicochetingArrowScript : ArrowScript {
        
    GameObject nearestEnemy;
    GameObject secondNearestEnemy;
    GameObject previousEnemy;

    // Use this for initialization
    void Start () {
        StartCoroutine(Destroy());
        col = GetComponent<Collider2D>();
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
                collided = true;
                previousEnemy = collider.gameObject;

                if (collider.gameObject.GetComponent<EnemyScript>() != null)
                {
                    collider.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
                    damage *= 0.75f;
                }


                Rigidbody2D rb2 = collider.GetComponent<Rigidbody2D>();
                if (rb2 != null)
                {
                    var trb = GetComponent<Rigidbody2D>();
                    rb2.AddForce(trb.velocity * trb.mass, ForceMode2D.Impulse);
                    trb.velocity -= trb.velocity * 0.2f;
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

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        if (collided)
            yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
