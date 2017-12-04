using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowScript : NetworkBehaviour {

    [SyncVar]
    public float damage = 1f;
    [SyncVar]
    public string player;
    [SyncVar (hook = "PierceHook")]
    protected bool piercing;

    public bool collided = false;

    protected Collider2D col;

    protected int numCollided = 0;
    protected int maxCollisions = 4;

    protected List<GameObject> hitList = new List<GameObject>();
    

	// Use this for initialization
	void Start () {
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

            if (collision.transform.tag == "Enemy" && !hitList.Contains(collision.gameObject))
            {
                collision.collider.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            }
        }

    }

    private void _printlist<T>(List<T> l) {
        if (l.Count == 0)
        {
            Debug.Log(l);
            return;
        }

        string doggo = "[";
        foreach (T t in l) {
            doggo += t;
            doggo += ", ";
        }
        doggo = doggo.Substring(0, doggo.Length - 2);
        doggo += "]";
        Debug.Log(doggo);
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
                if(collision.gameObject.GetComponent<EnemyScript>() != null)
                    collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
                
            }

            Rigidbody2D rb2 = collision.GetComponent<Rigidbody2D>();
            if (rb2 != null) {
                var trb = GetComponent<Rigidbody2D>();
                rb2.AddForce(trb.velocity * trb.mass, ForceMode2D.Impulse);
                trb.velocity -= trb.velocity * 0.2f;
                damage -= damage * 0.2f;
            }

            numCollided = hitList.Count;
           
            if (numCollided >= maxCollisions) {
                AttachTo(collision.transform);
            }
        }
    }

    void AttachTo(Transform t) {
        col.enabled = false;
        transform.parent = t;
        collided = true;
        Destroy(GetComponent<Rigidbody2D>());
    }

    private void PierceHook(bool newval)
    {
        SetPiercing(newval);
    }

    public void SetPiercing(bool pierc) {
        GetComponent<Collider2D>().isTrigger = pierc;
        piercing = pierc;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        if (collided)
            yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}
