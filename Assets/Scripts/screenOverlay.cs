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
	string[] wordOptions = {"Word 1", "Mississipi River"};
	public static bool stopAnimation = false;
	//bool paused = false;
	bool paperClicked = false;
	public static bool onScreen = false;
	
	//Changed from TextMesh
	public GameObject textPrefab;
	//public Transform putWordsHere;
	
	//Some placement numbers. Screen.width/2 wasn't working.
	float currX = -3.55f;
	float currY = -4.75f;
	float currZ = -3.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Moves papel up.
		if(stopAnimation == false && noteZoom.offScreen == true){
			transform.Translate((4 + 1/2) * Vector3.up * Time.deltaTime, Space.World);
			paperClicked = true;
		}
		
		//Stops papel in middle of screen
		if(transform.position.y > 0){
			//print("hey");
			stopAnimation = true;
			paperClicked = false;
		}
		
		//Checking if the background was clicked. If true, then move the papel down.
		if(pleaseLeavePaper.clickedBG == true && onScreen == true){
			transform.Translate((4 + 1/2) * Vector3.down * Time.deltaTime, Space.World);
			Destroy(GameObject.Find("word_option"));
			paperClicked = false;
			
			//reset positions once destroyed
			currX = -3.55f;
			currY = -4.75f;
			currZ = -3.5f;
		}
		
		//print(paperClicked);
	}

	void OnMouseDown(){
        //Debug.Log("Clicked");
		if (paperClicked == false && stopAnimation == true) {
			//paused = togglePause();
			paperClicked = true;
			createText();
		}
	}
	
	//NEW - Goes through array and creates a new GameObject for each word in array, and places 
	//      the new object on screen. Only runs when the word is clicked. See OnMouseDown. Screen.width / 2, currWordY, 0
	//TODO: Create GameObject instead of TextMesh. AddComponent<Script>() and then do magic in new script.
	void createText(){
		foreach(string w in wordOptions){
			GameObject textInstance;
			textInstance = Instantiate(textPrefab, new Vector3(currX, currY, currZ), Quaternion.identity) as GameObject;
			textInstance.name = "word_option";
			textInstance.transform.parent = GameObject.Find("Canvas").transform;
			textInstance.GetComponent<TextMesh>().text = w;
			textInstance.AddComponent<BoxCollider2D>();
			//textInstance.text = w;
			
			currX = -3.55f;
			currY = 6;
			currZ = -3.5f;
		}
	}
	
	// Used to display word options
	/*void OnGUI(){
		const int buttonWidth = 120;
		const int buttonHeight = 60;

		if(paused){
			if(GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2), (1 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight),wordOption1)){
				paused = togglePause();
				paperClicked = false;
				wordClicked = wordOption1;
			}

			// Center in X, 2/3 of the height in Y
			if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2),(2 * Screen.height / 3) - (buttonHeight / 2),buttonWidth,buttonHeight),wordOption2)){
				paused = togglePause();
				paperClicked = false;
				wordClicked = wordOption2;
			}
		}
	}*/

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
	
	// Once off screen set the onScreen bool to false.
	void OnBecameInvisible(){
		//print("Off screen");
		onScreen = false;
		noteZoom.moveSprite = true;
	}
}