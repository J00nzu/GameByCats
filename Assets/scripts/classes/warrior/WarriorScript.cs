using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorScript : ClassScript {

	PlayerScript player;

	// Use this for initialization
	void Start () {
		player = GetComponentInChildren<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
