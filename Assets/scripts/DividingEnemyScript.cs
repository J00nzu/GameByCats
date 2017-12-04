using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DividingEnemyScript : EnemyScript {

    public GameObject dividingEnemyPrefab;

	protected new void Start () {
		base.Start();
	}



	[Server]
    protected override IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < 2; i++)
        {
            if (transform.localScale.x > 0.5f)
            {
                var position = Random.insideUnitCircle * 0.5f;
                var enemy = (GameObject)Instantiate(
                    dividingEnemyPrefab,
                    transform.position + (Vector3)position,
                    Quaternion.identity);
                enemy.transform.localScale = transform.localScale * 0.5f;

				var es = enemy.GetComponent<EnemyScript>();

				es.hp = max_hp * 0.5f;
				es.max_hp = es.hp;
				es.moveSpeed = moveSpeed + 2f;
				es.acceleration = acceleration + 1f;

				NetworkServer.Spawn(enemy);
            }
        }
        
        NetworkServer.Destroy(gameObject);
    }
}
