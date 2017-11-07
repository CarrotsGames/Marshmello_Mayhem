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

    public bool playerMovement;

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
    private float lerpTimer;
    private float lerpDuration;
    private Vector3 lerpStartPosition;
    private Vector3 lerpTargetPosition;
    private AnimationCurve arcCurve;
    private float launchHeight;
    float height;
    float launchSpeed;

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
        if (enemyChemFlag == null)
        {
            Debug.Log("Nothing assigned to Player's 'Enemy Chem Flag'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isLerping == true)
        {
            
            if (lerpTimer == 0)
            {
                height = transform.position.y + launchHeight;
            }
            lerpTimer += Time.deltaTime;

            //calculate value from 0 - 1 which is percentage of movement completed
            //float lerpValue = lerpTimer / lerpDuration;
            //
            //if (lerpValue >= 1.0f)
            //{
            //    lerpValue = 1.0f;
            //    isLerping = false;
            //}
            //
            //playerHealth.transform.position = Vector3.Lerp(lerpStartPosition, lerpTargetPosition + Vector3.up * arcCurve.Evaluate(lerpValue);
            float lerpValue = launchSpeed / launchHeight;
            float y = (transform.position.y - launchHeight) / launchSpeed;
            
            if (transform.position.y >= height * 0.75)
            {
                isLerping = false;
                lerpTimer = 0;
            }
            else if (transform.position.y < height && isLerping)
            {
                transform.position += new Vector3(0, lerpValue - y, 0);

            }
            else
            {
                lerpTimer = 0;
                isLerping = false;
            }
            
        }

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

        //PickUpChemFlag();
        if (holdingChemFlag == null)
        {
            if (Vector3.Distance(enemyChemFlag.position, transform.position) < distanceFromChemFlagToPickUp)
            {
                enemyChemFlag.SetParent(chemFlagHoldPoint);
                enemyChemFlag.position = chemFlagHoldPoint.position;
                enemyChemFlag.GetComponent<Chem_Flag>().wasDropped = false;
                enemyChemFlag.GetComponent<Chem_Flag>().PickUpChemFlag();
                holdingChemFlag = enemyChemFlag.GetComponent<Chem_Flag>();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Y,controller))
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
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);


        //REMOVE
        if (controller == XboxController.First)
        {
            if (Input.GetKey(KeyCode.W))
            {
                axisZ = 1.0f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                axisZ = -1.0f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                axisX = 1.0f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                axisX = -1.0f;
            }
        }
        //THIS


        Vector3 movement = new Vector3(axisX, 0, axisZ);

        charController.Move(movement * (speed * Time.deltaTime) + Vector3.up * -9.8f * Time.deltaTime);
    }

    public void DropChemFlag()
    {        
        if (holdingChemFlag != null)
        {
            //holdingChemFlag.DropChemFlag();
            holdingChemFlag = null;
            enemyChemFlag.GetComponent<Chem_Flag>().wasDropped = true;
            enemyChemFlag.GetComponent<Chem_Flag>().DropChemFlag();
        }
    }

    public void StartLerp(float a_speed, float a_launchHeight)
    {
        isLerping = true;
        lerpStartPosition = transform.position;
        lerpTimer = 0.0f;
        launchHeight = a_launchHeight;
        launchSpeed = a_speed;
    }

    public void PickUpChemFlag()
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
    }
}
