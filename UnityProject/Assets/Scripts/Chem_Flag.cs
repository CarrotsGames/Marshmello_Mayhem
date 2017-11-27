using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_Flag : MonoBehaviour {

    public GameObject blueTeamBase;
    public GameObject redTeamBase;

    public float dropRespawnTimer;
    public float respawnTimer;
    private float respawnAfterDrop;
    bool isRespawning;

    TeamScore_UI teamScore;

    public int teamNumber = 1;

    public bool isBeingCarried = false;
    public bool wasDropped = false;
    public bool canBePickedUp = true;
    public float timeBetweenDropAndPickUp;
    float timer = 0.0f;

    public float captureDistanceLeniency = 2;

    //(blue = 1, red = 2);

    void Start()
    {
        if (FindObjectOfType<TeamScore_UI>() != null)
        {
            teamScore = FindObjectOfType<TeamScore_UI>();
        }
    }
    void Update()
    {
        //if team 1 is capturing the chem_flag
        if (teamNumber == 2 && blueTeamBase != null)
        {
            Vector3 vecBetween = transform.position - blueTeamBase.transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= captureDistanceLeniency)
            {
                if (isRespawning == false)
                {
                    //increment team 1 score
                    teamScore.team1Score += 1;

                    //disable player's particles
                    GetComponentInParent<PlayerController>().DropChemFlag();

                    gameObject.SetActive(false);
                    Invoke("Respawn", respawnTimer);

                    isRespawning = true;
                }
            }
        }
        //if team 2 is capturing the chem_flag
        if (teamNumber == 1 && redTeamBase != null)
        {
            Vector3 vecBetween = transform.position - redTeamBase.transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= captureDistanceLeniency)
            {
                if (isRespawning == false)
                {
                    //increment team 2 score
                    teamScore.team2Score += 1;

                    //disable player's particles
                    GetComponentInParent<PlayerController>().DropChemFlag();

                    gameObject.SetActive(false);
                    Invoke("Respawn", respawnTimer);

                    isRespawning = true;
                }
            }
        }

        //respawn after time if dropped
        if (wasDropped == true)
        {
            respawnAfterDrop += Time.deltaTime;

            if (respawnAfterDrop >= dropRespawnTimer)
            {
                Respawn();
                respawnAfterDrop = 0;
                wasDropped = false;
            }
        }
        
        //disables pick up for time
        if (canBePickedUp == false)
        {            
            timer += Time.deltaTime;

            if (timer >= timeBetweenDropAndPickUp)
            {
                canBePickedUp = true;

                timer = 0;
            }
        }
    }

    public void PickUpChemFlag()
    {        
         if (canBePickedUp == true)
         {
             GetComponent<BoxCollider>().enabled = false;
             isBeingCarried = true;
             canBePickedUp = false;
         }      

        wasDropped = false;
        respawnAfterDrop = 0;
    }

	public void DropChemFlag()
    {
		isBeingCarried = false;
		GetComponent<BoxCollider>().enabled = true;
		transform.SetParent(null);
        wasDropped = true;
        canBePickedUp = false;
    }

    //reset bools and set position to respective team's base
    public void Respawn()
    {
        DropChemFlag();

        gameObject.SetActive(true);
        
        if (teamNumber == 1)
        {
            //reset position
            transform.SetPositionAndRotation(new Vector3(blueTeamBase.transform.position.x, transform.position.y, blueTeamBase.transform.position.z), Quaternion.identity);
        }
        if (teamNumber == 2)
        {
            //reset position
            transform.SetPositionAndRotation(new Vector3(redTeamBase.transform.position.x, transform.position.y, redTeamBase.transform.position.z), Quaternion.identity);
        }
        isRespawning = false;
        wasDropped = false;
    }
}
