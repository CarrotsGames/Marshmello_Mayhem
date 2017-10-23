using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScore : MonoBehaviour {

    public int team1Score;
    public int team2Score;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void OnTriggerEnter(Collider a_col)
    {
        int team = a_col.GetComponent<PlayerController>().teamNumber;

        if (team == 1)
        {
            team1Score++;
        }
        if (team == 2)
        {
            team2Score++;
        }
    }
}
