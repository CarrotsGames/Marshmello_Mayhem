using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_UI : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerHealth>() != null)
        {
            GetComponent<Slider>().value = player.GetComponent<PlayerHealth>().currentHealth;
        }
	}
}
