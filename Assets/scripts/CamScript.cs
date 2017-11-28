using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

	GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target != null){
			this.transform.position = new Vector3(
					target.transform.position.x,
					target.transform.position.y,
					transform.position.z
				);
		}
	}

	public void SetTarget (GameObject target) {
		this.target = target;
	}
}
