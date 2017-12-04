using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PiercingArrowScript : ArrowScript {

    protected int numCollided = 0;
    public int maxCollisions = 4;

    protected List<GameObject> hitList = new List<GameObject>();
    

	// Use this for initialization
	protected new void Start () {
		base.Start();
		col.isTrigger = true;
    }

    // Update is called once per frame
    void Update () {

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collided && collision.transform.tag != "Arrow" && collision.transform.name != player)
        {
            if (!hitList.Contains(collision.gameObject))
            {
                hitList.Add(collision.gameObject);
            }
            else
            {
                return;
            }

            if (collision.transform.tag == "Enemy")
            {
				var es = collision.gameObject.GetComponent<EnemyScript>();
				if (es != null) {
					es.TakeDamage(damage);
				}

			}

            Rigidbody2D rb2 = collision.GetComponent<Rigidbody2D>();
            if (rb2 != null) {
                var trb = GetComponent<Rigidbody2D>();
                rb2.AddForce(trb.velocity * trb.mass, ForceMode2D.Impulse);
                trb.velocity -= trb.velocity * 0.1f;
                damage -= damage * 0.05f;
            }

            numCollided = hitList.Count;
           
            if (numCollided >= maxCollisions) {
                AttachTo(collision.transform);
            }
        }
    }
}
