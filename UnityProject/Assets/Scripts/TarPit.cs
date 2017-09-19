using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarPit : DefenseTrap {

    public int speedReduction;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {		
	}

    //requires a_col to have a rigidbody (check Is Kinematic)
    //also requires this to have collider with Is Trigger checked
    void OnTriggerEnter(Collider a_col)
    {
        a_col.GetComponent<PlayerController>().speed -= speedReduction;
        Debug.Log("Trigger Enter");
    }

    void OnTriggerExit(Collider a_col)
    {
        a_col.GetComponent<PlayerController>().speed += speedReduction;
        Debug.Log("Trigger Exit");
    }
}
