using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for each pick up item
//modifiable values:
//  pickUpIncrease (the amount of resources gained)

public class ResourcePickUp : MonoBehaviour {

    public int resourceGain;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider a_col)
    {
        a_col.GetComponent<ResourceController>().currentResource += resourceGain;
        Destroy(gameObject);
    }
}
