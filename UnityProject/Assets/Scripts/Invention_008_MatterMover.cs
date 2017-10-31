using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_008_MatterMover : TrapBehaviour
{

    //requires two points (can only have two exist)
    //if used, destroy both
    
    GameObject pairedObject;
    bool isPaired = false;
    bool wasUsed;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (wasUsed == true)
        {
            Break();
            pairedObject.GetComponent<Invention_008_MatterMover>().wasUsed = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (isPaired == true && wasUsed == false)
        {
            other.transform.position = pairedObject.transform.position;
            wasUsed = true;
            
        }
    }

    public void Break()
    {
        FindObjectOfType<GameController>().placedTraps.Remove(gameObject);
        Destroy(gameObject);
    }

    public void SetPartner(GameObject a_object)
    {
        pairedObject = a_object;
        isPaired = true;
    }
}
