using UnityEngine;
using System.Collections;

public class wordOptionsScript : MonoBehaviour {
	public static string textMeshWord = "";
	
	GameObject textBox;
	TextBox textBoxScript;
	
	void Start(){
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
	}
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		if(gameObject.transform.name == "WordOption2"){
			textBoxScript.Swap();
			
			//Reloads notes pre-parsed			
			// for(int k = textBoxScript.allNoteLines[currAct].Length - 1; k >= 0; k--){
				// string noteName = gameControllerScript.noteArray[currAct][k].name;
				
				// GameObject.Find(noteName).transform.GetChild(0).GetComponent<CanvasScript>().Parser();
			// }
			
		}
		
		//Destroy the words from game world
		Destroy(GameObject.Find("WordOption1"));
		Destroy(GameObject.Find("WordOption2"));
	}
}
