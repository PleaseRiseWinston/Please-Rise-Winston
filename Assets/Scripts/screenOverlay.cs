/* 
* NEW Screen Overlay V2.2 - Now with centered text!
* Displays the word options with a center alignment. Also fixed the problem with
* position. Using Camera.main.ViewportToWorldPoint. Should work with all types of
* screens.
*
* Screen Overlay V2.1 - Now without GUI Buttons!
* Displays the possible word options when object is clicked. Destroys newly 
* instantiated word options when it is clicked.
*/

using UnityEngine;
using System.Collections;

public class screenOverlay : MonoBehaviour {
	public static string[] wordOptions = {"Word 1", "Mississipi River"};
	public static bool stopAnimation = false;
	bool paperClicked = false;
	public static bool onScreen = false;
	public static bool wordOptionsUp = false;
	
	//Changed from TextMesh
	public GameObject textPrefab;
	
	//Instantiated word prefabs use Camera.main
	// NOT ARBITRARY POINTS
	public static float currX = .5f;
	public static float currY = .75f;
	public static float currZ = .5f;

	void Update () {
		//Print word that's clicked
		//print(wordOptionsScript.textMeshWord);
		
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
		if(pleaseLeavePaper.clickedBG == true && onScreen == true && wordOptionsUp == false){
			transform.Translate((4 + 1/2) * Vector3.down * Time.deltaTime, Space.World);
			paperClicked = false;
		}
		
		//print(paperClicked);
	}

	void OnMouseDown(){
        //Debug.Log("Clicked");
		if (paperClicked == false && stopAnimation == true && wordOptionsUp == false) {
			paperClicked = true;
			createText();
		}
	}
	
	// Creates the word options and puts them on screen.
	void createText(){
		wordOptionsUp = true;
		
		foreach(string w in wordOptions){
			GameObject textInstance;
			textInstance = Instantiate(textPrefab, Camera.main.ViewportToWorldPoint(new Vector3(currX,currY,currZ)), Quaternion.identity) as GameObject;
			textInstance.name = w;
			textInstance.transform.parent = GameObject.Find("Canvas").transform;
			textInstance.GetComponent<TextMesh>().text = w;
			textInstance.AddComponent<BoxCollider2D>();
			
			currX = textInstance.transform.position.x;
			currY = textInstance.transform.position.y;
			currZ = textInstance.transform.position.z;
			float textSize = textInstance.GetComponent<BoxCollider2D>().size.x;
			float newPosX = textInstance.transform.position.x - (textSize / 2);
			textInstance.transform.position = new Vector3(newPosX, currY, currZ);
			
			currX = .5f;
			currY = .25f;
			currZ = .5f;
		}
	}
	
	// Once off screen set the onScreen bool to false.
	void OnBecameInvisible(){
		//print("Off screen");
		onScreen = false;
		noteZoom.moveSprite = true;
	}
}