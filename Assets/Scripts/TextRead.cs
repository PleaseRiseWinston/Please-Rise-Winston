using UnityEngine;
using System.Collections;

public class TextRead : MonoBehaviour {

	public TextAsset inputFile;

	void Start(){
		print (inputFile.text);
		//string testText = inputFile.text;
		//GetComponent<TextMesh> ().text = "Hello World.";
		GetComponent<TextMesh> ().text = inputFile.text;
	}

	void Update(){
		//GetComponent<TextMesh> ().text = inputFile;
	}
}
