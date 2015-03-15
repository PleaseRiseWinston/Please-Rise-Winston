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
	private Regex braceRe = new Regex(@"\*([0-9]+)\*\{([A-Za-z]+)\|([A-Za-z]+)\}|\{([A-Za-z]+)\|([A-Za-z]+)\}"); //Looks for {word|alt}
	
	public List<string> wordList = new List<string>();
	public List<int> dependenciesList = new List<int>();
	public string[] words;
	public string[] lines;
	
	DirectoryInfo info;
	string fileName = "\\winstonNote";
	int fileNum = 0;
	string fileExt = ".prw";
	int count = 0;
	public List<string> arrText;
	string[] fileLoadWords;

	public static string editString = "edit me {word|alt}";
	string currDir;
	string[] fileEntries;
	int wordStructCount = 0;
	public GameObject canvas;
	public CanvasScript canvasScript;
	public List<WordStructure> structList = new List<WordStructure>();
	int structListIndex = 0;

	void Start(){
		info = new DirectoryInfo(Application.dataPath);
		currDir = info.ToString();					//makes directory into string
		fileEntries = Directory.GetFiles(currDir);  //gets files in current directory
	}
	
	void Update(){
		canvas = GameObject.Find("Canvas(Clone)");
		canvasScript = canvas.GetComponent<CanvasScript>();
	}
	
	void OnGUI() {
		const int buttonWidth = 84;
		const int buttonHeight = 50;

		Rect buttonParse = new Rect (0, 0, buttonWidth, buttonHeight);
		Rect buttonSave = new Rect(buttonWidth + 10, 0, buttonWidth, buttonHeight);
		Rect buttonDump = new Rect(buttonWidth * 2 + 20, 0, buttonWidth, buttonHeight);
		Rect buttonLoad = new Rect(buttonWidth * 3 + 30, 0, buttonWidth, buttonHeight);
		Rect buttonReset = new Rect(buttonWidth * 4 + 40, 0, buttonWidth, buttonHeight);
		Rect buttonSwap = new Rect(buttonWidth * 5 + 50, 0, buttonWidth, buttonHeight);
		
		editString = GUI.TextArea (new Rect (0, 50, 200, 200), editString, 500);

		if (GUI.Button(buttonParse, "Parse")){
			//canvasScript.Parser();
			Parser();
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
			print(structList[structListIndex].current + " " + structList[structListIndex].alt);
		}

	}
	
	void loadFile(){
		arrText = new List<string>();
		StreamReader objReader = new StreamReader(info + fileName + count + fileExt);
		print("File Num" + count);
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

	void Parser () {
		lines = editString.Split(delimiterNewline);	
		foreach (string s in lines)
		{			
			words = s.Split(delimiterSpace);
			
			// Clears the list if there is any content to make room for new line
			wordList.Clear();
			
			// Each word entry is parsed via regex
			foreach (string word in words)
			{
				Match result = re.Match(word);
				
				if (result.Success)
				{						
					// Parse *wordID*{word|alt} with and without punctuation
					if (result.Groups[1].Value != "" && result.Groups[2].Value != ""){
						wordList.Add(result.Groups[1].Value);
						wordList.Add(result.Groups[2].Value + " ");
					}
					else if (result.Groups[3].Value != ""){
						wordList.Add(result.Groups[3].Value + " ");
					}
					//Parse {word|alt} with and without punctuation
					else if (result.Groups[4].Value != "" && result.Groups[5].Value != "")
					{
						wordList.Add(result.Groups[4].Value);
						wordList.Add(result.Groups[5].Value + " ");
					}
					else if (result.Groups[6].Value != ""){
						wordList.Add(result.Groups[6].Value + " ");
					}
					// Parse conjunction + punctuation
					else if (result.Groups[7].Value != "" && result.Groups[8].Value != ""){
						wordList.Add(result.Groups[7].Value);
						wordList.Add(result.Groups[8].Value + " ");
					}
					// Parse normal word + punctuation
					else if (result.Groups[9].Value != "" && result.Groups[10].Value != "")
					{
						wordList.Add(result.Groups[9].Value);
						wordList.Add(result.Groups[10].Value + " ");
					}
					// Parse conjunction
					else if (result.Groups[11].Value != "")
					{
						wordList.Add(result.Groups[11].Value + " ");
					}
				}
				else
				{
					wordList.Add(word + " ");
				}
			}
			
			// lineScript of child gets this line's wordList in array form
			words = wordList.ToArray();

            foreach(string t in words){
                WordStructure wordStructure = new WordStructure();
				Match secRes = braceRe.Match(t);
				if(secRes.Success){
					//*wordID*{word|alt}
					//current = word
					//alt = alt
					//dependencies[] = [wordID]
					if (secRes.Groups[1].Value != "" && secRes.Groups[2].Value != "" && secRes.Groups[3].Value != ""){
						wordStructure.fullWord = t;
						dependenciesList.Add(int.Parse(secRes.Groups[1].Value));
						wordStructure.current = secRes.Groups[2].Value;
						wordStructure.alt = secRes.Groups[3].Value;
						wordStructure.dependencies = dependenciesList.ToArray();
					}
					//Assigns current word and alternate word
					//{word|alt}
					//current = word
					//alt = alt
					else if (secRes.Groups[4].Value != "" && secRes.Groups[5].Value != ""){
						wordStructure.fullWord = t;
						wordStructure.current = secRes.Groups[4].Value;
						wordStructure.alt = secRes.Groups[5].Value;
						//print("Dep = N/A");
					}
				}
				wordStructure.wordID = wordStructCount;
				wordStructCount++;

                // Debug.Log(t + " word ID:" + wordStructure.wordID + " Current word: " + wordStructure.current + " Alt word: " + wordStructure.alt);
				// if(wordStructure.dependencies != null){
					// foreach(int num in wordStructure.dependencies){
						// Debug.Log("Dependency " + num);
					// }
				// }
				
				structList.Add(wordStructure);
            }
        }
	}
	
	void Swap(){
		int dependerIndex = 0;
		for(int i = 0; i <= words.Length-1; i++){
			Match swapResult = braceRe.Match(words[i]);
			
			if(swapResult.Success){
				if(swapResult.Groups[1].Value != ""){
					if(dependerIndex == int.Parse(swapResult.Groups[1].Value)){
						foreach(WordStructure wStruct in structList){
							if(wStruct.wordID == i){
								structListIndex = i;
								string tempString = wStruct.current;
								wStruct.current = wStruct.alt;
								wStruct.alt = tempString;
								//#TYBM
							}
						}
					}
				}
				else if(swapResult.Groups[4].Value != "" && swapResult.Groups[5].Value != ""){
					dependerIndex = i;
				}
			}
		}
	}
}