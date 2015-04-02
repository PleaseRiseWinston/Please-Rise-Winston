using UnityEngine;
using System.Collections;

public class wordOptionsScript : MonoBehaviour {
	public static string textMeshWord = "";
	
	GameObject canvas;
	CanvasScript canvasScript;
	
	GameObject paper;
	PaperScript paperScript;
	
	GameObject line;
	LineScript lineScript;
	
	GameObject textBox;
	TextBox textBoxScript;
	
	void Start(){
		paper = GameObject.Find("GamePaper");
		paperScript = paper.GetComponent<PaperScript>();
		
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
	}
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		if(gameObject.transform.name == "WordOption2"){
			lineScript.quickFixNum = 0;
			//canvasScript.wordOptionClicked = true;
			
			textBoxScript.Swap();
			textBoxScript.editString = "";
			canvasScript.noteContent = "";
			
			//Spacing may or may not be a problem
			foreach(WordStructure wordStruct in textBoxScript.structList){
				//Rebuild {current|alt}
				if(wordStruct.current != "N/A" && wordStruct.alt != "N/A" && wordStruct.dependencies == null){
					textBoxScript.editString += "{" + wordStruct.current + "|" + wordStruct.alt + "} "; 
					canvasScript.noteContent = textBoxScript.editString; 
				}
				//Rebuild *wordID*{current|alt}
				else if(wordStruct.current != "N/A" && wordStruct.alt != "N/A" && wordStruct.dependencies != null){
					textBoxScript.editString += "*" + wordStruct.dependencies[0] + "*{" + wordStruct.current + "|" + wordStruct.alt + "} ";
					canvasScript.noteContent = textBoxScript.editString;
				}
				//Add reg word to string
				else if(wordStruct.current != "N/A" && wordStruct.alt == "N/A"){
					textBoxScript.editString += wordStruct.current + " ";
					canvasScript.noteContent = textBoxScript.editString;
				}
			}
			textBoxScript.structList.Clear();
			canvasScript.displayWords.Clear();
			//lineScript.words = null;
			textBoxScript.wordStructCount = 0;
			
			foreach(string s in canvasScript.lineIDList){
				print("destroying");
				Destroy(GameObject.Find(s));
			}
			canvasScript.curSpacing = 10;
			canvasScript.linePosCount = 0;
			
			//print(canvasScript.noteContent);
			canvasScript.Parser();
			//canvasScript.CreateNewLine();
			//print(canvasScript.noteContent);
		}
		
		//Destroy the words from game world
		Destroy(GameObject.Find("WordOption1"));
		Destroy(GameObject.Find("WordOption2"));
		
		//Reset position coordinates
		// screenOverlay.currX = .5f;
		// screenOverlay.currY = .75f;
		// screenOverlay.currZ = .5f;
		
		// screenOverlay.wordOptionsUp = false;
		//print("You clicked " + textMeshWord);
	}
	
	void Update(){
		canvas = GameObject.Find("GameCanvas");
		canvasScript = canvas.GetComponent<CanvasScript>();
		
		line = GameObject.Find("Line0");
		lineScript = line.GetComponent<LineScript>();
	}
}
