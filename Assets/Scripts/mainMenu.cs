// Super simple Main Menu. Add more stuff as we go along. Does enough to show off in class.

using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {
	void OnGUI(){
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		Rect buttonRect = new Rect(Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight);
		
		if(GUI.Button(buttonRect, "Start Demo")){
			Application.LoadLevel("noteClickZoom");
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
