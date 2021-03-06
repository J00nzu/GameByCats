﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetDog : NetworkBehaviour {

	public List<PlayerScript> players = new List<PlayerScript>();
    public List<EnemyScript> enemies = new List<EnemyScript>();
	public List<EnemyScript> spookyScarySkellingtons = new List<EnemyScript>();

	public PlayerScript localPlayer;
	Dog dog;

    public void SetLocalPlayer (PlayerScript player) {
		localPlayer = player;
		dog = GetComponent<Dog>();
		dog.OnPlayerSpawned(player.gameObject);
	}

	public void AddPlayer(PlayerScript player) {
		players.Add(player);
	} 

    public void AddEnemy(EnemyScript enemy)
    {
		if (enemy.tag == "Skellington") {
			spookyScarySkellingtons.Add(enemy);
		} else {
			enemies.Add(enemy);
		}
	}

    public void RemoveEnemy(EnemyScript enemy)
    {
		if (enemy.tag == "Skellington") {
			spookyScarySkellingtons.Remove(enemy);
		} else {
			enemies.Remove(enemy);
		}
	}
}
