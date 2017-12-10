using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HiveScript : NetworkBehaviour {

    public GameObject enemyPrefab;
    bool dead;
    [SyncVar]
    public float hp = 50;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemy());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Server]
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            dead = true;
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (!dead)
        {
            var position = Random.insideUnitCircle * 2;
            var enemy = (GameObject)Instantiate(
            enemyPrefab,
            position + (Vector2) transform.position,
            Quaternion.identity);
            NetworkServer.Spawn(enemy);
            yield return new WaitForSeconds(5f);
        }
    }
}
