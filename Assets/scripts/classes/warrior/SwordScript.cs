using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

	TrailRenderer trail;

	const float arc = 90;
	const float swingTime = 0.25f;

	void Start () {
		trail = GetComponentInChildren<TrailRenderer>(true);
	}

	public IEnumerator SwingFast () {
		RegenerateTail();
		float time = 0;
		while (time < swingTime) {
			time += Time.deltaTime;
			float angle = Mathf.LerpAngle(-arc, arc, time / swingTime);
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			yield return null;
		}
	}

	public IEnumerator PowerSwing () {
		yield return null;
	}

	public void RegenerateTail () {
		trail = GetComponentInChildren<TrailRenderer>(true);
		GameObject nu = Instantiate(trail.gameObject);
		nu.transform.parent = transform;
		nu.transform.SetPositionAndRotation(trail.transform.position, trail.transform.rotation);
		DestroyImmediate(trail.gameObject);
		trail = nu.GetComponent<TrailRenderer>();
	}
}
