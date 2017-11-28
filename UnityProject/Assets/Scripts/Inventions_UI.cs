using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventions_UI : MonoBehaviour {

    private Sprite first;
    private GameController gameController;
    private Image image;
    private GameObject player;
    public Image a;
    public Image b;

	// Use this for initialization
	void Start () {
        gameController = FindObjectOfType<GameController>();
        player = GetComponentInParent<Inventions_Overlay_UI>().player;
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        first = gameController.FirstInventionSetImage;

        if (player.GetComponent<BuildTrap>().extraTraps == false)
        {
            image.sprite = first;
        }
        if (player.GetComponent<BuildTrap>().isEnabled == true)
        {
            b.enabled = true;
        }
        else
        {
            b.enabled = false;
        }
	}
}
