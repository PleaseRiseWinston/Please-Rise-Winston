using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

/*
 * Canvas reads the corresponding text file.
 * The script then parses out the file's contents and instantiates lines and line contents.
 */

public class CanvasScript : MonoBehaviour {

    public float curSpacing;
    public const float lineSpacing = -4.5f;

    public GameObject paper;
    public PaperScript paperScript;

    public GameObject line;
    public LineScript lineScript;

    public string noteContent;

    private const char delimiterNewline = '\n';
    private const char delimiterSpace = ' ';
	private string[] punctuationArray = {".", ",", ";", ":", "!", "?"};
	//{([A-Za-z]+)\^([0-9])\|([A-Za-z]+)\^([0-9])}
    private Regex re = new Regex(@"(\$BRANCH)|#([A-Za-z]+)|(@[A-Z])|(\*[0-9]+\*[0-9]+\*\{.+\|.+\})([.,!?:;])|(\*[0-9]+\*[0-9]+\*\{.+\|.+\})|(\{.+\^.+\|.+\^.+\})([^\w\s'])|(\{.+\^.+\|.+\^.+\})|([A-Za-z]+'[a-z]+)([^\w\s'])|([A-Za-z]+)([.,!?:;])|([A-Za-z]+'[a-z]+)|([0-9]+\.[0-9]+[A-Za-z])([^\w\s'])");
	private Regex braceRe = new Regex(@"\*([0-9]+)\*([0-9]+)\*\{(.+)\|(.+)\}|\{(.+)\^(.+)\|(.+)\^(.+)\}");
	private Regex noteRegex = new Regex(@"([0-9]+).([0-9]+)");

    public List<string> wordList = new List<string>();
    public string[] words;
    public string[] lines;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	public GameObject gameController;
	public GameController gameControllerScript;
	
	//May or may not be useful... k
	public List<string> displayWords = new List<string>();
	
	public bool wordOptionClicked = false;
	
	public int linePosCount;
	public List<string> lineIDList = new List<string>();

	public char submitPaperTo;
	public string witnessState;
	public bool branchState = false;

	public string noteName;
	public int wordNum;
	public int firstNum = 0;
	public int secNum = 0;
	public int noteIndexCount;
	
	void Start () {
		// Canvas gets the parent paper object
        paper = transform.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();
		
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
		
		gameController = GameObject.Find("GameController");
		gameControllerScript = gameController.GetComponent<GameController>();
		
		noteName = transform.parent.name;

        if (!paperScript.start && !paperScript.exit)
        {
			actNumberParse();
			curSpacing = 10;
        }
        else
        {
            curSpacing = 0;
        }
		
		Parser();
	}
	
	public void Parser(){		
		if(textBoxScript.didSwap == true){
			actNumberParse();
		}
		
		noteContent = paperScript.noteContent;
		
		//print(gameObject.transform.parent.name);
        // 'lines' array gets every single line with spaces
		lines = noteContent.Split(Environment.NewLine.ToCharArray());
		
		int lineCount = lines.Length;
		int lineCounter = 1;
		
		linePosCount = 0;
		wordNum = 0;
		curSpacing = 10;
		textBoxScript.wordStructCount = 0;
		
		foreach (string s in lines)
        {
			int arrayCount = 0;
			displayWords.Clear();
            // Instantiates a new line and modifies its values accordingly
            if (!paperScript.start && !paperScript.exit)
            {

				//Creating newLine for paper on screen
                GameObject newLine = Instantiate(line, paper.transform.position + (paper.transform.up * 14) + (paper.transform.up * curSpacing), paper.transform.rotation) as GameObject;
				newLine.name = "Line" + linePosCount;
				newLine.tag = "Line";
				linePosCount++;
				
				lineIDList.Add(newLine.name);

                lineScript = newLine.GetComponent<LineScript>();
                newLine.transform.SetParent(transform);
				//End newLine
				
				//Adding to words array
                words = s.Split(delimiterSpace);

                // Clears the list if there is any content to make room for new line
                wordList.Clear();

                // Each word entry is parsed via regex
                foreach (string word in words)
                {
                    Match result = re.Match(word);

                    if (result.Success)
                    {
						//$BRANCH
						if(result.Groups[1].Value != ""){
							branchState = true;
						}
						//#W
						if(result.Groups[2].Value != ""){
							witnessState = result.Groups[2].Value;
						}
						//@W
						else if(result.Groups[3].Value != ""){
							if(result.Groups[3].Value == "@W"){
								submitPaperTo = 'w';
							}
							else if(result.Groups[3].Value == "@P"){
								submitPaperTo = 'p';
							}
							else if(result.Groups[3].Value == "@J"){
								submitPaperTo = 'j';
							}
						}
                        // Parse *wordID*{word|alt} with and without punctuation
						else if (result.Groups[4].Value != "" && result.Groups[5].Value != ""){
							wordList.Add(result.Groups[4].Value);
							wordList.Add(result.Groups[5].Value);
						}
						else if (result.Groups[6].Value != ""){
							wordList.Add(result.Groups[6].Value);
						}
						//Parse {word|alt} with and without punctuation
						else if (result.Groups[7].Value != "" && result.Groups[8].Value != "")
						{
							//print(result.Groups[7].Value);
							wordList.Add(result.Groups[7].Value);
							wordList.Add(result.Groups[8].Value);
						}
						else if (result.Groups[9].Value != ""){
							wordList.Add(result.Groups[9].Value);
						}
						// Parse conjunction + punctuation
						else if (result.Groups[10].Value != "" && result.Groups[11].Value != ""){
							wordList.Add(result.Groups[10].Value);
							wordList.Add(result.Groups[11].Value);
						}
						// Parse normal word + punctuation
						else if (result.Groups[12].Value != "" && result.Groups[13].Value != "")
						{
							wordList.Add(result.Groups[12].Value);
							wordList.Add(result.Groups[13].Value);
						}
						// Parse conjunction
						else if (result.Groups[14].Value != "")
						{
							wordList.Add(result.Groups[14].Value);
						}
						else if(result.Groups[15].Value != "" && result.Groups[16].Value != ""){
							wordList.Add(result.Groups[15].Value);
							wordList.Add(result.Groups[16].Value);
						}
                    }
                    else
                    {
                        wordList.Add(word);
                    }
                }

                // Increment curSpacing to add deviation to the line positions
                curSpacing += lineSpacing;
            }
            else
            {
                GameObject newLine = Instantiate(line, (paper.transform.position) + (paper.transform.forward * -0.1f), paper.transform.rotation) as GameObject;

                lineScript = newLine.GetComponent<LineScript>();
                newLine.transform.SetParent(transform);

                words = s.Split(delimiterSpace);

                foreach(string word in words){
                    wordList.Add(word);
                }
            }
			
            lineScript.words = wordList.ToArray();
			
			/********** GIVING WORDS A WORD STRUCTURE **********/
			if(!paperScript.start && !paperScript.exit){
				//print(words.Length);
				foreach(string t in lineScript.words){
					//print(t);
					WordStructure wordStructure = new WordStructure();
					Match secRes = braceRe.Match(t);
					
					if(gameObject.transform.parent.name == firstNum + "." + secNum + "a"){
						wordStructure.branchAB = "a";
					}
					else if(gameObject.transform.parent.name == firstNum + "." + secNum + "b"){
						wordStructure.branchAB = "b";
					}
					
					if(secRes.Success){
						//*(noteID)*(wordID)*{(word)|(alt)}
						//current = word
						//alt = alt
						//dependencies[] = [wordID]
						if (secRes.Groups[1].Value != "" && secRes.Groups[2].Value != "" && secRes.Groups[3].Value != "" &&  secRes.Groups[4].Value != ""){
							wordStructure.current = secRes.Groups[3].Value;
							wordStructure.alt = secRes.Groups[4].Value;
							wordStructure.noteID = int.Parse(secRes.Groups[1].Value);
							wordStructure.dependencies = int.Parse(secRes.Groups[2].Value);
							wordStructure.wordID = textBoxScript.wordStructCount;
							wordStructure.noteID = secNum;
							wordStructure.actID = firstNum;
							
							textBoxScript.wordStructCount++;
							wordNum++;
							textBoxScript.structList.Add(wordStructure);
							textBoxScript.swapWordList.Add(wordStructure);
							displayWords.Add(wordStructure.current);
						}
						//Assigns current word and alternate word
						//{word|alt}
						//current = word
						//alt = alt
						else if (secRes.Groups[5].Value != "" && int.Parse(secRes.Groups[6].Value) != -1 && secRes.Groups[7].Value != "" && int.Parse(secRes.Groups[8].Value) != -1){
							wordStructure.current = secRes.Groups[5].Value;
							wordStructure.alt = secRes.Groups[7].Value;
							wordStructure.wordWeightCurr = int.Parse(secRes.Groups[6].Value);
							wordStructure.wordWeightAlt = int.Parse(secRes.Groups[8].Value);
							wordStructure.wordID = textBoxScript.wordStructCount;
							wordStructure.noteID = secNum;
							wordStructure.isChangeable = true;
							wordStructure.actID = firstNum;
							
							textBoxScript.wordStructCount++;
							wordNum++;
							textBoxScript.structList.Add(wordStructure);
							textBoxScript.swapWordList.Add(wordStructure);
							displayWords.Add(wordStructure.current);
						}
						//punctuation
						// else if(secRes.Groups[9].Value != ""){
							// wordStructure.current = secRes.Groups[9].Value;
							// wordStructure.isPunctuation = true;
							// wordStructure.noteID = secNum;
							
							// textBoxScript.structList.Add(wordStructure);
							// displayWords.Add(wordStructure.current);
							// wordNum++;
						// }
					}
					else{						
						if(punctuationArray.Contains(t) == true){
							//print(t);
							wordStructure.current = t;
							wordStructure.isPunctuation = true;
							wordStructure.noteID = secNum;
							
							textBoxScript.structList.Add(wordStructure);
							displayWords.Add(wordStructure.current);
							wordNum++;
						}
						else{	
							wordStructure.current = t;
							wordStructure.wordID = textBoxScript.wordStructCount;
							wordStructure.noteID = secNum;
							
							textBoxScript.wordStructCount++;
							wordNum++;
							textBoxScript.structList.Add(wordStructure);					
							displayWords.Add(wordStructure.current);
						}
					}

					if (arrayCount == lineScript.words.Length - 1 && lineCount != lineCounter){
						wordStructure.newLine = true;
						wordStructure.lastWord = false;
					}
					else if (arrayCount == lineScript.words.Length -1 && lineCount == lineCounter) {
						wordStructure.newLine = false;
						wordStructure.lastWord = true;
					}
					else {
						wordStructure.newLine = false;
						wordStructure.lastWord = false;
					}
					arrayCount++;
				}
				lineScript.words = displayWords.ToArray();
			}
			lineCounter++;
        }
		
		if(noteContent != "Start" && noteContent != "Exit"){
			textBoxScript.noteWordCount[firstNum -1][secNum - 1] = wordNum;
		}
	}
	
	public void actNumberParse(){
			Match noteNumber = noteRegex.Match(noteName);
			
			if (noteNumber.Success){
				//print(noteNumber.Groups[2].Value);
				firstNum = int.Parse(noteNumber.Groups[1].Value);
				secNum = int.Parse(noteNumber.Groups[2].Value);
				noteIndexCount = textBoxScript.allNoteLines[firstNum - 1].Length - 1;

				//paperScript.noteContent = textBoxScript.allNoteLines[firstNum -1][textBoxScript.allNoteLines[firstNum - 1].Length - secNum];
				if(noteName == firstNum + "." + secNum + "a"){
					paperScript.noteContent = textBoxScript.allNoteLines[firstNum -1][findABNoteIndex('a')];
				}
				else if(noteName == firstNum + "." + secNum + "b"){
					paperScript.noteContent = textBoxScript.allNoteLines[firstNum -1][findABNoteIndex('b')];
				}
				else{
					paperScript.noteContent = textBoxScript.allNoteLines[firstNum -1][secNum - 1];
				}
				textBoxScript.editString = paperScript.noteContent;
			}
		//print(gameObject.transform.parent.name);
	}
	
	public int findABNoteIndex(char extAorB){
		foreach(GameObject notes in GameObject.FindGameObjectsWithTag("Papers")){
			if(noteName == notes.name){
				return noteIndexCount;
			}
			noteIndexCount--;
		}
		return 0;
	}
}
