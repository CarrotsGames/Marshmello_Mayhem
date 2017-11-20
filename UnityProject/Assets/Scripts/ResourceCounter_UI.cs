using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter_UI : MonoBehaviour {

    public GameObject player;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player != null)
        {
            if (player.GetComponent<ResourceController>() != null)
            {
                GetComponent<Slider>().value = player.GetComponent<ResourceController>().currentResource;
            }
        }
	}
}
