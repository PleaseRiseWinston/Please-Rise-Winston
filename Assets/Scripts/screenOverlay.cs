using UnityEngine;
using System.Collections;


// Will clean up at some point. Essentially a "pause menu" right now.
// Bind to gameObject(word). Make sure it has a box collider 2D component (needed for OnMouseDown() function apparently) 

public class screenOverlay : MonoBehaviour {
	//bool isDestroyed = false;
	bool paused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown(){
		paused = togglePause ();
	}

	void OnGUI(){
		const int buttonWidth = 120;
		const int buttonHeight = 60;

		if(paused){
		if(GUI.Button(
			new Rect(
				Screen.width / 2 - (buttonWidth / 2), 
				(1 * Screen.height / 3) - (buttonHeight / 2), 
				buttonWidth, buttonHeight),
			"Word 1")){
			print ("Hey");
		}

		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Word 2"
			)
			)
		{
			print ("Listen");
			}
		}
		
		
		/*if(paused){
			GUILayout.Label("Game is paused!");
			if(GUILayout.Button("Click me to unpause"))
				paused = togglePause();
		}*/
	}

	bool togglePause(){
		if (Time.timeScale == 0f) {
			Time.timeScale = 1f;
			return(false);
		}
		else{
			Time.timeScale = 0f;
			return(true);
		}
	}
}