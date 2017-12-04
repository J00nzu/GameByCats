using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowScript : NetworkBehaviour {

    [SyncVar]
    public float damage = 4f;
    [SyncVar]
    public string player;

    public bool collided = false;

    protected Collider2D col;    

	// Use this for initialization
	protected void Start () {
        StartCoroutine(Destroy());
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update () {

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collided && collision.collider.transform.tag != "Arrow" && collision.transform.name != player)
        {
            transform.Find("Trail").GetComponent<TrailRenderer>().enabled = false;

            AttachTo(collision.transform);

            if (collision.transform.tag == "Enemy")
            {
                collision.collider.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            }
        }

    }

	protected void AttachTo(Transform t) {
        col.enabled = false;
        transform.parent = t;
        collided = true;
        Destroy(GetComponent<Rigidbody2D>());
    }

    virtual protected IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        if (collided)
            yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}
