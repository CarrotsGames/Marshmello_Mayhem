using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarPit : DefenseTrap {

    public int speedReduction;

	// Use this for initialization
	void Start ()
    {
        Collider collision = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
    {		
	}

    void OnCollisionEnter(Collision a_col)
    {
        a_col.collider.GetComponent<PlayerController>().speed -= speedReduction;
        Debug.Log("Enter: " + a_col.collider.name);
    }

    void OnCollisionExit(Collision a_col)
    {
        a_col.collider.GetComponent<PlayerController>().speed += speedReduction;
        Debug.Log("Exit: " + a_col.collider.name);
    }
}
