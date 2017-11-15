﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ColourChange : MonoBehaviour {

    public bool isChanging;
    private float colourTime;

    private Material originalMaterial;
    private Material damagedMaterial;
	// Use this for initialization
	void Start () {
        originalMaterial = GetComponent<Renderer>().material;
        damagedMaterial = GetComponentInParent<PlayerHealth>().emissiveMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        if (colourTime <= 0)
        {
            colourTime = GetComponentInParent<PlayerHealth>().timeSpentAsDifferentColour;
        }

        //change colour
        if (isChanging == true)
        {
            GetComponent<Renderer>().material = damagedMaterial;
            colourTime -= Time.deltaTime;

            //set isChanging to false (change colour back to original)
            if (colourTime <= 0)
            {
                isChanging = false;
                
                colourTime = GetComponentInParent<PlayerHealth>().timeSpentAsDifferentColour;
            }
        }
        //set colour to original
        if (isChanging == false)
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
    }
}
