using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_Flag : MonoBehaviour {

    public GameObject team1Base;
    public GameObject team2Base;

    public float respawnTimer;
    private float respawnAfterDrop;

    TeamScore_UI teamScore;

    //automatic pick up (detect if player is close to chem_flag)
    //play sounds

    bool isRespawning;
	public bool isBeingCarried = false;
	public int teamNumber = 1;
    public bool wasDropped = false;

    public bool canBePickedUp = true;
    public float timeBetweenDropAndPickUp;
    float timer = 0.0f;


    //(blue = 1, red = 2);

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

        if (team1Base == null)
        {
            Debug.Log("Chem Canister is missing team 1 base");
        }
        if (team2Base == null)
        {
            Debug.Log("Chem Canister is missing team 2 base");
        }     
        
        
    }
    void Update()
    {
        //if team 1 is capturing the chem_flag
        if (teamNumber == 2 && team1Base != null)
        {
            Vector3 vecBetween = transform.position - team1Base.transform.position;
            vecBetween.y = 0;

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
            vecBetween.y = 0;

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

        
        if (wasDropped == true)
        {
            respawnAfterDrop += Time.deltaTime;

            if (respawnAfterDrop >= respawnTimer)
            {
                Respawn();
                respawnAfterDrop = 0;
            }
        }
        

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
             //playerController.PickUpChemFlag (this);
             GetComponent<BoxCollider>().enabled = false;
             isBeingCarried = true;
            canBePickedUp = false;
         }
            

        
        wasDropped = false;
        respawnAfterDrop = 0;
    }

	public void DropChemFlag()
    {
        if (GetComponentInParent<PlayerController>() != null)
        {
            GetComponentInParent<PlayerController>().DropChemFlag();
        }
		isBeingCarried = false;

		GetComponent<BoxCollider>().enabled = true;
		transform.SetParent(null);

        canBePickedUp = false;
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
        wasDropped = false;
    }
}
