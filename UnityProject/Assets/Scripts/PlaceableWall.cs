using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWall : TrapBehaviour
{

    public int maxHealth = 1;
    private int health;

	// Use this for initialization
	void Start () {
        health = maxHealth;
        trapName = "PlaceableWall";
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
        {
            FindObjectOfType<GameController>().placedTraps.Remove(gameObject);
            Destroy(gameObject);
        }
	}

    public void TakeDamage(int a_damageValue)
    {
        health -= a_damageValue;
    }
}
