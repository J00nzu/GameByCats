using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DividingEnemyScript : EnemyScript {

    public GameObject dividingEnemyPrefab;

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
                enemy.GetComponent<EnemyScript>().hp = max_hp * 0.5f;
                NetworkServer.Spawn(enemy);
            }
        }
        
        NetworkServer.Destroy(gameObject);
    }
}
