using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

    public float timeOutOfCombat;
    public float timeBetweenPlayerHeals;
    public int healthGain;

    public Sprite FirstInventionSetImage;
    public Sprite SecondInventionSetImage;

    public Material trapPreviewMaterial;

    public enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }

	// Use this for initialization
	void Start ()
    {
        teamScore = FindObjectOfType<TeamScore_UI>();

        if (FindObjectOfType<Countdown_Timer>() == null)
        {
            Debug.Log("Missing Countdown_Timer script");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (projectileSpeed > speedCap)
        {
            projectileSpeed = speedCap;
        }

        if (FindObjectOfType<Countdown_Timer>() != null)
        {
            gameOverState = FindObjectOfType<Countdown_Timer>().gameOver;
        }

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
            //play game over sound
            if (FindObjectOfType<PrefabList>().GameOverAudio != null)
            {
                FindObjectOfType<PrefabList>().GameOverAudio.Play();
            }

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
