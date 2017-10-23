using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWall : DefenseTrap {

    public int maxHealth = 1;
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

    public void TakeDamage(int a_damageValue)
    {
        health -= a_damageValue;
    }
}
