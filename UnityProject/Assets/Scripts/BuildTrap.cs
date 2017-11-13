using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildTrap : MonoBehaviour {

    private Vector3 targetPosition;
    private bool isBuilding = false;
    public int buildTime;
    GameObject selectedTrap;
    List<Vector3> potentialTiles;
    Quaternion rotation;
    List<GameObject> createdObjects;
    public bool isEnabled = false;
    XboxController controller;

    GameObject[] floorGrid;
    
    private PrefabList prefabList;
    private AudioSource audio;
    float rise;
    int cost;

    bool previewExist;
    GameObject preview;

    bool error;

    public bool extraTraps;

    GameObject tempObject;

    //used for leniency for detecting what tile the player is on
    public float playerToTileDistance = 1.8f;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<PlayerController>().controller;

        if (FindObjectOfType<PrefabList>() == null)
        {
            Debug.Log("PrefabList game object doesn't exist");
        }
        else
        {
            prefabList = FindObjectOfType<PrefabList>();
            audio = prefabList.BuildingAudio;
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
            if (prefabList.PillarOfSaws == null)
            {
                Debug.Log("PillarOfSaws not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.PillarOfSaws.GetComponent<Invention_005_PillarOfSaws>() == null)
            {
                Debug.Log("PillarOfSaws script not assigned to Pillar Of Saws object");
                error = true;
            }
            if (prefabList.Anti_StickMatter == null)
            {
                Debug.Log("Anti stick matter not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.Anti_StickMatter.GetComponent<Invention_006_AntiStickMatter>() == null)
            {
                Debug.Log("Anti_stickMatter script not assigned to Anti stick matter object");
                error = true;
            }
            if (prefabList.LastPrayer == null)
            {
                Debug.Log("Last Prayer not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.LastPrayer.GetComponent<Invention_007_LastPrayer>() == null)
            {
                Debug.Log("LastPrayer script not assigned to Last Prayer object");
                error = true;
            }
            if (prefabList.MatterMover == null)
            {
                Debug.Log("Matter Mover not assigned to PrefabList game object");
                error = true;
            }
            if (prefabList.MatterMover.GetComponent<Invention_008_MatterMover>() == null)
            {
                Debug.Log("MatterMover script not assigned to Matter Mover object");
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

        //disable gun controller
        if (isEnabled == true)
        {
            gameObject.GetComponentInChildren<GunController>().display = false;
        }

        //checks if a trap is currently being built
        if (isBuilding == false && isEnabled == true)
        {
            if (selectedTrap == null)
            {
                Default();
            }
            
            if (XCI.GetDPadDown(XboxDPad.Right, controller) || Input.GetKey(KeyCode.Alpha2))
            {
                selectedTrap = prefabList.Pit;
                cost = selectedTrap.GetComponent<Pit>().cost;
                rise = -0.2f;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Up, controller) || Input.GetKey(KeyCode.Alpha1))
            {
                selectedTrap = prefabList.PlaceableWall;
                cost = selectedTrap.GetComponent<PlaceableWall>().cost;
                rise = 0.5f;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Left, controller) || Input.GetKey(KeyCode.Alpha3))
            {
                selectedTrap = prefabList.HumanThrower;
                cost = selectedTrap.GetComponent<HumanThrower>().cost;
                rise = 0.6f;
                Destroy(preview);
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Down, controller) || Input.GetKey(KeyCode.Alpha4))
            {
                selectedTrap = prefabList.LastPrayer;
                cost = selectedTrap.GetComponent<Invention_007_LastPrayer>().cost;
                rise = 1.2f;
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

            //sort for lowest magnitude
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
                //check if potentialTiles is an already existing trap
                for (int i = 0; i < createdObjects.Count; i++)
                {
                    if (potentialTiles[0] == createdObjects[i].transform.position)
                    {
                        potentialTiles.Remove(potentialTiles[0]);
                    }
                }

                //call preview
                Preview();
            }

            //checks if player has pressed Xbox:A or the space bar
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A, controller))
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
                                isBuilding = true;
                                Debug.Log("Building started");

                                GetComponent<PlayerController>().playerMovement = false;

                                //play audio
                                audio.Play();

                                tempObject = Instantiate(selectedTrap, potentialTiles[0], rotation);
                                tempObject.SetActive(false);
                                //create trap at position closest to selected area and add it to list of traps
                                createdObjects.Add(tempObject);

                                //calls Build function after delay (seconds)
                                Invoke("Build", buildTime);
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
                            isBuilding = true;
                            Debug.Log("Building started");

                            GetComponent<PlayerController>().playerMovement = false;

                            //play audio
                            audio.Play();

                            //create trap at position closest to selected area and add it to list of traps
                            //createdObjects.Add(Instantiate(selectedTrap, potentialTiles[0], rotation));

                            tempObject = Instantiate(selectedTrap, potentialTiles[0], rotation);
                            tempObject.SetActive(false);
                            //create trap at position closest to selected area and add it to list of traps
                            createdObjects.Add(tempObject);

                            //calls Build function after delay (seconds)
                            Invoke("Build", buildTime);
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

            if (XCI.GetButtonDown(XboxButton.B, controller))
            {
                Destroy(preview);
                previewExist = false;
                isEnabled = false;
            }
        }
        else if (isEnabled == false)
        {
            if (XCI.GetButtonDown(XboxButton.A, controller) || Input.GetKeyDown(KeyCode.Tab))
            {
                isEnabled = true;
            }

            //enable gun controller
            gameObject.GetComponentInChildren<GunController>().display = true;
            
        }
	}

    void Build ()
    {
        //re-enables ability to build a trap
        isBuilding = false;
        Debug.Log("Building finished");

        //stops audio clip
        audio.Stop();

        tempObject.SetActive(true);


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
            preview = Instantiate(selectedTrap, potentialTiles[0], rotation);

            //change material of preview object
            preview.GetComponentInChildren<MeshRenderer>().material = FindObjectOfType<GameController>().trapPreviewMaterial;

            if (selectedTrap.GetComponent<HumanThrower>() != null)
            {                
                 preview.GetComponentInChildren<MeshRenderer>().materials[1] = FindObjectOfType<GameController>().trapPreviewMaterial;                               
            }

            previewExist = true;
        }


        //display where trap will be placed
        for (int i = 0; i < floorGrid.Length; i++)
        {
            if (potentialTiles.Count > 0)
            {
                Vector3 vecbetween = potentialTiles[0] - floorGrid[i].transform.position;
                vecbetween.y = 0;

                if (vecbetween.magnitude < playerToTileDistance)
                {
                    //set preview position to closest potential tile
                    preview.transform.position = potentialTiles[0];
                }
            }
        }
       
        preview.transform.rotation = rotation;

        //disables preview object's scripts
        if (FindObjectOfType<PrefabList>() != null)
        {
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
            if (preview.GetComponent<Invention_007_LastPrayer>() != null)
            {
                preview.GetComponent<Invention_007_LastPrayer>().enabled = false;
            }
            if (preview.GetComponent<Invention_005_PillarOfSaws>() != null)
            {
                preview.GetComponent<Invention_005_PillarOfSaws>().enabled = false;
            }
            if (preview.GetComponent<Invention_006_AntiStickMatter>() != null)
            {
                preview.GetComponent<Invention_006_AntiStickMatter>().enabled = false;
            }
            if (preview.GetComponent<Invention_008_MatterMover>() != null)
            {
                preview.GetComponent<Invention_008_MatterMover>().enabled = false;
            }
        }
        //disables colliders
        if (preview.GetComponent<Collider>() != null)
        {
            preview.GetComponent<Collider>().enabled = false;
        }
    }

    void Default()
    {
        if (extraTraps == false)
        {
            selectedTrap = prefabList.LastPrayer;
            cost = selectedTrap.GetComponent<Invention_007_LastPrayer>().cost;
            rise = 1.2f;
            if (preview != null)
            {
                Destroy(preview);
                previewExist = false;
            }
        }
    }
}
