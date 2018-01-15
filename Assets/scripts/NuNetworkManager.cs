using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NuNetworkManager : NetworkManager {

	public Button archerButton;
	public Button warriorButton;
    public Button nekoromancerButton;

    int avatarIndex;

	// Use this for initialization
	void Start () {
		/*
		archerButton.onClick.AddListener(delegate { playerClassPicker(archerButton.name); });
		warriorButton.onClick.AddListener(delegate { playerClassPicker(warriorButton.name); });
        nekoromancerButton.onClick.AddListener(delegate { playerClassPicker(nekoromancerButton.name); });
		*/
    }

	// Update is called once per frame
	void Update () {
		
	}

	void playerClassPicker (string buttonName) {
		switch (buttonName) {
			case ("archer"):
				avatarIndex = 0;
				break;
			case ("warrior"):
				avatarIndex = 1;
				break;
            case ("nekoromancer"):
                avatarIndex = 2;
                break;
        }

		playerPrefab = spawnPrefabs[avatarIndex];
	}

	public override void OnClientConnect (NetworkConnection conn) {

		IntegerMessage msg = new IntegerMessage(avatarIndex);
		if (!clientLoadedScene) {
			ClientScene.Ready(conn);
			if (autoCreatePlayer) {
				ClientScene.AddPlayer(conn, 0, msg);
			}
		}
	}

	public override void OnClientSceneChanged (NetworkConnection conn) {
		// always become ready.
		ClientScene.Ready(conn);

		if (!autoCreatePlayer) {
			return;
		}

		IntegerMessage msg = new IntegerMessage(avatarIndex);


		bool addPlayer = (ClientScene.localPlayers.Count == 0);
		bool foundPlayer = false;
		foreach (var playerController in ClientScene.localPlayers) {
			if (playerController.gameObject != null) {
				foundPlayer = true;
				break;
			}
		}
		if (!foundPlayer) {
			// there are players, but their game objects have all been deleted
			addPlayer = true;
		}
		if (addPlayer) {
			ClientScene.AddPlayer(conn, 0, msg);
		}

	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) {
		Debug.Log("MEwo!");
		base.OnServerAddPlayer(conn, playerControllerId);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		int id = 0;
		Debug.Log("Player add message received: " + extraMessageReader);
		if (extraMessageReader != null) {
			Debug.Log(extraMessageReader);

			IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage>();
			id = i.value;
			Debug.Log("value: "+ id);

		}

		GameObject playerPrefab = spawnPrefabs[id];

		Debug.Log("playerPrefab: " + playerPrefab);


		GameObject player;
		Transform startPos = GetStartPosition();
		if (startPos != null) {
			player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
		} else {
			player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
}
