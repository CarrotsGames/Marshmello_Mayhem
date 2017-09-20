using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour {

    public Rigidbody rigidBody;
    public XboxController controller;

    public float speed;
    public float maxSpeed = 5;
    public Vector3 previousRotation = Vector3.forward;
    private float attackDelay;
    public float attackDamage;
    public float timeBetweenAttack = 0.02f;
    public bool playerMovement = true;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
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

        transform.Translate(movement);
        MovePlayer();
    }

    private void MovePlayer()
    {
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        Vector3 movement = new Vector3(axisX, 0, axisZ);

        rigidBody.AddForce(movement * speed);

        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }
    }
}
