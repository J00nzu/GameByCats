using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetGuard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (FindObjectOfType<NetworkManager>() == null) {
			SceneManager.LoadScene(0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
