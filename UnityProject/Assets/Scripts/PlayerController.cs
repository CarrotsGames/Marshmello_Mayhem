using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{

    public CharacterController charController;
    public XboxController controller;
    public GunController rayGun;
    public MeleeAttack meleeHitbox;
    public float speed;
    public float maxSpeed = 5;
    public Vector3 previousRotation = Vector3.forward;
    private float attackDelay;
    public float attackDamage;
    public bool playerMovement;

    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
		if(XCI.GetButtonDown(XboxButton.LeftBumper,controller))
        {
            rayGun.isFiring = true;
        }
		if(XCI.GetButtonUp(XboxButton.LeftBumper,controller))
        {
            rayGun.isFiring = false;
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            meleeHitbox.isSwinging = true;
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            meleeHitbox.isSwinging = false;
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

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal * (speed * Time.deltaTime), 0, moveVertical * (speed * Time.deltaTime));

        //transform.Translate(movement);
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (playerMovement == true)
        {
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);

            Vector3 movement = new Vector3(axisX, 0, axisZ);

            charController.Move(movement * speed * Time.deltaTime + Vector3.up * -9.8f * Time.deltaTime);
        }
    }
}
