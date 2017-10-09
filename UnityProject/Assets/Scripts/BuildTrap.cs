using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildTrap : MonoBehaviour {

    private Vector3 targetPosition;
    private bool isActive = false;
    public int delay;
    GameObject selectedTrap;
    List<Vector3> potentialTiles;
    Quaternion rotation;
    List<GameObject> createdObjects;
    public bool isEnabled = false;
    GameObject[] floorGrid;
    private PrefabList prefabList;
    private float rise;
    GameObject ghost;
    int cost;
    public float rotationX;
    public float rotationZ;
    
	// Use this for initialization
	void Start ()
    {
        prefabList = GameObject.Find("PrefabList").GetComponent<PrefabList>();
        floorGrid = GameObject.FindGameObjectsWithTag("Floor");
        
        //gets player's original position, return to this at end
        Vector3 originalPlayerPos = GetComponent<PlayerController>().transform.position;

        potentialTiles = new List<Vector3>();
        createdObjects = new List<GameObject>();

        selectedTrap = prefabList.TarPit;
        rise = 0.8f;
        cost = selectedTrap.GetComponent<TarPit>().cost;


        //sets the rotation (call in update for modular rotation using player's rotation)
        Vector3 forward = new Vector3(rotationX, 0, rotationZ);
        Vector3 upwards = new Vector3(0, 1, 0);
        rotation = Quaternion.LookRotation(forward, upwards);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //checks if a trap is currently being built
        if (isActive == false && isEnabled == true)
        {
            if (XCI.GetDPad(XboxDPad.Up) || Input.GetKey(KeyCode.Alpha1))
            {
                selectedTrap = prefabList.TarPit;
                cost = selectedTrap.GetComponent<TarPit>().cost;
                rise = 0.8f;
            }
            if (XCI.GetDPad(XboxDPad.Down) || Input.GetKey(KeyCode.Alpha2))
            {
                selectedTrap = prefabList.Pit;
                cost = selectedTrap.GetComponent<Pit>().cost;
                rise = 0.2f;
                //rotation = selectedTrap.GetComponent<Pit>().rotation;
            }
            if (XCI.GetDPad(XboxDPad.Right) || Input.GetKey(KeyCode.Alpha3))
            {
                selectedTrap = prefabList.PlaceableWall;
                cost = selectedTrap.GetComponent<PlaceableWall>().cost;
                rise = 1.0f;
            }
            if (XCI.GetDPad(XboxDPad.Left) || Input.GetKey(KeyCode.Alpha4))
            {
                selectedTrap = prefabList.HumanThrower;
                cost = selectedTrap.GetComponent<HumanThrower>().cost;
                rise = 1.0f;
            }

            //loop over all tiles
            for (int i = 0; i < floorGrid.Length; i++)
            {
                Vector3 vecBetween = transform.position - floorGrid[i].transform.position;
                vecBetween.y = 0;

                if (vecBetween.magnitude < 2)
                {
                    targetPosition = floorGrid[i].transform.position;
                    targetPosition.y += rise;
                    potentialTiles.Add(targetPosition);
                }
            }

            //loop over all tiles and sort so lowest magnitude is at index zero
            for (int e = 0; e < potentialTiles.Count; e++)
            {
                for (int i = 1; i < potentialTiles.Count; i++)
                {
                    if (potentialTiles[i - 1].magnitude > potentialTiles[i].magnitude)
                    {
                        Vector3 temp = potentialTiles[i - 1];
                        potentialTiles[i - 1] = potentialTiles[i];

                        potentialTiles[i] = temp;
                    }
                }
            }
            
            //display where trap will be placed            
            //for (int i = 0; i < floorGrid.Length; i++)
            //{
            //    Vector3 vecbetween = potentialTiles[0] - floorGrid[i].transform.position;
            //    
            //    if (vecbetween.magnitude < 2)
            //    {
            //        potentialTiles[0].Set(potentialTiles[0].x, potentialTiles[0].y + rise + 5, potentialTiles[0].z);
            //        ghost = Instantiate(selectedTrap, potentialTiles[0], rotation);
            //        
            //    }
            //    else
            //    {
            //        Destroy(ghost);
            //    }
            //}

            //checks if player has pressed Xbox:A or the space bar
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A))
            {                
                //checks if player is within range of any tiles
                if (potentialTiles.Count > 0)
                {   
                    bool trapInRange = false;

                    //checks if at least one trap exists
                    if (createdObjects.Count > 0)
                    {
                        //check if trap exists on selected tile
                        for (int i = 0; i < createdObjects.Count; i++)
                        {
                            //gets distance between the selected area and an existing trap
                            Vector3 vecBetween = potentialTiles[0] - createdObjects[i].transform.position;

                            //if selected area is occupied
                            if (vecBetween.magnitude < 2)
                            {
                                trapInRange = true;                                                                
                            }
                        }

                        //if selected area is not occupied
                        if (trapInRange == false)
                        {
                            if (GetComponent<ResourceController>().currentResource - cost >= 0)
                            {
                                //set isActive to true, disables this until false
                                isActive = true;
                                Debug.Log("Building started");

                                //calls Build function after delay (seconds)
                                Invoke("Build", delay);
                            }
                            else
                            {
                                Debug.Log("Not enough resources");
                            }
                        }
                        else
                        {
                            Debug.Log("An invention already exists there!");

                            potentialTiles.Clear();
                        }
                    }
                    else
                    {
                        if (GetComponent<ResourceController>().currentResource - cost >= 0)
                        {
                            //set isActive to true, disables this until false
                            isActive = true;
                            Debug.Log("Building started");

                            //calls Build function after delay (seconds)
                            Invoke("Build", delay);
                        }       
                        else
                        {
                            Debug.Log("Not enough resources");
                        }
                    }
                }
            }
            else
            {
                potentialTiles.Clear();
            }
        }
	}

    void Build ()
    {
        //re-enables ability to build a trap
        isActive = false;
        Debug.Log("Building finished");

        //create trap at position closest to selected area and add it to list of traps
        createdObjects.Add(Instantiate(selectedTrap, potentialTiles[0], rotation));
        GetComponent<ResourceController>().currentResource -= cost;

        potentialTiles.Clear();
    }
}
