using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStartButton : MonoBehaviour {

    public string levelName = "Greybox";
    Button startButton;

	// Use this for initialization
	void Start () {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(ChangeScene);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void ChangeScene()
    {
        Application.LoadLevel(levelName);
    }
}
