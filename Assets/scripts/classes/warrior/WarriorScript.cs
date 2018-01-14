using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorScript : ClassScript {

	SwordScript sword;
	bool occupied = false;

	protected override void ClassStart () {
		sword = GetComponentInChildren<SwordScript>(true);
		sword.gameObject.SetActive(false);
	}

	protected override void LocalPlayerUpdate () {
		if (Input.GetMouseButton(0) && !occupied) {
			StartCoroutine(Swing());
		}
	}

	public IEnumerator Swing () {
		occupied = true;
		sword.gameObject.SetActive(true);
		yield return sword.SwingFast();
		sword.gameObject.SetActive(false);
		occupied = false;
	}

}
