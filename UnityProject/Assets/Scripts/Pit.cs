using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : DefenseTrap {

    GameObject[] floorGrid;

	// Use this for initialization
	void Start () {

        floorGrid = GameObject.FindGameObjectsWithTag("Floor");

        //loop over all tiles
        for (int i = 0; i < floorGrid.Length; i++)
        {
            Vector3 vecBetween = transform.position - floorGrid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude < 1.2)
            {
                if (floorGrid[i].GetComponent<Tile>() != null)
                {
                    floorGrid[i].GetComponent<Tile>().isPit = true;
                }
                else
                {
                    Debug.Log("Missing Tile script on floor prefab");
                }
            }
        }

        if (GetComponent<BoxCollider>() == null)
        {
            Debug.Log("Missing BoxCollider on pit object");

            if (GetComponent<BoxCollider>().isTrigger == false)
            {
                Debug.Log("Pit BoxCollider is not a trigger");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter(Collider a_col)
    {
        if (a_col.GetComponent<PlayerController>() != null)
        {
            if (a_col.GetComponent<PlayerHealth>() != null)
            {
                Debug.Log("Pit triggered");
                PlayerController player = a_col.GetComponent<PlayerController>();
                                
                player.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                player.transform.Translate(0.0f, -1.0f, 0.0f);

                a_col.GetComponent<PlayerHealth>().Death();
            }
            else
            {
                Debug.Log("Missing PlayerHealth script on player");
            }
        }
        else
        {
            Debug.Log("Missing PlayerController script on player");
        }
        
    }
}
