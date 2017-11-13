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

    public bool playerMovement = true;

    public Transform chemFlagHoldPoint;

    public int teamNumber = 1;
    public float distanceFromChemFlagToPickUp = 2;

    public Transform enemyChemFlag;

    PlayerHealth playerHealth;

    public Chem_Flag holdingChemFlag;
    [HideInInspector] public Vector3 previousRotation = Vector3.forward;

    public GameController.Direction direction;

    //variables for lerp
    private bool isLerping = false;
    private float launchHeight;
    float launchSpeed;
    private float timeInAir;
    float timeLeft;


    //(blue = 1, red = 2);

    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();

        if (GetComponent<PlayerHealth>() != null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.Log("Missing PlayerHealth script on player");
        }
        if (enemyChemFlag == null)
        {
            Debug.Log("Nothing assigned to Player's 'Enemy Chem Flag'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is being launched
        if (isLerping == true)
        {
            //count down time
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }

            //if time <= 0, set launching to false
            else if (timeLeft <= 0)
            {
                isLerping = false;

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

        RotatePlayer();


        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) >= 0.1f || Input.GetKey(KeyCode.Alpha0))
        {
            rayGun.Shoot();
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
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal * (speed * Time.deltaTime), 0, moveVertical * (speed * Time.deltaTime));

            MovePlayer();
        }
    }

    private void RotatePlayer()
    {
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

    public void DropChemFlag()
    {
        if (holdingChemFlag != null)
        {
            holdingChemFlag = null;
            enemyChemFlag.SetParent(null);
            enemyChemFlag.GetComponent<Chem_Flag>().DropChemFlag();
        }
    }

    public void PickUpChemFlag()
    {
        if (enemyChemFlag.GetComponent<Chem_Flag>().canBePickedUp == true)
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

            }
        }
    }

    public void StartLerp(float a_speed, float a_launchHeight, float a_timeInAir)
    {
        timeInAir = a_timeInAir;
        isLerping = true;
        launchHeight = a_launchHeight;
        //launchSpeed = a_speed;

        timeLeft = timeInAir;
    }

    public void Knockback(Vector3 a_projectileDirection, float a_knockbackStrength)
    {
        Ray ray = new Ray(transform.position, a_projectileDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, a_knockbackStrength))
        {
            Physics.Raycast(ray, out hit);

            transform.position = hit.transform.position - a_projectileDirection;
        }
        else
        {
            transform.position += a_projectileDirection * a_knockbackStrength;
        }
    }
}
