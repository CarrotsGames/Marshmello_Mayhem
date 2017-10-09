using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWall : DefenseTrap {

    public int maxHealth;
    private int health;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
        {
            Destroy(gameObject);
        }
	}
}
