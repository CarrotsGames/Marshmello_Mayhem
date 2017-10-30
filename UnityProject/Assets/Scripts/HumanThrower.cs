using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanThrower : TrapBehaviour {

    public GameObject startTile;
    public GameObject endTile;
    public float speed;
    private float startTime;
    GameObject[] grid;
    public GameController.Direction direction;

    //test variables
    GameObject player;

    //slerping
    ///////////
    private bool isLerping = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float lerpTimer = 0.0f;
    private float lerpDuration = 1.5f;

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
    }
	
	// Update is called once per frame
	void Update () {


        //if(isLerping)
        //{
        //    lerpTimer += Time.deltaTime;

        //    //calculate value from 0 - 1 which is percentage of movement completed.
        //    float lerpValue = lerpTimer / lerpDuration;

        //    //lerp complete
        //    if (lerpValue >= 1.0f)
        //    {
        //        lerpValue = 1.0f;
        //        isLerping = false;
        //    }

        //    player.transform.position = Vector3.Lerp(startPosition, targetPosition, lerpValue) + Vector3.up * arcCurve.Evaluate(lerpValue);

        //}
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

    private void OnDrawGizmos()
    {

      //  Gizmos.DrawSphere(targetPosition, 1.0f);
    }
}
