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

    }

    private void OnTriggerEnter(Collider a_col)
    {
        if (a_col.GetComponent<PlayerController>() != null)
        {
            player = a_col.gameObject;

            Debug.Log("Player Launched");
        }

        Vector3 centre = (startTile.transform.position + endTile.transform.position) * 0.5f;

        centre -= new Vector3(0, 2, 0);

        Vector3 rise = startTile.transform.position - centre;
        Vector3 set = endTile.transform.position - centre;

        set = new Vector3(set.x, player.transform.position.y / 2, set.z);

        float fracComplete = (Time.time - startTime) / speed;

        a_col.GetComponent<PlayerController>().transform.position = Vector3.Slerp(rise, set, fracComplete);

        a_col.GetComponent<PlayerController>().transform.position += centre;
    }
}
