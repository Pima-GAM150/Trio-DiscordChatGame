using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

    public Canvas canvas;

    private bool escapedPressed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            escapedPressed = !escapedPressed;
            canvas.gameObject.SetActive(escapedPressed);
            //Debug.Log("Pressed Escape! " + escapedPressed);
        }
	}

    public void ClickedStart()
    {
        Debug.Log("Clicked Start!");
    }
}
