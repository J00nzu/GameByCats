using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawBarScript : MonoBehaviour {

    public GameObject drawBPrefab;

    UnityEngine.UI.Slider drawBar;

	// Use this for initialization
	void Start () {
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;

        drawBar = Instantiate(drawBPrefab).GetComponent<Slider>();

        drawBar.transform.parent = canvas.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDrawBar(float percent, Vector3 position, Vector3 direction) {
        if (percent == 0)
        {
            drawBar.gameObject.SetActive(false);
        }
        else {
            drawBar.gameObject.SetActive(true);
            drawBar.value = percent;
            drawBar.transform.position = Camera.main.WorldToScreenPoint(position);
            drawBar.transform.right = direction;
        }
    }
}
