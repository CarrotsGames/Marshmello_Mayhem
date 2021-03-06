﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : TrapBehaviour
{

    GameObject[] floorGrid;
    public int timeBeforeActivation = 5;
    public float timeBeforeDeath = 2;
    private float timeRemaining;
    bool isTriggered;
    GameObject player;
    AudioSource pitSound;
    GameObject tile;
    bool hasMoved;

    // Use this for initialization
    void Start () {
        trapName = "Pit";
        pitSound = FindObjectOfType<PrefabList>().PitAudio;

        timeRemaining = timeBeforeDeath;

        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        floorGrid = GameObject.FindGameObjectsWithTag("Floor");

        //loop over all tiles
        for (int i = 0; i < floorGrid.Length; i++)
        {
            Vector3 vecBetween = transform.position - floorGrid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude < 1.2)
            {
                if (floorGrid[i].GetComponent<Tile>() != null)
                {
                    tile = floorGrid[i];
                    floorGrid[i].GetComponent<Tile>().isPit = true;
                }
            }
        }

        Invoke("Activate", timeBeforeActivation);
    }
	
	// Update is called once per frame
	void Update()
    {
        if (health <= 0)
        {
            tile.SetActive(true);
            tile.GetComponent<Tile>().isPit = false;
            CheckForDestruction();
        }

        if (isTriggered == true)
        {
            if (hasMoved == false)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(player.GetComponent<PlayerHealth>().currentHealth);
                hasMoved = true;
            }

            //kill player after delay
            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                timeRemaining = timeBeforeDeath;
                isTriggered = false;
                hasMoved = false;
            }
        }
    }

    private void OnTriggerEnter(Collider a_col)
    {
        if (a_col.GetComponent<PlayerController>() != null)
        {
            if (a_col.GetComponent<PlayerHealth>() != null)
            {
                pitSound.Play();

                player = a_col.gameObject;

                if (a_col.GetComponent<PlayerController>().holdingChemFlag != null)
                {
                    if (a_col.GetComponent<PlayerController>().holdingChemFlag.isBeingCarried == true)
                    {
                        a_col.GetComponent<PlayerController>().DropChemFlag();
                    }
                }               

                isTriggered = true;

                hasMoved = false;
            }            
        }        
    }

    void Activate()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
