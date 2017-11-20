using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button startButton;
    public Button quitButton;
    public string levelName = "Greybox";

	// Use this for initialization
	void Start () {
        startButton.onClick.AddListener(ChangeScene);

        quitButton.onClick.AddListener(Quit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeScene()
    {
        Application.LoadLevel(levelName);
    }

    void Quit()
    {
        Application.Quit();
    }
}
