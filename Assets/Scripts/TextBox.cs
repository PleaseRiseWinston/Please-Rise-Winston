using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class TextBox : MonoBehaviour {

	public float curSpacing;
	public float lineSpacing = 0.035f;
	
	public GameObject paper;
	public PaperScript paperScript;
	
	public GameObject line;
	public LineScript lineScript;

    public GameObject empty;
    public GameObject structHolder;
    public WordStructure wordStructure; 
	
	public string noteContent;
	
	private char delimiterNewline = '\n';
	private char delimiterSpace = ' ';
	private Regex re = new Regex(@"([A-Za-z]+'[a-z])([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z])");
	private Regex braceRe = new Regex(@"\{([A-Za-z]+)\|([A-Za-z]+)\}"); //Looks for {word|alt}
	
	public List<string> wordList = new List<string>();
	public string[] words;
	public string[] lines;
	
	DirectoryInfo info;
	string fileName = "\\winstonNote";
	int fileNum = 0;
	string fileExt = ".prw";
	int count = 0;
	public List<string> arrText;
	string[] fileLoadWords;

	public string editString = "edit me";
	string currDir;
	string[] fileEntries;

	void Start(){
		info = new DirectoryInfo(Application.dataPath);
		currDir = info.ToString();					//makes directory into string
		fileEntries = Directory.GetFiles(currDir);  //gets files in current directory
	}
	
	
	void OnGUI() {
		const int buttonWidth = 84;
		const int buttonHeight = 50;

		Rect buttonParse = new Rect (0, 0, buttonWidth, buttonHeight);
		Rect buttonSave = new Rect(buttonWidth + 10, 0, buttonWidth, buttonHeight);
		Rect buttonDump = new Rect(buttonWidth * 2 + 20, 0, buttonWidth, buttonHeight);
		Rect buttonLoad = new Rect(buttonWidth * 3 + 30, 0, buttonWidth, buttonHeight);
		Rect buttonReset = new Rect(buttonWidth * 4 + 40, 0, buttonWidth, buttonHeight);
		
		editString = GUI.TextArea (new Rect (50, 50, 700, 400), editString, 500);

		if (GUI.Button(buttonParse, "Parse")){
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
			// Instantiates a new line, gives it a collider, and modifies its values accordingly
			GameObject newLine = Instantiate(line, (paper.transform.position) + (paper.transform.forward * -0.1f) + (paper.transform.up * -2 * curSpacing) - new Vector3(paper.transform.right.x * 2.8f, 0, 0), paper.transform.rotation) as GameObject;
			lineScript = newLine.GetComponent<LineScript>();
			newLine.transform.SetParent(transform);
			
			words = s.Split(delimiterSpace);
			
			// Clears the list if there is any content to make room for new line
			wordList.Clear();
			
			// Each word entry is parsed via regex
			foreach (string word in words)
			{
				Match result = re.Match(word);
				
				if (result.Success)
				{
					// Parse conjunction + punctuation
					if (result.Groups[1].Value != "" && result.Groups[2].Value != "")
					{
						wordList.Add(result.Groups[1].Value);
						wordList.Add(result.Groups[2].Value + " ");
					}
					// Parse normal word + punctuation
					else if (result.Groups[3].Value != "" && result.Groups[4].Value != "")
					{
						wordList.Add(result.Groups[3].Value);
						wordList.Add(result.Groups[4].Value + " ");
					}
					// Parse conjunction
					else if (result.Groups[5].Value != "")
					{
						wordList.Add(result.Groups[5].Value + " ");
					}
				}
				else
				{
					wordList.Add(word + " ");
				}
			}
			
			// lineScript of child gets this line's wordList in array form
			lineScript.words = wordList.ToArray();
			
			// Increment curSpacing to add deviation to the line positions
			curSpacing += lineSpacing;

            foreach(string t in lineScript.words){
                WordStructure wordStructure = new WordStructure();
				Match secRes = braceRe.Match(t)
				if(secRes.Success){
					wordStructure.current = secRes.Groups[1].Value;
					wordStructure.alt = secRes.Groups[2].Value;
				}

                Debug.Log(wordStructure.current + " " + wordStructure.alt);
            }
        }
	}
}