using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextScript : MonoBehaviour {

	Text text;
	float fadeTime = 2;
	float stayTime = 2;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		text.enabled = false;
	}

	public IEnumerator fadeText (int wave) {
		text.text = "Wave " + wave;
		text.enabled = true;

		for (float f = 0; f < fadeTime; f += Time.deltaTime) {
			float a = f / fadeTime;
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			yield return null;
		}

		yield return new WaitForSeconds(stayTime);

		for (float f = fadeTime; f > 0; f -= Time.deltaTime) {
			float a = f / fadeTime;
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			yield return null;
		}
		text.enabled = false;
	}
	
}
