using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Timer : MonoBehaviour {

    public float minutes = 10;
    float seconds = 0;
    public bool gameOver;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        seconds -= Time.deltaTime;

        if (seconds < 0)
        {
            if (minutes > 0)
            {
                minutes--;
                seconds = 60;
            }
            
        }
	}

    //uses simple GUI rectangle to display time
    //(only need if/else statements from here)
    private void OnGUI()
    {
        if (minutes > 0 || seconds > 0)
        {
            if (seconds < 10)
            {
                GUI.Label(new Rect(100, 100, 200, 100), "Time Remaining : " + (int)minutes + ":0" + (int)seconds);
            }
            else
            {
                GUI.Label(new Rect(100, 100, 200, 100), "Time Remaining : " + (int)minutes + ":" + (int)seconds);
            }
        }
        else
        {
            GUI.Label(new Rect(100, 100, 200, 100), "Game Over");
            gameOver = true;
        }
    }
}
