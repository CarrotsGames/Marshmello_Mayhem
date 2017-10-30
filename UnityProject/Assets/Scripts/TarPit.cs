using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarPit : TrapBehaviour
{

    public int slowedSpeed;

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
    private void OnTriggerEnter(Collider a_col)
    {
        a_col.GetComponent<PlayerController>().speed = slowedSpeed;
        Debug.Log("Trigger Enter");
    }

    private void OnTriggerExit(Collider a_col)
    {
        a_col.GetComponent<PlayerController>().speed = a_col.GetComponent<PlayerController>().maxSpeed;
        Debug.Log("Trigger Exit");
    }
}
