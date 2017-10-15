using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : DefenseTrap {

    GameObject[] floorGrid;

	// Use this for initialization
	void Start () {
        floorGrid = GameObject.FindGameObjectsWithTag("Floor");

        for (int i = 0; i < floorGrid.Length; i++)
        {
            Vector3 vecBetween = transform.position - floorGrid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude < 1.2)
            {
                floorGrid[i].GetComponent<Tile>().isPit = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter(Collider a_col)
    {
        //currently cannot access Death() due to it being defaulted to private
        //a_col.GetComponent<PlayerHealth>().Death();
    }
}
