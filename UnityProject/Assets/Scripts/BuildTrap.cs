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
    private int rows = 11;
    private int columns = 24;
    List<GameObject> createdObjects;
    public bool isEnabled = false;

	// Use this for initialization
	void Start ()
    {
        //gets player's original position, return to this at end
        Vector3 originalPlayerPos = GetComponent<PlayerController>().transform.position;
        //disable player's movement
        GetComponent<PlayerController>().playerMovement = false;

        //manually adjust for world position offset
        originTile.x = 19.33f;
        originTile.y = 7.7595f;
        originTile.z = -7.788241f;

        grid = new Vector3[rows, columns];

        potentialTiles = new List<Vector3>();
        createdObjects = new List<GameObject>();

        //creates grid
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                grid[r, c].z = originTile.z + (r * 3) - 12;
                grid[r, c].y = originTile.y;
                grid[r, c].x = originTile.x + (c * 2);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //checks if a trap is currently being built
        if (isActive == false && isEnabled == true)
        {
            if (XCI.GetDPad(XboxDPad.Up) || Input.GetKey(KeyCode.Alpha1))
            {
                selectedTrap = Resources.Load("Prefabs/Invention_001_Lab_001_TarPit") as GameObject;                
            }
            if (XCI.GetDPad(XboxDPad.Down) || Input.GetKey(KeyCode.Alpha2))
            {
                selectedTrap = Resources.Load("Prefabs/Invention_002_Lab_002_Pit") as GameObject;
            }
            if (XCI.GetDPad(XboxDPad.Right) || Input.GetKey(KeyCode.Alpha3))
            {
                selectedTrap = Resources.Load("Prefabs/Invention_003_Lab_003_PlaceableWall") as GameObject;
            }
            if (XCI.GetDPad(XboxDPad.Left) || Input.GetKey(KeyCode.Alpha4))
            {
                selectedTrap = Resources.Load("Prefabs/Invention_004_Lab_004_HumanThrower") as GameObject;
            }


            //checks if player has pressed Xbox:A or the space bar
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A))
            {
                //loop over all tiles
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        //check for closest tile
                        Vector3 vecBetween = transform.position - grid[r, c];
                        vecBetween.y = 0;

                        if (vecBetween.magnitude < 2)
                        {
                            targetPosition = grid[r, c];
                            targetPosition.y += 4.1f;
                            potentialTiles.Add(targetPosition);
                        }
                    }
                }

                //checks if player is within range of any tiles
                if (potentialTiles.Count > 0)
                {
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
                        Debug.Log("Building disabled");

                        //calls Build function after delay (seconds)
                        Invoke("Build", delay);
                    }
                }
            }
        }
	}

    //after 15 debug messages, stuck with trap "always in range"

    void Build ()
    {
        Debug.Log("Trap Successfully built");
        //re-enables ability to build a trap
        isActive = false;
        Debug.Log("Building Enabled");
        //re-enables player movement
        GetComponent<PlayerController>().playerMovement = true;

        //create trap at position closest to selected area and add it to list of traps
        createdObjects.Add(Instantiate(selectedTrap, potentialTiles[0], rotation));
        
        potentialTiles.Clear();
    }
}
