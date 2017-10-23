using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Countdown_Timer : MonoBehaviour {

    public float minutes = 1;
    float seconds = 0;
    public bool gameOver;

    Text time;

	// Use this for initialization
	void Start ()
    {
        time = GetComponent<Text>();
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

        if (minutes > 0 || seconds > 0)
        {
            if (seconds < 10)
            {
                time.text = "Time" + "\n" + (int)minutes + ":0" + (int)seconds;
            }
            else
            {
                time.text = "Time" + "\n" + (int)minutes + ":" + (int)seconds;
            }
        }
        else
        {
            time.text = "Game Over";
            gameOver = true;
        }
    }

}
