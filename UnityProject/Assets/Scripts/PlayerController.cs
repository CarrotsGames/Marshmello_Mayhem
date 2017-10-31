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

    [HideInInspector] public Chem_Flag holdingChemFlag;
    [HideInInspector] public Vector3 previousRotation = Vector3.forward;

    public GameController.Direction direction;

    //variables for lerp
    private bool isLerping = false;
    private float lerpTimer;
    private float lerpDuration;
    private Vector3 lerpStartPosition;
    private Vector3 lerpTargetPosition;
    private AnimationCurve arcCurve;

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
            lerpTimer += Time.deltaTime;

            //calculate value from 0 - 1 which is percentage of movement completed
            float lerpValue = lerpTimer / lerpDuration;

            if (lerpValue >= 1.0f)
            {
                lerpValue = 1.0f;
                isLerping = false;
            }

            playerHealth.transform.position = Vector3.Lerp(lerpStartPosition, lerpTargetPosition, lerpValue) + Vector3.up * arcCurve.Evaluate(lerpValue);
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

        Vector3 movement = new Vector3(axisX, 0, axisZ);

        charController.Move(movement * (speed * Time.deltaTime) + Vector3.up * -9.8f * Time.deltaTime);
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

    public void StartLerp(Vector3 a_targetPosition, float a_lerpDuration, AnimationCurve a_arcCurve)
    {
        isLerping = true;
        lerpStartPosition = transform.position;
        lerpTargetPosition = a_targetPosition + Vector3.up * 1.0f;
        lerpTimer = 0.0f;
        lerpDuration = a_lerpDuration;
        arcCurve = a_arcCurve;
    }
}
