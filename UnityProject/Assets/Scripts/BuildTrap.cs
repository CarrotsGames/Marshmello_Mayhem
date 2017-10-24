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
    float rise;
    int cost;

    bool previewExist;
    GameObject preview;

    bool error;

    //used for leniency for detecting what tile the player is on
    public float playerToTileDistance = 1.5f;

    // Use this for initialization
    void Start ()
    { 
        //if (GameObject.Find("PrefabList").GetComponent<PrefabList>() == null)
        //{
        //    Debug.Log("PrefabList game object doesn't exist");
        //}
        //else
        //{
        //    prefabList = GameObject.Find("PrefabList").GetComponent<PrefabList>();
        //}

        if (FindObjectOfType<PrefabList>() == null)
        {
            Debug.Log("PrefabList game object doesn't exist");
        }
        else
        {
            prefabList = FindObjectOfType<PrefabList>();
        }

        if (GameObject.FindGameObjectsWithTag("Floor") == null)
        {
            Debug.Log("No floor tiles with 'Floor' tag");
        }
        else
        {
            floorGrid = GameObject.FindGameObjectsWithTag("Floor");
        }

        potentialTiles = new List<Vector3>();
        createdObjects = FindObjectOfType<GameController>().placedTraps;

        //checks for missing components
        if (floorGrid.Length > 0)
        {
            if (prefabList.TarPit == null)
            {
                Debug.Log("Tar Pit not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.TarPit.GetComponent<TarPit>() == null)
            {
                Debug.Log("TarPit script not assigned to Tar Pit object");
                error = true;
            }
            if (prefabList.Pit == null)
            {
                Debug.Log("Pit not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.Pit.GetComponent<Pit>() == null)
            {
                Debug.Log("Pit script not assigned to Pit object");
                error = true;
            }
            if (prefabList.PlaceableWall == null)
            {
                Debug.Log("Placeable Wall not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.PlaceableWall.GetComponent<PlaceableWall>() == null)
            {
                Debug.Log("PlaceableWall script not assigned to Placeable Wall object");
                error = true;
            }
            if (prefabList.HumanThrower == null)
            {
                Debug.Log("Human Thrower not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.HumanThrower.GetComponent<HumanThrower>() == null)
            {
                Debug.Log("HumanThrower script not assigned to Human Thrower object");
                error = true;
            }
            if (GetComponent<PlayerController>() == null)
            {
                Debug.Log("PlayerController not attached to player");
                error = true;
            }
            if (GetComponent<ResourceController>() == null)
            {
                Debug.Log("ResourceController not attached to player");
                error = true;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (error == true)
        {
            isEnabled = false;
        }
        //checks if a trap is currently being built
        if (isActive == false && isEnabled == true)
        {            
            if (selectedTrap == null)
            {
                selectedTrap = prefabList.TarPit;
                rise = 0.8f;
                cost = selectedTrap.GetComponent<TarPit>().cost;
            }
            if (XCI.GetDPadDown(XboxDPad.Up, GetComponent<PlayerController>().controller) || Input.GetKey(KeyCode.Alpha1))
            {
                selectedTrap = prefabList.TarPit;
                cost = selectedTrap.GetComponent<TarPit>().cost;
                rise = 0.8f;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Down, GetComponent<PlayerController>().controller) || Input.GetKey(KeyCode.Alpha2))
            {
                selectedTrap = prefabList.Pit;
                cost = selectedTrap.GetComponent<Pit>().cost;
                rise = 0;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Right, GetComponent<PlayerController>().controller) || Input.GetKey(KeyCode.Alpha3))
            {
                selectedTrap = prefabList.PlaceableWall;
                cost = selectedTrap.GetComponent<PlaceableWall>().cost;
                rise = 1.0f;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Left, GetComponent<PlayerController>().controller) || Input.GetKey(KeyCode.Alpha4))
            {                
                selectedTrap = prefabList.HumanThrower;
                cost = selectedTrap.GetComponent<HumanThrower>().cost;
                rise = 1.0f;
                Destroy(preview);
                previewExist = false;
            }

            //sets the rotation to player's previous rotation
            Vector3 forward = GetComponent<PlayerController>().previousRotation;
            Vector3 upwards = new Vector3(0, 1, 0);

            if (selectedTrap.GetComponent<Pit>() != null)
            {
                forward = new Vector3(1, 0, 0);
            }
            
            rotation = Quaternion.LookRotation(forward, upwards);

            //loop over all tiles
            for (int i = 0; i < floorGrid.Length; i++)
            {
                Vector3 vecBetween = transform.position - floorGrid[i].transform.position;
                vecBetween.y = 0;

                if (vecBetween.magnitude < playerToTileDistance)
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

            if (potentialTiles.Count > 0)
            {
                //call preview
                Preview();
            }

            //checks if player has pressed Xbox:A or the space bar
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A, GetComponent<PlayerController>().controller))
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
                            if (vecBetween.magnitude < playerToTileDistance)
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

                                GetComponent<PlayerController>().playerMovement = false;

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

                            GetComponent<PlayerController>().playerMovement = false;

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

            if (XCI.GetButtonDown(XboxButton.B, GetComponent<PlayerController>().controller))
            {
                Destroy(preview);
                previewExist = false;
                isEnabled = false;
            }
        }
        else if (isEnabled == false)
        {
            if (XCI.GetButtonDown(XboxButton.A, GetComponent<PlayerController>().controller) || Input.GetKeyDown(KeyCode.Tab))
            {
                isEnabled = true;
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

        if (selectedTrap == prefabList.HumanThrower)
        {
            //passes current rotation in to human thrower object to determine direction
            createdObjects[createdObjects.Count - 1].GetComponent<HumanThrower>().SetTargetDirection(rotation);
        }

        GetComponent<ResourceController>().currentResource -= cost;
        potentialTiles.Clear();

        GetComponent<PlayerController>().playerMovement = true;

        Destroy(preview);
        previewExist = false;
        isEnabled = false;
    }

    void Preview()
    {
        if (previewExist == false)
        {
            //create ghost object                   
            preview  = Instantiate(selectedTrap, potentialTiles[0], rotation);
            previewExist = true;
        }

        //display where trap will be placed
        for (int i = 0; i < floorGrid.Length; i++)
        {
            if (potentialTiles.Count > 0)
            {
                Vector3 vecbetween = potentialTiles[0] - floorGrid[i].transform.position;

                if (vecbetween.magnitude < playerToTileDistance)
                {
                    //set preview position to closest potential tile
                    preview.transform.position = potentialTiles[0];
                }
            }
        }

        preview.transform.rotation = rotation;

        //disables preview object's scripts
        if (preview.GetComponent<TarPit>() != null)
        {
            preview.GetComponent<TarPit>().enabled = false;
        }
        if (preview.GetComponent<Pit>() != null)
        {
            preview.GetComponent<Pit>().enabled = false;
            preview.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (preview.GetComponent<PlaceableWall>() != null)
        {
            preview.GetComponent<PlaceableWall>().enabled = false;
        }
        if (preview.GetComponent<HumanThrower>() != null)
        {
            preview.GetComponent<HumanThrower>().enabled = false;
        }

        if (preview.GetComponent<Collider>() != null)
        {
            Physics.IgnoreCollision(preview.GetComponent<Collider>(), GetComponent<CharacterController>());
        }
    }
}
