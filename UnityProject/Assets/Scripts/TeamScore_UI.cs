using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TeamScore_UI : MonoBehaviour {

    public int team1Score;
    public int team2Score;
    public string captureObjectName = "Chems";
    Text score;

	// Use this for initialization
	void Start () {
        score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score.text = team1Score + "|" + team2Score;
	}
}
