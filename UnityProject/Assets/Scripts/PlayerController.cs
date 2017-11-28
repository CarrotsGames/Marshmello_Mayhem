using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{
    public CharacterController charController;
    public XboxController controller;
    public GunController rayGun;

    //animations
    private Animator animator;
    public bool isShooting;
    public bool isIdle;

    //movement
    public float speed;
    public bool playerMovement = true;
    [HideInInspector] public Vector3 previousRotation = Vector3.forward;
    public GameController.Direction direction;

    //used for capturing enemy's chem canister
    public Transform chemFlagHoldPoint;
    public int teamNumber = 1;
    public float distanceFromChemFlagToPickUp = 2;
    public Transform enemyChemFlag;
    [HideInInspector] public Chem_Flag holdingChemFlag;

    //player's health
    PlayerHealth playerHealth;

    //variables for human thrower launch
    private bool isBeingLaunched = false;
    private float launchHeight;
    float launchSpeed;
    private float timeInAir;
    float timeLeft;
    float originalY;

    //particles
    public ParticleSystem buildingParticles;
    public GameObject chemHoldIndicator;

    bool alternativeControls = false;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        
        //prevents particles from starting
        buildingParticles.Stop();

        
    }

    // Update is called once per frame
    void Update()
    {         
        if (transform.position.y > originalY + 1.5f)
        {
            GetComponent<BuildTrap>().CancelBuildInProgress();
        }
        //check if player is being launched
        if (isBeingLaunched == true)
        {
            GetComponent<BuildTrap>().isEnabled = false;
            
            //count down time
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }

            //if time <= 0, set launching to false
            else if (timeLeft <= 0)
            {
                isBeingLaunched = false;
            }

            float changeInTime = timeInAir - timeLeft;

            //speed = targetHeight - currentHeight / time since start of launch
            launchSpeed = launchHeight / changeInTime;

            //if time > 0, increase player's y by speed
            if (timeLeft > 0)
            {
                transform.Translate(0.0f, launchSpeed / 15, 0.0f);
            }
        }

        if (XCI.GetButtonDown(XboxButton.Back, controller))
        {
            if (alternativeControls == false)
            {
                alternativeControls = true;
            }
            else
            {
                alternativeControls = false;
            }
        }

        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) >= 0.1f || Input.GetKey(KeyCode.Alpha0))
        {
            if (GetComponent<BuildTrap>().isEnabled == false && GetComponent<ResourceController>().currentResource >= FindObjectOfType<GameController>().shootingCost)
            {
                isShooting = true;
                gameObject.GetComponentInChildren<GunController>().display = true;
                rayGun.Shoot();
            }
        }

        //PickUpChemFlag();
        if (holdingChemFlag == null)
        {
            //check if flag is close enough to player
            if (Vector3.Distance(enemyChemFlag.position, transform.position) < distanceFromChemFlagToPickUp)
            {               
                PickUpChemFlag();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Y, controller))
        {
            DropChemFlag();
        }

        if (playerMovement == true)
        {
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);

            //animation changes
            //is moving
            if (axisX != 0 || axisZ != 0)
            {
                if (isShooting == false)
                {
                    animator.SetBool("IsRunning", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsShooting", false);
                    isIdle = false;
                }
            }
            //is shooting
            if (isShooting == true)
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsShooting", true);

            }
            //is idle
            else if (axisX == 0 && axisZ == 0)
            {
                isIdle = true;
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsShooting", false);
            }


            //apply rotation
            RotatePlayer();

            //apply movement
            MovePlayer();
        }

        //simulate gravity for the player
        charController.Move(Vector3.up * -9.8f * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        //change direction enum
        if (previousRotation == new Vector3(0, 0, 1))
        {
            direction = GameController.Direction.UP;
        }
        if (previousRotation == new Vector3(0, 0, -1))
        {
            direction = GameController.Direction.DOWN;
        }
        if (previousRotation == new Vector3(1, 0, 0))
        {
            direction = GameController.Direction.RIGHT;
        }
        if (previousRotation == new Vector3(-1, 0, 0))
        {
            direction = GameController.Direction.LEFT;
        }

        float rotateAxisX = 0;
        float rotateAxisZ = 0;

        if (alternativeControls == true)
        {
            //get controller's left stick input
            rotateAxisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            rotateAxisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        }
        else if (alternativeControls == false)
        {
            //get controller's right stick input
            rotateAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller);
            rotateAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller);
        }

        //set controller input to new vector
        Vector3 directionVector = new Vector3(rotateAxisX, 0, rotateAxisZ);
        //prevents debug log from appearing when vector is 0
        if (directionVector.magnitude < 0.1f)
        {
            directionVector = previousRotation;
        }
        
        directionVector = directionVector.normalized;
        previousRotation = directionVector;

        transform.rotation = Quaternion.LookRotation(directionVector);        
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3();

        //get controller's left stick input
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        //set movement to new vector
        movement = new Vector3(axisX, 0, axisZ);
        
        //apply movement changes to player's character controller
        charController.Move(movement * (speed * Time.deltaTime));        
    }

    public void DropChemFlag()
    {
        //check if the player is holding a chem flag
        if (holdingChemFlag != null)
        {
            //remove the chem flag from the player
            holdingChemFlag = null;
            enemyChemFlag.SetParent(null);
            enemyChemFlag.GetComponent<Chem_Flag>().DropChemFlag();

            //disable 
            chemHoldIndicator.SetActive(false);
        }
    }

    public void PickUpChemFlag()
    {
        if (enemyChemFlag.GetComponent<Chem_Flag>().canBePickedUp == true && enemyChemFlag.GetComponent<Chem_Flag>().isBeingCarried == false)
        {
            if (Vector3.Distance(enemyChemFlag.position, transform.position) < distanceFromChemFlagToPickUp)
            {
                //set flag's parent and position
                enemyChemFlag.SetParent(chemFlagHoldPoint);
                enemyChemFlag.position = chemFlagHoldPoint.position;
                //set wasDropped to false to ensure no respawn until dropped
                enemyChemFlag.GetComponent<Chem_Flag>().wasDropped = false;
                //call pick up function 
                enemyChemFlag.GetComponent<Chem_Flag>().PickUpChemFlag();
                holdingChemFlag = enemyChemFlag.GetComponent<Chem_Flag>();

                //enable partical system
                chemHoldIndicator.SetActive(true);
            }
        }
    }

    public void StartLerp(float a_speed, float a_launchHeight, float a_timeInAir)
    {
        timeInAir = a_timeInAir;
        isBeingLaunched = true;
        launchHeight = a_launchHeight;
        timeLeft = timeInAir;
    }

    public void Knockback(Vector3 a_projectileDirection, float a_knockbackStrength)
    {
            //raycast to detect walls in direction and knockback distance
            Ray ray = new Ray(transform.position, a_projectileDirection);
            RaycastHit hit;

            //if wall is detected, place player on this side of wall
            if (Physics.Raycast(ray, a_knockbackStrength))
            {
                Physics.Raycast(ray, out hit);
            }
            else
            {
                transform.position += a_projectileDirection * a_knockbackStrength;
            }
        
    }
}
