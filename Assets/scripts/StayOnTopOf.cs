using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StayOnTopOf : NetworkBehaviour {
    Transform target;
	[SyncVar]
	public string targetName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			var go = GameObject.Find(targetName);
			if (go != null) {
				target = go.transform;
			}
			return;
		}
        transform.position = new Vector3(target.position.x, target.position.y, -9f);
	}
}
