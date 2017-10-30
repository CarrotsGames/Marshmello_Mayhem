using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanThrower : TrapBehaviour {

    public GameObject startTile;
    public GameObject endTile;
    private float startTime;
    GameObject[] grid;
    public GameController.Direction direction;

    //test variables
    GameObject player;

    //slerping
    private bool isLerping = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float lerpTimer = 0.0f;
    public float lerpDuration = 1.5f;
    private float timer = 0.0f;
    public float timeBeforeActivation = 5.0f;

    [SerializeField] private AnimationCurve arcCurve;

    // Use this for initialization
    void Start () {
        grid = GameObject.FindGameObjectsWithTag("Floor");

        for (int i = 0; i < grid.Length; i++)
        {
            Vector3 vecBetween = transform.position - grid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= 1.2)
            {
                startTile = grid[i];
            }
        }        

        //variable to determine where to land
        Vector3 pos = startTile.transform.position;

        if (direction == GameController.Direction.UP)
        {
            pos.z += 5;
        }
        if (direction == GameController.Direction.DOWN)
        {
            pos.z -= 5;
        }
        if (direction == GameController.Direction.RIGHT)
        {
            pos.x += 4;
        }
        if (direction == GameController.Direction.LEFT)
        {
            pos.x -= 4;
        }

        //gets end position of arc
        for (int i = 0; i < grid.Length; i++)
        {
            Vector3 vecBetween = pos - grid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= 1.2)
            {
                endTile = grid[i];
            }
        }

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= timeBeforeActivation)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider a_col)
    {
        player = a_col.gameObject;

        isLerping = true;
        startPosition = player.transform.position;
        targetPosition = endTile.transform.position + Vector3.up * 1.0f;
        lerpTimer = 0.0f;

        //call lerp function in player
        player.GetComponent<PlayerController>().StartLerp(targetPosition, lerpDuration, arcCurve);

    }
}
