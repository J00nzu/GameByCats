using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

	CamScript mainCam;

	// Use this for initialization
	void Start () {
		mainCam = FindObjectOfType<CamScript>();
		if (mainCam == null) {
			mainCam = Camera.main.gameObject.AddComponent<CamScript>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 RequestStartPosition () {
		return new Vector3(0, 1, 0);
	}

	public void OnPlayerSpawned (GameObject player) {
		player.AddComponent<ControlScript>();
		FindObjectOfType<CamScript>().SetTarget(player);
	}
}
