using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter_UI : MonoBehaviour {

    public GameObject player;
    public GameObject resourcesColour;
    private bool changingColour;
    private Color originalColour;

	// Use this for initialization
	void Start ()
    {
        originalColour = resourcesColour.GetComponent<Image>().color;
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

            if (player.GetComponent<BuildTrap>().NotEnoughResources())
            {
                changingColour = true;
            }
            else
            {
                changingColour = false;
            }
        }

        if (changingColour == true)
        {
            resourcesColour.GetComponent<Image>().color = Color.red;
        }
        else
        {
            resourcesColour.GetComponent<Image>().color = originalColour;
        }
	}
}
