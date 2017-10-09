using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : DefenseTrap {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider a_col)
    {
        a_col.GetComponent<PlayerController>().playerMovement = false;
        a_col.transform.Translate(0.0f, -0.2f, 0.0f);
        
        if (a_col.transform.position.y <= 0)
        {
            a_col.GetComponent<PlayerHealth>().currentHealth = 0;
        }
    }
}
