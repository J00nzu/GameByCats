using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ClassScript : NetworkBehaviour
{

    protected PlayerScript player;

    protected SpriteRenderer sprite;


    // Use this for initialization
    void Start()
    {
        player = GetComponentInChildren<PlayerScript>();
        sprite = player.gameObject.GetComponent<SpriteRenderer>();
        ClassStart();
    }

    protected virtual void ClassStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isLocalPlayer)
        {
            LocalPlayerUpdate();
        }
    }

    protected abstract void LocalPlayerUpdate();
}

