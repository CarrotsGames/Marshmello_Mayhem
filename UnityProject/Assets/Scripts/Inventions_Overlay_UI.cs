using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventions_Overlay_UI : MonoBehaviour {

    public GameObject player;
    private BuildTrap playerTraps;
    public GameObject pitOverlay;
    public GameObject wallOverlay;
    public GameObject humanThrowerOverlay;
    public GameObject lastPrayerOverlay;

    private GameObject currentImage;
	// Use this for initialization
	void Start () {
        currentImage = lastPrayerOverlay;
	}
	
	// Update is called once per frame
	void Update () {
        playerTraps = player.GetComponent<BuildTrap>();
        
        if (playerTraps.isEnabled == true)
        {
            if (playerTraps.selectedTrap != null)
            {
                if (playerTraps.selectedTrap.GetComponent<PlaceableWall>() != null)
                {
                    currentImage.SetActive(false);
                    currentImage = wallOverlay;
                }
                if (playerTraps.selectedTrap.GetComponent<Pit>() != null)
                {
                    currentImage.SetActive(false);
                    currentImage = pitOverlay;
                }
                if (playerTraps.selectedTrap.GetComponent<Invention_007_LastPrayer>() != null)
                {
                    currentImage.SetActive(false);
                    currentImage = lastPrayerOverlay;
                }
                if (playerTraps.selectedTrap.GetComponent<HumanThrower>() != null)
                {
                    currentImage.SetActive(false);
                    currentImage = humanThrowerOverlay;
                }

                if (currentImage != null)
                {
                    currentImage.SetActive(true);
                }
            }
        }
        else
        {
            if (currentImage != null)
            {
                currentImage.SetActive(false);
            }
        }
    }
}
