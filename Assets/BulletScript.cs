using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour {

    public bool collided = false;
    [SyncVar]
    public string player;

	// Use this for initialization
	void Start () {
        StartCoroutine(Destroy());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collided && collision.collider.transform.tag != "bullet" && collision.transform.name != player)
        {
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.transform;
            collided = true;
            Destroy(GetComponent<Rigidbody2D>());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        if (collided)
            yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
