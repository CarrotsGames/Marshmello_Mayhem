using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;


public class GameController : MonoBehaviour {

    XboxController controller;

    public float shootingCost;
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

    public GameObject blueTeamWinDisplay;
    public GameObject redTeamWinDisplay;
    public GameObject tieDisplay;

    public float timeBeforeReturnToMainMenu;

    bool returningToMenu = false;

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

        //add pre-existing walls to placedTraps list
        PlaceableWall[] walls = FindObjectsOfType<PlaceableWall>();
        for (int i = 0; i < walls.Length; i++)
        {
            placedTraps.Add(walls[i].gameObject);
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

        if (returningToMenu == true)
        {
            //play game over sound
            if (FindObjectOfType<PrefabList>().GameOverAudio != null)
            {
                FindObjectOfType<PrefabList>().GameOverAudio.Play();
            }

            timeBeforeReturnToMainMenu -= Time.deltaTime;
            
            if (timeBeforeReturnToMainMenu <= 0 || XCI.GetButtonDown(XboxButton.A, XboxController.All))
            {
                Application.LoadLevel("Title Screen");
            }
        }
	}

    //show UI for which team won
    //return to main menu on button press

    void Team1Victory()
    {
        blueTeamWinDisplay.SetActive(true);
        ReturnToMenu();
    }

    void Team2Victory()
    {
        redTeamWinDisplay.SetActive(true);
        ReturnToMenu();
    }

    void Tie()
    {
        tieDisplay.SetActive(true);
        ReturnToMenu();
    }

    void ReturnToMenu()
    {
        returningToMenu = true;
    }
}
