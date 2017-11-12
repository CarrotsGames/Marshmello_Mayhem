﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanThrower : TrapBehaviour {

    //test variables
    GameObject player;

    //slerping
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float timer = 0.0f;
    public float timeBeforeActivation = 5.0f;

    public float launchHeight = 0.4f;
    float launchSpeed = 5.0f;

    public float timeInAir = 3.0f;

    // Use this for initialization
    void Start () {

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= timeBeforeActivation)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider a_col)
    {
        player = a_col.gameObject;

        //call lerp function in player
        if (player.GetComponent<PlayerController>() != null)
        {
            player.GetComponent<PlayerController>().StartLerp(launchSpeed, launchHeight, timeInAir);
        }
    }
}
