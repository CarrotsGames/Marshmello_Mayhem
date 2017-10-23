using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_Flag : MonoBehaviour {

    public GameObject team1Base;
    public GameObject team2Base;

    TeamScore teamScore;

	public bool isBeingCarried = false;
	public int teamNumber = 1;

    //(blue = 1, red = 2);

    //	void OnTriggerStay(Collider Other){
    //
    //		if (isBeingCarried) {
    //			return;
    //		}
    //
    //		if (Other.tag != "Player") {
    //			return;
    //		}
    //			
    //		PlayerController playerController = Other.GetComponent<PlayerController> ();
    //
    //		if (playerController.teamNumber != teamNumber) {
    //			playerController.CanPickUpChemFlag ();
    //		}
    //	}

    void Start()
    {
        if (FindObjectOfType<TeamScore>() != null)
        {
            teamScore = FindObjectOfType<TeamScore>();
        }
        else
        {
            Debug.Log("No object with TeamScore");
        }
    }
    void Update()
    {
        if (team1Base != null || team2Base != null)
        {
            //if team 2 is capturing the chem_flag
            if (teamNumber == 1)
            {
                Vector3 vecBetween = transform.position - team2Base.transform.position;

                if (vecBetween.magnitude <= 2)
                {
                    teamScore.team2Score++;
                }
            }
            //if team 1 is capturing the chem_flag
            if (teamNumber == 2)
            {
                Vector3 vecBetween = transform.position - team1Base.transform.position;
            }
        }
    }

    public void PickUpChemFlag()
    {
		//playerController.PickUpChemFlag (this);
		GetComponent<BoxCollider>().enabled = false;
		isBeingCarried = true;
	}

	public void DropChemFlag()
    {
		isBeingCarried = false;
		GetComponent<BoxCollider>().enabled = true;
		transform.SetParent(null);
	}
}
