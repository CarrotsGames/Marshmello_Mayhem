using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public float timeBeforeDespawn = 30.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeBeforeDespawn -= Time.deltaTime;

        if (timeBeforeDespawn <= 0)
        {
            Destroy(gameObject);
        }
	}
}
