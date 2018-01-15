using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ArcheryScript : ClassScript {

	public GameObject SkellingtonPrefab;

	public GameObject summoningEffectPrefab;
	GameObject SummoningEffect;

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
	protected override void ClassStart () {
        loadBar = gameObject.GetComponent<DrawBarScript>();
    }

	// Update is called once per frame
	void Update () {
		if (player.isLocalPlayer) {
			LocalPlayerUpdate();
		}
	}

	protected override void LocalPlayerUpdate () {
		timeSinceLastShot += Time.deltaTime;
		if (Input.GetMouseButtonDown(0)) {
			StartCoroutine(ShotTimer());
		}


		if (Input.GetKeyDown(KeyCode.Alpha1)) {

			mode = SpecialMode.Piercing;

		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {

			mode = SpecialMode.Ricochet;

		} else if (Input.GetKeyDown(KeyCode.Q)) {

			Rpc_ToggleStealth(!player.stealthed);

		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			StartCoroutine(SummonTimer());
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
	[Client]
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
		Rpc_ToggleStealth(false);
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



	IEnumerator SummonTimer () {
		SummoningEffect = Instantiate(summoningEffectPrefab, new Vector3(transform.position.x, transform.position.y, -9f), Quaternion.identity) as GameObject;
		SummoningEffect.GetComponent<StayOnTopOf>().targetName = transform.name;

		NetworkServer.Spawn(SummoningEffect);

		float holdTimer = 0;
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		while (holdTimer < 2.5f) {
			if (!Input.GetKey(KeyCode.Alpha3)) {
				NetworkServer.Destroy(SummoningEffect);
				yield break;
			}
			holdTimer += Time.deltaTime;
			rb.velocity = new Vector2(0, 0);
			yield return null;
		}

		Cmd_MakeSkellington(transform.position + transform.right * 2, transform.rotation);


		NetworkServer.Destroy(SummoningEffect);
		yield return null;
	}

	[Command]
	void Cmd_MakeSkellington (Vector3 position, Quaternion rotation) {
		GameObject skellington = Instantiate(SkellingtonPrefab, position, rotation) as GameObject;
		Rpc_Poof(skellington.transform.position);
		NetworkServer.Spawn(skellington);
	}

	[ClientRpc]
	void Rpc_Poof (Vector3 position) {
		Instantiate(SmokePoofPrefab, position, Quaternion.identity);
	}
}
