using UnityEngine;
using System.Collections;


// Essentially a "pause menu" right now but it does the job.
// Bind to gameObject(word). Make sure it has a box collider 2D component (needed for OnMouseDown() function apparently) 

public class screenOverlay : MonoBehaviour {
	bool paused = false;
	bool objectClicked = false;
	string wordClicked = "";
	string wordOption1 = "Word 1";
	string wordOption2 = "Word 2";
	bool stopAnimation = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//print the word that's being tracked.
		//print (wordClicked);
		
		if(noteZoom.timeLeft <= 0 && stopAnimation == false){
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
		}
		
		if(transform.position.y > 0){
			print("hey");
			stopAnimation = true;
		}
	}

	void OnMouseDown(){
		if (objectClicked == false) {
			paused = togglePause ();
			objectClicked = true;
		}
	}

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