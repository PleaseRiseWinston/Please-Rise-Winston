using UnityEngine;
using System.Collections;

/*
* Essentially a "pause menu" right now but it does the job.
* Bind to gameObject(word). Make sure it has a box collider 2D component (needed for OnMouseDown() function apparently) 
* 
* Cont from noteZoom.cs
* Once timer from screenOverlay.cs reaches zero, it moves a piece of papel up the screen. Once it reaches the middle of the
* screen then it stops moving and becomes clickable.
*/

public class screenOverlay : MonoBehaviour {
	string wordClicked = "";
	string wordOption1 = "Word 1";
	string wordOption2 = "Word 2";
	public static bool stopAnimation = false;
	bool paused = false;
	bool objectClicked = false;
	public static bool onScreen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//print the word that's being tracked.
		//print (wordClicked);
		print("Stop animation: " + stopAnimation);
		print("MMX offScreen: " + noteZoom.offScreen);
		
		//Moves papel up.
		if(stopAnimation == false && noteZoom.offScreen == true){
			transform.Translate((4 + 1/2) * Vector3.up * Time.deltaTime, Space.World);
			objectClicked = true;
		}
		
		//Stops papel in middle of screen
		if(transform.position.y > 0){
			//print("hey");
			stopAnimation = true;
			objectClicked = false;
		}
		
		//Checking if the background was clicked. If true, then move the papel down.
		if(pleaseLeavePaper.clickedBG == true && onScreen == true){
			transform.Translate((4 + 1/2) * Vector3.down * Time.deltaTime, Space.World);
		}
	}

	void OnMouseDown(){
		if (objectClicked == false && stopAnimation == true) {
			paused = togglePause ();
			objectClicked = true;
		}
	}
	
	// Used to display word options
	void OnGUI(){
		const int buttonWidth = 120;
		const int buttonHeight = 60;

		if(paused){
			if(GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2), (1 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight),wordOption1)){
				paused = togglePause();
				objectClicked = false;
				wordClicked = wordOption1;
			}

			// Center in X, 2/3 of the height in Y
			if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2),(2 * Screen.height / 3) - (buttonHeight / 2),buttonWidth,buttonHeight),wordOption2)){
				paused = togglePause();
				objectClicked = false;
				wordClicked = wordOption2;
			}
		}
	}

	// Pauses the game
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
	
	// Once off screen set the bool to false.
	void OnBecameInvisible(){
		//print("Off screen");
		onScreen = false;
		noteZoom.moveSprite = true;
	}
}