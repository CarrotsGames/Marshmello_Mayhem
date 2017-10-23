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
    public Vector3 previousRotation = Vector3.forward;
    private float attackDelay;
    public float attackDamage;
    public bool playerMovement;

	public Transform chemFlagHoldPoint;
	private Chem_Flag holdingChemFlag;
	public int teamNumber = 1;
	public float distanceFromChemFlagToPickUp = 2;

	public Transform ememyChemFlag;

    public PlayerHealth playerHealth;

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

		if (XCI.GetButtonDown(XboxButton.LeftBumper, controller))
        {
            rayGun.Shoot();
        }
        //if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
        //{
        //    meleeHitbox.isSwinging = true;
        //}

		if (XCI.GetButtonDown(XboxButton.Y,controller))
		{
			if (holdingChemFlag == null)
            {
				if (Vector3.Distance(ememyChemFlag.position, transform.position) < distanceFromChemFlagToPickUp)
                {
					ememyChemFlag.SetParent(chemFlagHoldPoint);
					ememyChemFlag.position = chemFlagHoldPoint.position;
					ememyChemFlag.GetComponent<Chem_Flag>().PickUpChemFlag();
					holdingChemFlag = ememyChemFlag.GetComponent<Chem_Flag>();
				}
			}
            else
            {
				holdingChemFlag.DropChemFlag();
				holdingChemFlag = null;
				ememyChemFlag.GetComponent<Chem_Flag>().DropChemFlag();
			}
		}

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal * (speed * Time.deltaTime), 0, moveVertical * (speed * Time.deltaTime));

        //transform.Translate(movement);
        MovePlayer();
    }
		
//
//	public void PickUpChemFlag(Chem_Flag chemFlag)
//  {
//		chemFlag.transform.SetParent (chemFlagHoldPoint);
//		holdingChemFlag = chemFlag;
//	}

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
        if (playerMovement == true)
        {
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);

            Vector3 movement = new Vector3(axisX, 0, axisZ);

            charController.Move(movement * (speed * Time.deltaTime) + Vector3.up * -9.8f * Time.deltaTime);
        }
    }
}
