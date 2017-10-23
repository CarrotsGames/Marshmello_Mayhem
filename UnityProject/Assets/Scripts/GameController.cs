using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public TeamScore_UI teamScore;
    public int winScore = 3;
    private Countdown_Timer timer;

	// Use this for initialization
	void Start ()
    { 

	}
	
	// Update is called once per frame
	void Update ()
    {
        //if a team reaches winScore
		if (teamScore.team1Score == winScore)
        {
            Team1Victory();
        }
        if (teamScore.team2Score == winScore)
        {
            Team2Victory();
        }

        //if timer hits zero
        if (timer.gameOver == true)
        {
            if (teamScore.team1Score > teamScore.team2Score)
            {
                Team1Victory();
            }
            if (teamScore.team2Score > teamScore.team1Score)
            {
                Team2Victory();
            }
            if (teamScore.team1Score == teamScore.team2Score)
            {
                Tie();
            }
        }
	}

    void Team1Victory()
    {

    }

    void Team2Victory()
    {

    }

    void Tie()
    {

    }
}
