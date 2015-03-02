// Super simple Main Menu. Add more stuff as we go along. Does enough to show off in class.

using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {
	public SceneFadeInOut other;
	GameObject screenFade;
	private bool sceneEnding = false;
	bool hasBeenPressed = false;
	
	void Awake(){
		screenFade = GameObject.Find("screenFader");
		other = screenFade.GetComponent<SceneFadeInOut>();
	}
	void Update(){
		if(sceneEnding){
			other.EndScene();
		}
	}
		
	void OnGUI(){
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		Rect buttonStart = new Rect(Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4) - (buttonHeight / 2), buttonWidth, buttonHeight);
		Rect buttonQuit = new Rect(Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight);
		
		if(!hasBeenPressed){
			if(GUI.Button(buttonStart, "Start Demo")){
				sceneEnding = true;
				hasBeenPressed = true;
			}
			else if(GUI.Button(buttonQuit, "Quit Game")){
				Application.Quit();
			}
		}
	}
}
