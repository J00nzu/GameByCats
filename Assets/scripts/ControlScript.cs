using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerScript))]

public class ControlScript : NetworkBehaviour {

	Rigidbody2D rb;
	PlayerScript player;

    public DrawBarScript loadBar;

    public float timeSinceLastShot = 0;
    float maxLastShotTimer = 1f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		if (rb == null) {
			rb = gameObject.AddComponent<Rigidbody2D>();
		}
		player = GetComponent<PlayerScript>();

        loadBar = GetComponent<DrawBarScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
        
        timeSinceLastShot += Time.deltaTime;

        Vector2 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
		rb.velocity = moveDir*5;

        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = mouseScreenPosition - (Vector2) transform.position;        

		if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(ShotTimer());
		}

        if (Input.GetKey(KeyCode.Alpha1))
        {
            player.ChangeArrowType("Normal");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            player.ChangeArrowType("Ricocheting");
        }
    }

    IEnumerator ShotTimer()
    {
        float damageMultiplier = 1f;
        float speedMultiplier = 1f;
        float holdTimer = 0;
        bool piercing = false;

        while (Input.GetMouseButton(0)) {
            holdTimer += Time.deltaTime;
            loadBar.SetDrawBar(Mathf.Clamp01((holdTimer-0.2f) / 3.0f), transform.position + transform.right, transform.right);
            yield return null;
        }

        loadBar.SetDrawBar(0, Vector3.zero, Vector3.zero);


        if (holdTimer > 0.5f)
        {
            piercing = true;
            damageMultiplier *= Mathf.Clamp(holdTimer, 0.5f, 3f) * 4f;
            speedMultiplier *= Mathf.Clamp(holdTimer, 0.5f, 2f) * 1.5f;
        }
        else if (timeSinceLastShot < maxLastShotTimer)
        {
            damageMultiplier *= timeSinceLastShot;
        }
        else {
            // Do nothing
        }
        player.Cmd_BaseAttack(transform.position, transform.right, damageMultiplier, speedMultiplier, piercing);
        timeSinceLastShot = 0f;
    }
}
