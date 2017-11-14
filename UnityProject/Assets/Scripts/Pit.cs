using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : TrapBehaviour
{

    GameObject[] floorGrid;
    public int timeBeforeActivation = 5;
    public float timeBeforeDeath = 2;
    private float timeRemaining;
    bool isTriggered;
    GameObject player;
    AudioSource pitSound;

    // Use this for initialization
    void Start () {
        trapName = "Pit";
        pitSound = FindObjectOfType<PrefabList>().PitAudio;

        timeRemaining = timeBeforeDeath;

        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            Debug.Log("Missing BoxCollider on Pit");
        }

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

        Invoke("Activate", timeBeforeActivation);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isTriggered == true)
        {
            //kill player after delay
            timeRemaining -= Time.deltaTime;

            player.GetComponent<PlayerHealth>().isAlive = false;

            if (timeRemaining <= 0)
            {
                timeRemaining = timeBeforeDeath;
                player.GetComponent<PlayerHealth>().Death();
                Debug.Log("Death");
                isTriggered = false;
            }
        }
    }

    private void OnTriggerEnter(Collider a_col)
    {
        if (a_col.GetComponent<PlayerController>() != null)
        {
            if (a_col.GetComponent<PlayerHealth>() != null)
            {
                pitSound.Play();

                Debug.Log("Pit triggered");
                player = a_col.gameObject;

                if (a_col.GetComponent<PlayerController>().holdingChemFlag != null)
                {
                    if (a_col.GetComponent<PlayerController>().holdingChemFlag.isBeingCarried == true)
                    {
                        a_col.GetComponent<PlayerController>().DropChemFlag();
                    }
                }
                //sets player's position to middle of pit
                player.GetComponent<PlayerController>().transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), Quaternion.identity);

                
                isTriggered = true;

            }
            else if (a_col.GetComponent<PlayerHealth>() == null && a_col.GetComponent<Projectile>() == null)
            {
                Debug.Log("Missing PlayerHealth script on player");
            }
            
        }

        
    }

    void Activate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
