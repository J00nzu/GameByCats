using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DividingEnemyScript : EnemyScript {


	protected new void Start () {
		base.Start();
	}

	[Server]
    protected override IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < 2; i++)
        {
            if (transform.localScale.x > 0.8f)
            {
				GameObject pref = Resources.Load("prefabs/dividing_Enemy") as GameObject;
                var position = Random.insideUnitCircle * 0.5f;
                var enemy = (GameObject)Instantiate(
                    pref,
                    transform.position + (Vector3)position,
                    Quaternion.identity);
				
				var es = enemy.GetComponent<DividingEnemyScript>();

				es.hp = max_hp * 0.5f;
				es.max_hp = es.hp;
				es.moveSpeed = moveSpeed + 2f;
				es.acceleration = acceleration + 1f;

				NetworkServer.Spawn(enemy);
				es.Rpc_ChangeSize(transform.localScale.x * 0.6f);
            }
        }
        
        NetworkServer.Destroy(gameObject);
    }

	[ClientRpc]
	public void Rpc_ChangeSize (float size) {
		transform.localScale = Vector3.one * size;
	}
}
