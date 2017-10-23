using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter_UI : MonoBehaviour {

    public GameObject scientist;
    public GameObject assistant;

    ResourceController top;
    ResourceController bottom;

    Text text;

	// Use this for initialization
	void Start ()
    {
        top = scientist.GetComponent<ResourceController>();
        bottom = assistant.GetComponent<ResourceController>();

        if (top == null)
        {
            Debug.Log("Missing ResourceController on top player");
        }

        if (bottom == null)
        {
            Debug.Log("Missing ResourceController on bottom player");
        }

        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (top != null && bottom != null)
        {
            text.text = top.currentResource + "\n" + "---" + "\n" + bottom.currentResource;
        }
	}
}
