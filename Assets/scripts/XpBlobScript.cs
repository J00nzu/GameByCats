using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpBlobScript : MonoBehaviour {

    Transform target;
    protected NetDog netDog;

    float moveSpeed = 5;

    // Use this for initialization
    void Start () {
        netDog = FindObjectOfType<NetDog>();
        target = netDog.localPlayer.transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            Vector3 targetVel = (target.position - transform.position).normalized * moveSpeed;

            transform.position += targetVel * Time.deltaTime;

            transform.right = target.position - transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
