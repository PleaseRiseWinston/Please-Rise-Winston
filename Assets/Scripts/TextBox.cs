using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class TextBox : MonoBehaviour {
    public GameObject empty;
    public GameObject structHolder;
    public WordStructure wordStructure; 
	
	private char delimiterNewline = '\n';
	private char delimiterSpace = ' ';
	private Regex re = new Regex(@"(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})|(\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\{[A-Za-z]+\|[A-Za-z]+\})|([A-Za-z]+'[a-z]+)([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z]+)");
	private Regex braceRe = new Regex(@"\*([0-9]+)\*\{([A-Za-z]+)\|([A-Za-z]+)\}|\{([A-Za-z]+)\^([0-9])\|([A-Za-z]+)\^([0-9])\}"); //Looks for {word|alt}
	
	//public List<string> wordCountListTemp = new List<string>();
	public string[] words;
	public string[] lines;
	
	DirectoryInfo info;
	public string fileName = "//winstonNote";
	public int fileNum = 0;
	public string fileExt = ".prw";
	public int count = 0;
	public List<string> arrText;
	public string[] fileLoadWords;
	public int actNumber = 1;
	public int currDirFileCount = 0;

    [TextArea(3,10)] public string editString = "";
	public string currDir;
	public string[] fileEntries;
	public int wordStructCount = 0;
	public GameObject canvas;
	public CanvasScript canvasScript;
	
	public List<WordStructure> structList = new List<WordStructure>();
	public List<WordStructure> swapWordList = new List<WordStructure>();
	
	public int structListIndex = 0;
	public string clickedWordID = "";
	public string[][] allNoteLines;
	public int[][] noteWordCount;
	public bool didSwap = false;
	
	public int quickFixNum = 0;
	
	public GameObject notes;
	public GameObject gameController;
	public GameController gameControllerScript;

	void Start(){
		info = new DirectoryInfo(Application.dataPath);
		currDir = info.ToString();					       //makes directory into string
		fileEntries = Directory.GetFiles(currDir);  //gets files in current directory
		allNoteLines = new string[5][];
		noteWordCount = new int[5][];
		
		notes = GameObject.FindGameObjectWithTag("Notes").gameObject;
		gameController = GameObject.FindGameObjectWithTag("GameController").gameObject;
		gameControllerScript = gameController.GetComponent<GameController>();
		
		for(int i = 0; i < notes.transform.childCount; i++){
			//print(notes.transform.childCount);
			allNoteLines[i] = new string[notes.transform.GetChild(i).childCount];
			noteWordCount[i] = new int[notes.transform.GetChild(i).childCount];
			for(int j = 0; j < notes.transform.GetChild(i).childCount; j++){
				loadFile();
				//print(notes.transform.GetChild(i).childCount);
				allNoteLines[i][j] = editString;
			}
			count = 0;
			actNumber++;
		}
		editString = allNoteLines[0][0];
		
		//print(allNoteLines[0][0]);
	}
	
	void Update(){
		canvas = GameObject.Find("GameCanvas");
		canvasScript = canvas.GetComponent<CanvasScript>();
	}
	/*
  	void OnGUI() {
		const int buttonWidth = 84;
		const int buttonHeight = 50;

		Rect buttonParse = new Rect (0, 0, buttonWidth, buttonHeight);
		Rect buttonSave = new Rect(buttonWidth + 10, 0, buttonWidth, buttonHeight);
		Rect buttonDump = new Rect(buttonWidth * 2 + 20, 0, buttonWidth, buttonHeight);
		Rect buttonLoad = new Rect(buttonWidth * 3 + 30, 0, buttonWidth, buttonHeight);
		Rect buttonReset = new Rect(buttonWidth * 4 + 40, 0, buttonWidth, buttonHeight);
		Rect buttonSwap = new Rect(buttonWidth * 5 + 50, 0, buttonWidth, buttonHeight);
		Rect buttonDisplay2Con = new Rect(buttonWidth * 6 + 60,0,buttonWidth,buttonHeight);
		
		editString = GUI.TextArea (new Rect (0, 50, 200, 200), editString, 500);

		if (GUI.Button(buttonParse, "Parse")){
			canvasScript.Parser();
			// Parser();
		}
		else if(GUI.Button(buttonSave, "Save")){
			checkFileNum(fileName + fileNum + fileExt);
			StreamWriter writer = new StreamWriter(info + fileName + fileNum + fileExt);
			writer.WriteLine(editString);
			writer.Close();
			fileNum++;
		}
		else if(GUI.Button(buttonDump, "Dump")){
			editString = "";
		}
		else if(GUI.Button(buttonLoad, "Load")){
			loadFile();
		}
		else if(GUI.Button(buttonReset, "Reset")){
			count = 0;
		}
		else if(GUI.Button(buttonSwap, "Swap")){
			Swap();
			// print(structList[structListIndex].current + " " + structList[structListIndex].alt);
			// foreach(WordStructure wordS in structList){
				// print(wordS.current);
			// }
		}
		else if(GUI.Button(buttonDisplay2Con, "Print")){
			print(Resources.Load("Act1/Test"));
			// foreach(WordStructure wordStruct in structList){
				  // print(wordStruct.current);
			 // }
			// print(allNoteLines[0][0]);
			// print(allNoteLines[0][1]);
			
			//print(swapWordList.Count);
		}
	} 
	*/
	public void loadFile(){
		arrText = new List<string>();
		StreamReader objReader = new StreamReader(info + "/Resources/Act" + actNumber + "/" + fileName + count + fileExt);
		//print(info+fileName+count);
		//print("File Num" + count);
		string sLine = "";
		
		while (sLine != null){
			sLine = objReader.ReadLine();
			if(sLine != null){
				arrText.Add(sLine);
			}
		}
		objReader.Close();
		fileLoadWords = arrText.ToArray();
		editString = string.Join("\n", fileLoadWords);
		count++;
	}
	
	void checkFileNum(string currFile){
		string testDir = currDir + currFile; 				//makes a full path string with current file at the end
		foreach(string nameFile in fileEntries){			//for loop to check current file with all files in dir
			if(testDir == nameFile){
				fileNum++;
				checkFileNum(fileName + fileNum + fileExt);
			}
		}
	}
	
	public void Swap(){
		int currAct = gameControllerScript.curAct - 1;
		int clickedWordCurrNote = 0;
		int currActTotalWords = 0;
		int IDofClickedWord = 0;
		
		foreach(WordStructure wordStruct in swapWordList){
			if(clickedWordID == "wordID" + wordStruct.wordID && wordStruct.alt != "N/A" && wordStruct.isClicked){
				IDofClickedWord = wordStruct.wordID;
				clickedWordCurrNote = wordStruct.noteID;
				
				string tempString = wordStruct.current;
				wordStruct.current = wordStruct.alt;
				wordStruct.alt = tempString;
				
				int tempNum = wordStruct.wordWeightCurr;
				wordStruct.wordWeightCurr = wordStruct.wordWeightAlt;
				wordStruct.wordWeightAlt = tempNum;
				
				updatePaper(wordStruct.lineID, currAct + 1, wordStruct.noteID, wordStruct.wordID, wordStruct.current, false);
				
				wordStruct.isClicked = false;
			}
			else if(IDofClickedWord == wordStruct.dependencies && clickedWordCurrNote == wordStruct.noteID){
				string tempString = wordStruct.current;
				wordStruct.current = wordStruct.alt;
				wordStruct.alt = tempString;
				
				updatePaper(wordStruct.lineID, currAct + 1, wordStruct.noteID, wordStruct.wordID, wordStruct.current, true);
			}
		}
		
		didSwap = true;
	}
	
	public void updatePaper(string lineID, int currentAct, int currentNote, int currentWordID, string currentWordText, bool changeable){
		string actPointNote = currentAct + "." + currentNote;
		
		//print(actPointNote);
		foreach(GameObject noteObj in GameObject.FindGameObjectsWithTag("Papers")){
			if(noteObj.name == actPointNote){
				GameObject wordToSwap = noteObj.transform.Find("GameCanvas/" + lineID + "/wordID" + currentWordID).gameObject;
				wordToSwap.GetComponent<Text>().text = currentWordText;
				wordToSwap.GetComponent<WordScript>().curText = currentWordText;
			    if (changeable)
			    {
                    Debug.Log("BOOM!");
                    GameObject switchLine = noteObj.transform.Find("GameCanvas/" + lineID).gameObject;
			        StartCoroutine(switchLine.GetComponent<LineScript>().untranslate());
			    }
			}
		}
	}
}