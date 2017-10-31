﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_007_LastPrayer : TrapBehaviour {

    float countdown = 5.0f;
    float explosionRadius = 10.0f;
    List<GameObject> traps;
    int explosionDamage = 100;
    GameObject[] players;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        traps = FindObjectOfType<GameController>().placedTraps;

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            Explosion();

            traps.Remove(gameObject);
            Destroy(gameObject);
        }
	}

    void Explosion()
    {    
        for (int i = 0; i < traps.Count; i++)
        {
            Vector3 vecBetween = transform.position - traps[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= explosionRadius)
            {
                if (traps[i].GetComponent<PlaceableWall>() != null)
                {                    
                    traps[i].GetComponent<PlaceableWall>().TakeDamage(explosionDamage);
                }                

                if (traps[i].GetComponent<Invention_005_PillarOfSaws>() != null)
                {
                    traps[i].GetComponent<Invention_005_PillarOfSaws>().TakeDamage(explosionDamage);
                }
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 vecBetween = transform.position - players[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= explosionRadius)
            {
                if (players[i].GetComponent<PlayerHealth>() != null)
                {
                    players[i].GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
                }
            }
        }
    }
}
