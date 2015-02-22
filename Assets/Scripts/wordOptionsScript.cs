using UnityEngine;
using System.Collections;

public class wordOptionsScript : MonoBehaviour {
	public static string textMeshWord = "";
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		//Destroy the words from game world
		Destroy(GameObject.Find("word_option"));
		Destroy(gameObject);
		
		//Reset position coordinates
		screenOverlay.currX = -3.55f;
		screenOverlay.currY = -4.75f;
		screenOverlay.currZ = -3.5f;
		
		screenOverlay.wordOptionsUp = false;
		//print("You clicked " + textMeshWord);
	}
}
