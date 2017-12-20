using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ArcheryScript : ClassScript {

	PlayerScript player;

    SpriteRenderer sprite;

	public GameObject ArrowPrefab;
	public GameObject PiercingArrowPrefab;
	public GameObject RicochetingArrowPrefab;

    public GameObject SmokePoofPrefab;

	public DrawBarScript loadBar;

	public float timeSinceLastShot = 0;
	float maxLastShotTimer = 1f;

	enum SpecialMode {
		Piercing, Ricochet
	}

	SpecialMode mode;

	// Use this for initialization
	void Start () {
		player = GetComponentInChildren<PlayerScript>();
		loadBar = gameObject.GetComponent<DrawBarScript>();
        sprite = player.gameObject.GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {
		if (player.isLocalPlayer) {
			LocalPlayerUpdate();
		}
	}

	void LocalPlayerUpdate () {
		timeSinceLastShot += Time.deltaTime;
		if (Input.GetMouseButtonDown(0)) {
			StartCoroutine(ShotTimer());
		}


		if (Input.GetKey(KeyCode.Alpha1)) {
			mode = SpecialMode.Piercing;
		} else if (Input.GetKey(KeyCode.Alpha2)) {
			mode = SpecialMode.Ricochet;
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rpc_ToggleStealth(!player.stealthed);
        }
	}

   [ClientRpc]
   void Rpc_ToggleStealth(bool val)
    {
        player.stealthed = val;
        if (player.stealthed)
        {
            var SmokePoof = (GameObject)Instantiate(
            SmokePoofPrefab,
            transform.position,
            Quaternion.identity);
            Destroy(SmokePoof, 2f);
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, val?0.2f:1f);

    }

    IEnumerator ShotTimer () {
		float damageMultiplier = 1f;
		float speedMultiplier = 1f;
		float holdTimer = 0;
		bool special = false;

		while (Input.GetMouseButton(0)) {
			holdTimer += Time.deltaTime;
			loadBar.SetDrawBar(Mathf.Clamp01((holdTimer - 0.2f) / 3.0f), transform.position + transform.right, transform.right);
			yield return null;
		}

		loadBar.SetDrawBar(0, Vector3.zero, Vector3.zero);


		if (holdTimer > 1f) {
			special = true;
			damageMultiplier *= Mathf.Clamp(holdTimer, 1f, 3f) * 2f;
			speedMultiplier *= Mathf.Clamp(holdTimer, 1f, 2f) * 1.5f;
		} else if (timeSinceLastShot < maxLastShotTimer) {
			damageMultiplier = 1f;
		} else {
			damageMultiplier = 2f;
		}

		if (special) {
			switch (mode) {
				case SpecialMode.Piercing:
					Cmd_PiercingArrow(transform.position, transform.right, damageMultiplier, speedMultiplier);
					break;
				case SpecialMode.Ricochet:
					Cmd_RicochetingArrow(transform.position, transform.right, damageMultiplier, speedMultiplier);
					break;
			}
		} else {
			Cmd_BaseAttack(transform.position, transform.right, damageMultiplier, speedMultiplier);
		}
		timeSinceLastShot = 0f;
	}

	[Command]
	public void Cmd_BaseAttack (Vector2 position, Vector2 direction, float damageMultiplier, float speedMultiplier) {

		// Create the Arrow from the Arrow Prefab
		var Arrow = (GameObject)Instantiate(
			ArrowPrefab,
			position + direction * 1,
			Quaternion.identity);

		Arrow.transform.right = direction;

		// Add velocity to the Arrow
		Arrow.GetComponent<Rigidbody2D>().velocity = Arrow.transform.right * 16 * speedMultiplier;

		var bscript = Arrow.GetComponent<ArrowScript>();
		bscript.player = player.playerName;
		bscript.damage *= damageMultiplier;

		NetworkServer.Spawn(Arrow);
	}

	[Command]
	public void Cmd_PiercingArrow (Vector2 position, Vector2 direction, float damageMultiplier, float speedMultiplier) {

		// Create the Arrow from the Arrow Prefab
		var Arrow = (GameObject)Instantiate(
			PiercingArrowPrefab,
			position + direction * 1,
			Quaternion.identity);

		Arrow.transform.right = direction;

		// Add velocity to the Arrow
		Arrow.GetComponent<Rigidbody2D>().velocity = Arrow.transform.right * 16 * speedMultiplier;

		var bscript = Arrow.GetComponent<ArrowScript>();
		bscript.player = player.playerName;
		bscript.damage *= damageMultiplier;

		NetworkServer.Spawn(Arrow);
	}

	[Command]
	public void Cmd_RicochetingArrow (Vector2 position, Vector2 direction, float damageMultiplier, float speedMultiplier) {

		// Create the Arrow from the Arrow Prefab
		var Arrow = (GameObject)Instantiate(
			RicochetingArrowPrefab,
			position + direction * 1,
			Quaternion.identity);

		Arrow.transform.right = direction;

		// Add velocity to the Arrow
		Arrow.GetComponent<Rigidbody2D>().velocity = Arrow.transform.right * 16 * speedMultiplier;

		var bscript = Arrow.GetComponent<ArrowScript>();
		bscript.player = player.playerName;
		bscript.damage *= damageMultiplier;

		NetworkServer.Spawn(Arrow);
	}
}
