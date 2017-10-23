using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanThrower : OffensiveTrap {

    private Vector3 direction;

    public Transform startTile;
    public Transform endTile;

    public float speed;

    private float startTime;

    GameObject[] grid;

    Quaternion rotation;

    //test variables
    GameObject player;

    // Use this for initialization
    void Start () {
        //Debug.Log(rotation.ToString());

        startTile = transform;

        grid = GameObject.FindGameObjectsWithTag("Floor");
        
        Vector3 pos = startTile.position;
        pos.x -= 3;

        for (int i = 0; i < grid.Length; i++)
        {
            Vector3 vecBetween = pos - grid[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= 1.2)
            {
                endTile = grid[i].transform;
            }
        }        
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 centre = (startTile.position + endTile.position) * 0.5f;

        centre -= new Vector3(0, 2, 0);

        Vector3 rise = startTile.position - centre;
        Vector3 set = endTile.position - centre;

        float fracComplete = (Time.time - startTime) / speed;

        if (player != null)
        {
            player.transform.position = Vector3.Slerp(rise, set, fracComplete);
            player.transform.position += centre;
        }
    }

    private void OnTriggerEnter(Collider a_col)
    {
        if (a_col.GetComponent<GameObject>() != null)
        {
            player = a_col.GetComponent<GameObject>();
        }
        //Vector3 centre = (startTile.position + endTile.position) * 0.5f;
        //
        //centre -= new Vector3(0, 2, 0);
        //
        //Vector3 rise = startTile.position - centre;
        //Vector3 set = endTile.position - centre;
        //
        //float fracComplete = (Time.time - startTime) / speed;
        //
        //a_col.GetComponent<PlayerController>().transform.position = Vector3.Slerp(rise, set, fracComplete);
        //a_col.GetComponent<PlayerController>().transform.position += centre;
    }

    public void SetTargetDirection(Quaternion a_rotation)
    {
        rotation = a_rotation;
    }
}
