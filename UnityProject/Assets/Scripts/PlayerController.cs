using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{

    public CharacterController charController;
    public XboxController controller;
    public GunController rayGun;
    //public MeleeAttack meleeHitbox;
    public float speed;
    public float maxSpeed = 5;

    //private float attackDelay;
    //public float attackDamage;
    public bool playerMovement;

	public Transform chemFlagHoldPoint;

	public int teamNumber = 1;
	public float distanceFromChemFlagToPickUp = 2;

	public Transform enemyChemFlag;

    PlayerHealth playerHealth;

    [HideInInspector] public Chem_Flag holdingChemFlag;
    [HideInInspector] public bool isBeingLaunched;
    [HideInInspector] public Vector3 previousRotation = Vector3.forward;

	//(blue = 1, red = 2);

    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerMovement = true;

        if (GetComponent<PlayerHealth>() != null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.Log("Missing PlayerHealth script on player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null)
        {
            if (playerHealth.isAlive == false)
            {
                playerMovement = false;
            }
            if (playerHealth.isAlive == true)
            {
                playerMovement = true;
            }
        }

        RotatePlayer();


        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) >= 0.1f || Input.GetKey(KeyCode.Alpha0))
        {
            
             rayGun.Shoot();
            
        }

        //if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
        //{
        //    meleeHitbox.isSwinging = true;
        //}

		if (XCI.GetButtonDown(XboxButton.Y,controller))
		{
            DropChemFlag();
		}

        if (playerMovement == true)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal * (speed * Time.deltaTime), 0, moveVertical * (speed * Time.deltaTime));

            //transform.Translate(movement);
            MovePlayer();
        }
    }		

    private void RotatePlayer()
    {
        float rotateAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller);
        float rotateAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller);
        Vector3 directionVector = new Vector3(rotateAxisX, 0, rotateAxisZ);

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
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        Vector3 movement = new Vector3(axisX, 0, axisZ);

        charController.Move(movement * (speed * Time.deltaTime) + Vector3.up * -9.8f * Time.deltaTime);
    }

    //move the player in an arc to the target position (used for humanthrower)
    public void Launch(Vector3 a_target)
    {
        while (isBeingLaunched == true)
        {
            //use slerp or lerp to move position

            //check if player position == a_target position (excluding y)

            isBeingLaunched = false;
        }

    }

    public void DropChemFlag()
    {
        if (holdingChemFlag == null)
        {
            if (Vector3.Distance(enemyChemFlag.position, transform.position) < distanceFromChemFlagToPickUp)
            {
                enemyChemFlag.SetParent(chemFlagHoldPoint);
                enemyChemFlag.position = chemFlagHoldPoint.position;
                enemyChemFlag.GetComponent<Chem_Flag>().PickUpChemFlag();
                holdingChemFlag = enemyChemFlag.GetComponent<Chem_Flag>();
            }
        }
        else
        {
            holdingChemFlag.DropChemFlag();
            holdingChemFlag = null;
            enemyChemFlag.GetComponent<Chem_Flag>().DropChemFlag();
        }
    }
}
