using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaveSpawnerScript : NetworkBehaviour {
    
    public int enemiesToSpawn = 5;
    public int wavesToSpawn = 5;
    public float timeBetweenWaves = 20f;

    public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnWave());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [Server]
    IEnumerator SpawnWave()
    {
        for (int i = 0; i < wavesToSpawn; i++)
        {
            for (int j = 0; j < enemiesToSpawn; j++)
            {
                var position = Random.insideUnitCircle * 25;
                var enemy = (GameObject)Instantiate(
                enemyPrefab,
                position,
                Quaternion.identity);
                NetworkServer.Spawn(enemy);
                yield return new WaitForSeconds(Random.Range(0f, 2.5f));
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}
