using UnityEngine;
using System.Collections;

public class wordOptionsScript : MonoBehaviour {
	public static string textMeshWord = "";
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		//Destroy the words from game world
		foreach(string w in screenOverlay.wordOptions){
			Destroy(GameObject.Find(w));
		}
		
		//Reset position coordinates
		screenOverlay.currX = .5f;
		screenOverlay.currY = .75f;
		screenOverlay.currZ = .5f;
		
		screenOverlay.wordOptionsUp = false;
		//print("You clicked " + textMeshWord);
	}
}
