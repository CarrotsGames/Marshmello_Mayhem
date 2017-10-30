using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_005_PillarOfSaws : TrapBehaviour
{
    public int maxHealth = 1;
    int currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0)
        {
            FindObjectOfType<GameController>().placedTraps.Remove(gameObject);
            Destroy(gameObject);            
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerHealth>().Death();
        }
    }
    
    public void TakeDamage(int a_damage)
    {
        currentHealth -= a_damage;
    }
}
