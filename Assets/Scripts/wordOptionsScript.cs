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
			canvasScript.quickFixNum = 0;
			//canvasScript.wordOptionClicked = true;
			
			textBoxScript.Swap();
			textBoxScript.editString = "";
			canvasScript.noteContent = "";
			
			//Spacing may or may not be a problem
			foreach(WordStructure wordStruct in textBoxScript.structList){
				if(wordStruct.current == textMeshWord){
					print("this is the wordID " + wordStruct.wordID);
				}
				//Rebuild {current|alt}
				//print("curr " + wordStruct.current);
				//print("alt " + wordStruct.alt);
				if(wordStruct.current != "N/A" && wordStruct.alt != "N/A" && wordStruct.dependencies == -1){
					textBoxScript.editString += "{" + wordStruct.current + "^" + wordStruct.wordWeightCurr + "|" + wordStruct.alt + "^" + wordStruct.wordWeightAlt + "}"; 
					if (wordStruct.newLine && !wordStruct.lastWord){
						//print("Working for {current|alt}");
						textBoxScript.editString += "\n";
					}
					else if (!wordStruct.newLine && wordStruct.lastWord){
						textBoxScript.editString += "";
					}
					else {
						textBoxScript.editString += " "; 
					}
					canvasScript.noteContent = textBoxScript.editString; 
				}
				//Rebuild *wordID*{current|alt}
				else if(wordStruct.current != "N/A" && wordStruct.alt != "N/A" && wordStruct.dependencies != -1){
					textBoxScript.editString += "*" + wordStruct.dependencies + "*{" + wordStruct.current + "|" + wordStruct.alt + "}";
					if (wordStruct.newLine){
						//print("Working for *wordID* words");
						textBoxScript.editString += "\n";
					}
					else if (!wordStruct.newLine && wordStruct.lastWord){
						textBoxScript.editString += "";
					}
					else{
						textBoxScript.editString += " ";
					}
					canvasScript.noteContent = textBoxScript.editString;
				}
				//Add reg word to string
				else if(wordStruct.current != "N/A" && wordStruct.alt == "N/A"){
					textBoxScript.editString += wordStruct.current;
					if (wordStruct.newLine){
						//print("Working for regular words");
						textBoxScript.editString += "\n";
					}
					else if (!wordStruct.newLine && wordStruct.lastWord){
						textBoxScript.editString += "";
					}
					else {
						textBoxScript.editString += " ";
					}
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
