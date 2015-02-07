// Super simple Main Menu. Add more stuff as we go along. Does enough to show off in class.

using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {
	void OnGUI(){
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		Rect buttonStart = new Rect(Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4) - (buttonHeight / 2), buttonWidth, buttonHeight);
		Rect buttonQuit = new Rect(Screen.width / 2 - (buttonWidth / 2), (2 *  Screen.height / 4) + buttonHeight, buttonWidth, buttonHeight);
		
		if(GUI.Button(buttonStart, "Start Demo")){
			Application.LoadLevel("noteClickZoom");
		}
		else if(GUI.Button(buttonQuit, "Quit Game")){
			Application.Quit();
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
