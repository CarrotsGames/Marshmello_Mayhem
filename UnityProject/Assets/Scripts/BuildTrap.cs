using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildTrap : MonoBehaviour {

    private Vector3 targetPosition;
    private Vector3[,] grid;
    private bool isActive = false;
    public int delay;
    public GameObject selectedTrap;
    Vector3 originTile;
    List<Vector3> potentialTiles;
    Quaternion rotation;
    List<GameObject> createdObjects;
    public bool isEnabled = false;
    GameObject[] floorGrid;
    private PrefabList prefabList;
    public float rise;
    GameObject ghost;
    

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
                rise = 1.2f;
            }
            if (XCI.GetDPad(XboxDPad.Down) || Input.GetKey(KeyCode.Alpha2))
            {
                selectedTrap = prefabList.Pit;
                rise = 0.2f;
            }
            if (XCI.GetDPad(XboxDPad.Right) || Input.GetKey(KeyCode.Alpha3))
            {
                selectedTrap = prefabList.PlaceableWall;
                rise = 1.0f;
            }
            if (XCI.GetDPad(XboxDPad.Left) || Input.GetKey(KeyCode.Alpha4))
            {
                selectedTrap = prefabList.HumanThrower;
                rise = 1.0f;
            }

            //loop over all tiles (tiles)
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
            //    if (potentialTiles[0] == floorGrid[i].transform.position)
            //    {
            //        ghost = Instantiate<GameObject>(selectedTrap, potentialTiles[0], rotation);
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
                            //set isActive to true, disables this until false
                            isActive = true;

                            Debug.Log("Building disabled");

                            //calls Build function after delay (seconds)
                            Invoke("Build", delay);
                        }
                        else
                        {
                            Debug.Log("An invention already exists there!");

                            potentialTiles.Clear();
                        }
                    }
                    else
                    {
                        //set isActive to true, disables this until false
                        isActive = true;
                        Debug.Log("Building started");

                        //calls Build function after delay (seconds)
                        Invoke("Build", delay);
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
        
        potentialTiles.Clear();
    }
}
