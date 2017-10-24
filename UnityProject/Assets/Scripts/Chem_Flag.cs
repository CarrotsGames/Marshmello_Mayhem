using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_Flag : MonoBehaviour {

    public GameObject team1Base;
    public GameObject team2Base;

    public float respawnTimer;

    TeamScore_UI teamScore;

    bool isRespawning;
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
        if (FindObjectOfType<TeamScore_UI>() != null)
        {
            teamScore = FindObjectOfType<TeamScore_UI>();
        }
        else
        {
            Debug.Log("TeamScore script not attached to anything");
        }
    }
    void Update()
    {
        //if team 1 is capturing the chem_flag
        if (teamNumber == 2 && team1Base != null)
        {
            Vector3 vecBetween = transform.position - team1Base.transform.position;

            if (vecBetween.magnitude <= 2)
            {
                if (isRespawning == false)
                {
                    //increment team 1 score
                    teamScore.team1Score += 1;

                    gameObject.SetActive(false);
                    Invoke("Respawn", respawnTimer);

                    isRespawning = true;
                }
            }
        }
        //if team 2 is capturing the chem_flag
        if (teamNumber == 1 && team2Base != null)
        {
            Vector3 vecBetween = transform.position - team2Base.transform.position;

            if (vecBetween.magnitude <= 2)
            {
                if (isRespawning == false)
                {
                    //increment team 2 score
                    teamScore.team2Score += 1;

                    gameObject.SetActive(false);
                    Invoke("Respawn", respawnTimer);

                    isRespawning = true;
                }
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

    public void Respawn()
    {
        DropChemFlag();

        gameObject.SetActive(true);
        
        if (teamNumber == 1)
        {
            //reset position
            transform.SetPositionAndRotation(new Vector3(team1Base.transform.position.x, transform.position.y, team1Base.transform.position.z), Quaternion.identity);
        }
        if (teamNumber == 2)
        {
            //reset position
            transform.SetPositionAndRotation(new Vector3(team2Base.transform.position.x, transform.position.y, team2Base.transform.position.z), Quaternion.identity);
        }
        isRespawning = false;
    }
}
