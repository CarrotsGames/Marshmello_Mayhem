using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_006_AntiStickMatter : TrapBehaviour
{

    private GameObject player;
    public int speedBoost;

    public float duration;
    bool isActive = false;

    float originalSpeed;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (isActive == true)
        {  
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);

            player.GetComponent<PlayerController>().speed = originalSpeed + speedBoost;

            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                player.GetComponent<PlayerController>().speed = originalSpeed;
                isActive = false;
            }

            if (isActive == false)
            {
                Debug.Log("Anti stick matter wore off");
                Destroy(gameObject);
            }
        }        
	}

    public void SetPlayer(GameObject a_player)
    {
        player = a_player;

        if (player != null)
        {
            Debug.Log("Anti stick matter applied");
            originalSpeed = player.GetComponent<PlayerController>().speed;
            isActive = true;
        }
    }
}
