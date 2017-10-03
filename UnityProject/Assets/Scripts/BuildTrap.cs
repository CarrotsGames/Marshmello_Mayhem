using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        grid = new Vector3[15, 26];

        potentialTiles = new List<Vector3>();

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
        //checks if player has pressed ______ button, and if a trap is currently being built
        if (Input.GetButtonDown("Fire1") && isActive == false)
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
                //set isActive to true, disables this until false
                isActive = true;
                Debug.Log("Building disabled");

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
                                
                //calls Build function after delay (seconds)
                Invoke("Build", delay);
            }
        }
	}

    void Build ()
    {
        Debug.Log("Trap Successfully built");
        //re-enables ability to build a trap
        isActive = false;
        Debug.Log("Building Enabled");
        //re-enables player movement
        GetComponent<PlayerController>().playerMovement = true;

        //create trap at position closest to selected area
        Instantiate(selectedTrap, potentialTiles[0], rotation);
        potentialTiles.Clear();
    }
}
