using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for each player
//modifiable values:
//  maxResource, currentResource, resourceIncrease, timeBetween

public class ResourceController : MonoBehaviour
{

    public float maxResource;
    public float currentResource;
    public float resourceIncrease;
    public float timeBetween;

    // Use this for initialization
    void Start()
    {
        currentResource = 0;
        TimeBetween();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentResource > maxResource)
        {
            currentResource = maxResource;
        }
    }

    void TimeBetween()
    {
        Invoke("IncrementResources", timeBetween);
    }

    void IncrementResources()
    {
        TimeBetween();
        currentResource += resourceIncrease;
    }
}
