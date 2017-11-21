using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour {

    public int cost;
    public int health = 1;
    public string trapName;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	public virtual void Update () {
        CheckForDestruction();
    }

    public void TakeDamage(int a_damageValue)
    {
        health -= a_damageValue;
    }

    public void CheckForDestruction()
    {
        if (health <= 0)
        {
            FindObjectOfType<GameController>().placedTraps.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
