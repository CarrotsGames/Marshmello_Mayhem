using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private TeamScore_UI teamScore;
    public int winScore = 3;
    public float projectileTimer;
    public int gunDamage;
    bool gameOverState;
    public float projectileCooldown;
    public float projectilePushback;
    public float projectileSpeed = 50.0f;
    private float speedCap = 120.0f;
    public List<GameObject> placedTraps;

	// Use this for initialization
	void Start ()
    {
        teamScore = FindObjectOfType<TeamScore_UI>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (projectileSpeed > speedCap)
        {
            projectileSpeed = speedCap;
        }

        gameOverState = FindObjectOfType<Countdown_Timer>().gameOver;

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
        if (gameOverState == true)
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
