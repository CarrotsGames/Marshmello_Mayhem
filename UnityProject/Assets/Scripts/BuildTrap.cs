using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildTrap : MonoBehaviour {

    private Vector3 targetPosition;
    private bool isBuilding = false;
    public float buildTime;
    [HideInInspector] public GameObject selectedTrap;
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

    public bool extraTraps;

    GameObject tempObject;
    public bool canBuild;
    float timer;

    //used for leniency for detecting what tile the player is on
    public float playerToTileDistance = 1.8f;

    public GameObject targetBuildArea;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<PlayerController>().controller;
        prefabList = FindObjectOfType<PrefabList>();
        audio = prefabList.BuildingAudio;
        floorGrid = GameObject.FindGameObjectsWithTag("Floor");        

        potentialTiles = new List<Vector3>();
        createdObjects = FindObjectOfType<GameController>().placedTraps;
        canBuild = true;
        timer = buildTime;
    }

    // Update is called once per frame
    void Update ()
    {
        if (controller != GetComponent<PlayerController>().controller)
        {
            controller = GetComponent<PlayerController>().controller;
        }
        if (previewExist == false)
        {
            Destroy(preview);
        }

        if (isBuilding == true)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && canBuild == true)
            {       
                
                Build();
                
                timer = buildTime;
            }
            else if (canBuild == false)
            {
                CancelBuildInProgress();
            }
        }

        if (isEnabled == false)
        {
            //change trap dependant on xbox controller's d-pad
            if (XCI.GetDPadDown(XboxDPad.Right, controller) || Input.GetKey(KeyCode.Alpha2))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.Pit;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<Pit>().cost;
                //sets height to be placed at
                rise = -0.2f;
            }
            if (XCI.GetDPadDown(XboxDPad.Up, controller) || Input.GetKey(KeyCode.Alpha1))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.PlaceableWall;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<PlaceableWall>().cost;
                //sets height to be placed at
                rise = 0.5f;
            }
            if (XCI.GetDPadDown(XboxDPad.Left, controller) || Input.GetKey(KeyCode.Alpha3))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.HumanThrower;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<HumanThrower>().cost;
                //sets height to be placed at
                rise = 0.6f;
            }
            if (XCI.GetDPadDown(XboxDPad.Down, controller) || Input.GetKey(KeyCode.Alpha4))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.LastPrayer;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<Invention_007_LastPrayer>().cost;
                //sets height to be placed at
                rise = 1.2f;
            }
        }

        //checks if a build mode is active
        if (isBuilding == false && isEnabled == true)
        {
            if (selectedTrap == null)
            {
                Default();
            }

            //change trap dependant on xbox controller's d-pad
            if (XCI.GetDPadDown(XboxDPad.Right, controller) || Input.GetKey(KeyCode.Alpha2))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.Pit;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<Pit>().cost;
                //sets height to be placed at
                rise = -0.2f;
                //destroy old preview object
                Destroy(preview);
                //allow new preview object to be created
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Up, controller) || Input.GetKey(KeyCode.Alpha1))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.PlaceableWall;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<PlaceableWall>().cost;
                //sets height to be placed at
                rise = 0.5f;
                //destroy old preview object
                Destroy(preview);
                //allow new preview object to be created
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Left, controller) || Input.GetKey(KeyCode.Alpha3))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.HumanThrower;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<HumanThrower>().cost;
                //sets height to be placed at
                rise = 0.6f;
                //destroy old preview object
                Destroy(preview);
                //allow new preview object to be created
                previewExist = false;
            }
            if (XCI.GetDPadDown(XboxDPad.Down, controller) || Input.GetKey(KeyCode.Alpha4))
            {
                //set the selected trap to object in prefabList
                selectedTrap = prefabList.LastPrayer;
                //get resource cost of the trap type
                cost = selectedTrap.GetComponent<Invention_007_LastPrayer>().cost;
                //sets height to be placed at
                rise = 1.2f;
                //destroy old preview object
                Destroy(preview);
                //allow new preview object to be created
                previewExist = false;
            }

            //sets the rotation to player's previous rotation
            Vector3 forward = GetComponent<PlayerController>().previousRotation;
            Vector3 upwards = new Vector3(0, 1, 0);

            //lock pit rotation
            if (selectedTrap.GetComponent<Pit>() != null)
            {
                forward = new Vector3(1, 0, 0);
            }

            rotation = Quaternion.LookRotation(forward, upwards);

            //search through all tiles for tiles within range of the player
            for (int i = 0; i < floorGrid.Length; i++)
            {
                //get distance between player and tile
                Vector3 vecBetween = targetBuildArea.transform.position - floorGrid[i].transform.position;
                vecBetween.y = 0;

                //find nearest tile to player
                if (vecBetween.magnitude < playerToTileDistance)
                {
                    if (floorGrid[i].GetComponent<Tile>().isOccupied == false)
                    {
                        //if closest tile is not occupied, set that tile as the target
                        targetPosition = floorGrid[i].transform.position;
                        targetPosition.y += rise;
                        //if more than one tile is within range, use this to sort
                        potentialTiles.Add(targetPosition);
                    }
                    //check if the tile is occupied, if true destroy preview and disable build mode
                    if (floorGrid[i].GetComponent<Tile>().isOccupied == true)
                    {
                        previewExist = false;
                        Destroy(preview);
                    }
                }
            }

            if (potentialTiles.Count > 0)
            {
                //sort for lowest magnitude
                for (int e = 0; e < potentialTiles.Count; e++)
                {
                    for (int i = 0; i < potentialTiles.Count - 1; i++)
                    {
                        Vector3 vecBetweenCurrent = targetBuildArea.transform.position - potentialTiles[i];
                        vecBetweenCurrent.y = 0;
                        Vector3 vecBetweenNext = targetBuildArea.transform.position - potentialTiles[i + 1];
                        vecBetweenNext.y = 0;

                        if (vecBetweenCurrent.magnitude > vecBetweenNext.magnitude)
                        {
                            Vector3 temp = potentialTiles[i + 1];
                            potentialTiles[i + 1] = potentialTiles[i];

                            potentialTiles[i] = temp;
                        }
                    }
                }
            }
            //remove any potential tiles if trap is already there
            if (potentialTiles.Count > 0)
            {
                if (createdObjects.Count > 0)
                {
                    for (int i = 0; i < createdObjects.Count; i++)
                    {
                        Vector3 vecBetween = potentialTiles[0] - createdObjects[i].transform.position;
                        vecBetween.y = 0;
                        if (vecBetween.magnitude < 0.5)
                        {
                            potentialTiles.Remove(potentialTiles[0]);
                        }

                        if (potentialTiles.Count > 0)
                        {
                            //create preview object at first sorted potential tile
                            Preview();
                        }
                        else
                        {
                            //destroy preview object if there is no longer a potential tile
                            Destroy(preview);
                            previewExist = false;
                        }

                    }
                }
                else
                {
                    //create preview object at first sorted potential tile
                    Preview();
                }

            }
            //destroy preview object when there are no potential tiles
            else if (potentialTiles.Count == 0)
            {
                Destroy(preview);
                previewExist = false;
            }

            //checks if player has pressed Xbox:A or the space bar
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A, controller))
            {
                //checks if player is within range of any tiles
                if (potentialTiles.Count > 0)
                {
                    if (NotEnoughResources() == false)
                    {
                        //set isActive to true, disables this until false
                        isBuilding = true;
                        GetComponent<PlayerController>().playerMovement = false;

                        //play audio
                        audio.Play();

                        //create new temporary object to store new trap's information
                        tempObject = Instantiate(selectedTrap, potentialTiles[0], rotation);
                        tempObject.SetActive(false);

                        //create trap at position closest to selected area and add it to list of traps
                        createdObjects.Add(tempObject);

                        //play particle system
                        GetComponent<PlayerController>().buildingParticles.Play();

                        
                    }

                    //clean potential tiles
                    potentialTiles.Clear();
                }
            }
            else
            {
                //clean potential tiles
                potentialTiles.Clear();
            }

            //deactivate build mode
            if (XCI.GetButtonDown(XboxButton.B, controller))
            {
                isEnabled = false;
            }
        }
        else if (isEnabled == false)
        {
            //ensure preview object is destroyed while build mode is not active
            Destroy(preview);
            previewExist = false;
            //activate build mode
            if (XCI.GetButtonDown(XboxButton.A, controller) || Input.GetKeyDown(KeyCode.Tab))
            {
                isEnabled = true;
            }
        }
	}

    void Build ()
    {
        //re-enables ability to build a trap
        isBuilding = false;
        
        //stops audio clip
        audio.Stop();
        
        tempObject.SetActive(true);
        
        //cost reduction from player's resources
        GetComponent<ResourceController>().currentResource -= cost;

        potentialTiles.Clear();

        //re-enable player's movement
        GetComponent<PlayerController>().playerMovement = true;

        previewExist = false;

        //deactivate build mode when new trap is built
        isEnabled = false;

        GetComponent<PlayerHealth>().ResetHitCounter();

        GetComponent<PlayerController>().buildingParticles.Stop();
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
                 preview.GetComponentInChildren<MeshRenderer>().materials[0] = FindObjectOfType<GameController>().trapPreviewMaterial;                               
            }

            //disables animations for preview object
            if (preview.GetComponent<Animator>() != null)
            {
                preview.GetComponent<Animator>().enabled = false;
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
                Invention_007_LastPrayer temp = preview.GetComponent<Invention_007_LastPrayer>();

                for (int i = 0; i < temp.explosionParticles.Length; i++)
                {
                    temp.explosionParticles[i].Stop();
                }

                temp.enabled = false;
            }
        }
        //disables colliders
        if (preview.GetComponent<Collider>() != null)
        {
            preview.GetComponent<Collider>().enabled = false;
        }
    }

    //will be called when build mode is activated to ensure selected trap is not null
    void Default()
    {        
        selectedTrap = prefabList.LastPrayer;
        cost = selectedTrap.GetComponent<Invention_007_LastPrayer>().cost;
        rise = 1.2f; 
    }

    public void CancelBuildInProgress()
    {
        if (isEnabled == true)
        {
            createdObjects.Remove(tempObject);
            Destroy(tempObject);
        }
        canBuild = true;
        isBuilding = false;

        audio.Stop();
        //re-enable player's movement
        GetComponent<PlayerController>().playerMovement = true;
        GetComponent<PlayerController>().buildingParticles.Stop();

        timer = buildTime;
    }

    public bool GetIsBuilding()
    {
        return isBuilding;
    }

    public bool NotEnoughResources()
    {
        if (GetComponent<ResourceController>().currentResource - cost >= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
