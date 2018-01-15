using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaveSpawnerScript : NetworkBehaviour {
    
    public int enemiesToSpawn = 3;
    //public int wavesToSpawn = 5;
    public float timeBetweenWaves = 3f;

	public int enemyIncrease = 3;

	public int maxConcurrentEnemies = 15;

	public float dividingWave = 3;
	public float hiveWave = 6;

    public GameObject enemyPrefab;
	public GameObject dividingPrefab;
	public GameObject hivePrefab;

	NetDog dog;

	WaveTextScript wts;


	// Use this for initialization
	void Start () {
		dog = FindObjectOfType<NetDog>();
		wts = FindObjectOfType<WaveTextScript>();
        StartCoroutine(SpawnWave());
	}

    [Server]
    IEnumerator SpawnWave()
    {
        for (int i=1;;i++)
        {
			WaveText(i);
			yield return new WaitForSeconds(timeBetweenWaves);
			for (int j = 0; j < enemiesToSpawn; j++)
            {
                var position = Random.insideUnitCircle * 25;

                var enemy = (GameObject)Instantiate(
                selectEnemy(i),
                position,
                Quaternion.identity);
                NetworkServer.Spawn(enemy);
                yield return new WaitForSeconds(Random.Range(0f, 2.5f));
				while (dog.enemies.Count > maxConcurrentEnemies) {
					yield return null;
				}
			}
			while (dog.enemies.Count > 0) {
				yield return null;
			}
			IncreaseWave();
		}
    }

	GameObject selectEnemy (int wave) {
		if (wave >= dividingWave && Random.Range(0, 10) == 0) {
			return dividingPrefab;
		} else if (wave >= hiveWave && Random.Range(0, 20) == 0) {
			return hivePrefab;
		} else {
			return enemyPrefab;
		}
	}

	void IncreaseWave () {
		enemiesToSpawn += enemyIncrease;
	}

	void WaveText (int i) {
		if (wts != null) {
			StartCoroutine(wts.fadeText(i));
		}
	}
}
