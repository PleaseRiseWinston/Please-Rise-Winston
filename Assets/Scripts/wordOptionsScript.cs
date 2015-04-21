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
	
	GameObject gameController;
	GameController gameControllerScript;
	
	// int startStringIndex = 0;
	// int endStringIndex = -1;
	int currAct = 0;
	
	void Start(){
		paper = GameObject.Find("1.1");
		paperScript = paper.GetComponent<PaperScript>();
		
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
		
		gameController = GameObject.FindGameObjectWithTag("GameController");
		gameControllerScript = gameController.GetComponent<GameController>();
	}
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		currAct = gameControllerScript.curAct - 1;
		
		if(gameObject.transform.name == "WordOption2"){
			int startStringIndex = 0;
			int endStringIndex = -1;
			textBoxScript.quickFixNum = 0;
			//canvasScript.wordOptionClicked = true;
			
			textBoxScript.Swap();
			paperScript.noteContent = "";
			textBoxScript.editString = "";
			canvasScript.noteContent = "";
			textBoxScript.wordStructCount = 0;
			canvasScript.wordNum = 0;
			
			//print(textBoxScript.noteWordCount[currAct].Length);
			
			for(int i = 0; i <= textBoxScript.noteWordCount[currAct].Length - 1; i++){
				if(i > 0){
					startStringIndex = endStringIndex + 1;
				}
				
				//print(textBoxScript.noteWordCount[currAct][i]);
				endStringIndex += textBoxScript.noteWordCount[currAct][i];
				
				for(int j = startStringIndex; j <= endStringIndex; j++){
					//Rebuild {current|alt}
					if(textBoxScript.structList[j].current != "N/A" && textBoxScript.structList[j].alt != "N/A" && textBoxScript.structList[j].dependencies == -1){
						textBoxScript.editString += "{" + textBoxScript.structList[j].current + "^" + textBoxScript.structList[j].wordWeightCurr + "|" + textBoxScript.structList[j].alt + "^" + textBoxScript.structList[j].wordWeightAlt + "}"; 
						if (textBoxScript.structList[j].newLine && !textBoxScript.structList[j].lastWord){
							//print("Working for {current|alt}");
							textBoxScript.editString += "\n";
						}
						else if (!textBoxScript.structList[j].newLine && textBoxScript.structList[j].lastWord){
							textBoxScript.editString += "";
						}
						else {
							textBoxScript.editString += " "; 
						}
						textBoxScript.allNoteLines[currAct][i] = textBoxScript.editString; 
					}
					//Rebuild *wordID*{current|alt}
					else if(textBoxScript.structList[j].current != "N/A" && textBoxScript.structList[j].alt != "N/A" && textBoxScript.structList[j].dependencies != -1){
						textBoxScript.editString += "*" + textBoxScript.structList[j].dependencies + "*{" + textBoxScript.structList[j].current + "|" + textBoxScript.structList[j].alt + "}";
						if (textBoxScript.structList[j].newLine){
							//print("Working for *wordID* words");
							textBoxScript.editString += "\n";
						}
						else if (!textBoxScript.structList[j].newLine && textBoxScript.structList[j].lastWord){
							textBoxScript.editString += "";
						}
						else{
							textBoxScript.editString += " ";
						}
						textBoxScript.allNoteLines[currAct][i] = textBoxScript.editString;
					}
					//Add reg word to string
					else if(textBoxScript.structList[j].current != "N/A" && textBoxScript.structList[j].alt == "N/A"){
						textBoxScript.editString += textBoxScript.structList[j].current;
						if (textBoxScript.structList[j].newLine){
							//print("Working for regular words");
							textBoxScript.editString += "\n";
						}
						else if (!textBoxScript.structList[j].newLine && textBoxScript.structList[j].lastWord){
							textBoxScript.editString += "";
						}
						else {
							textBoxScript.editString += " ";
						}
						textBoxScript.allNoteLines[currAct][i] = textBoxScript.editString;
					}
				}
				textBoxScript.editString = "";
			}
			
			// print(textBoxScript.allNoteLines[currAct][0]);
			//print(textBoxScript.allNoteLines[currAct][1]);
			// print(textBoxScript.allNoteLines[currAct][2]);
			
			textBoxScript.structList.Clear();
			canvasScript.displayWords.Clear();
			//lineScript.words = null;		
			
			canvasScript.curSpacing = 10;
			
			foreach(GameObject currLine in GameObject.FindGameObjectsWithTag("Line")){
				Destroy(currLine);
			}
			
			//Reloads notes pre-parsed.
			for(int k = textBoxScript.allNoteLines[currAct].Length - 1; k >= 0; k--){
				string noteName = gameControllerScript.noteArray[currAct][k].name;
				//print(noteName);
				
				GameObject.Find(noteName).transform.GetChild(0).GetComponent<CanvasScript>().Parser();
			}
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
		
		// line = GameObject.Find("Line0");
		// lineScript = line.GetComponent<LineScript>();
	}
}
