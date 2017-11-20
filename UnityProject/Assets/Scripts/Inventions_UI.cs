﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventions_UI : MonoBehaviour {

    private Sprite first;
    private GameController gameController;
    private Image image;
    public GameObject player;

	// Use this for initialization
	void Start () {
        gameController = FindObjectOfType<GameController>();

        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        first = gameController.FirstInventionSetImage;

        if (player.GetComponent<BuildTrap>().extraTraps == false)
        {
            image.sprite = first;
        }
	}
}
