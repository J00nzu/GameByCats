using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	[SyncVar]
	public float health = 100;
	[SyncVar]
	public float maxHealth = 100;

	[SyncVar]
	public int level;
	[SyncVar]
	public int exp;
	[SyncVar]
	public int expneeded;

	public string playerName;

	[SyncVar]
    public bool stealthed;

    public GameObject playerLightPrefab;
    GameObject playerLight;

	// Use this for initialization
	void Start () {
        playerName = "player_" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
        var nd = FindObjectOfType<NetDog>();
        nd.AddPlayer(this);
        playerLight = Instantiate(playerLightPrefab, transform.position, Quaternion.identity) as GameObject;
        playerLight.GetComponent<StayOnTop>().target = transform;
    }



    public override void OnStartLocalPlayer () {
        //GetComponent<SpriteRenderer>().color = Color.green;
		var nd = FindObjectOfType<NetDog>();
		nd.SetLocalPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[ClientRpc]
	public void Rpc_TakeDamage (float damage) {
		if (GetComponent<NetworkIdentity>().isLocalPlayer) {
			health -= damage;
		}
		if(!ouchGoing)StartCoroutine(ouch());
	}

	public bool ouchGoing = false;

	IEnumerator ouch () {
		ouchGoing = true;
		Light light = playerLight.GetComponent<Light>();
		if (light == null) {
			Debug.Log("Player missing a light!");
			yield break;

		}
		Color original = light.color;

		Color targetColor = Color.red;
		float mt = 0.1f;
		for(int i=0; i<2; i++) { 
			for (float f = 0; f < mt; f += Time.deltaTime) {
				float l = f / mt;
				light.color = Color.Lerp(original, targetColor, l);
				yield return null;
			}
			for (float f = mt; f > 0; f -= Time.deltaTime) {
				float l = f / mt;
				light.color = Color.Lerp(original, targetColor, l);
				yield return null;
			}
		}

		light.color = original;
		ouchGoing = false;
	}

    
}
