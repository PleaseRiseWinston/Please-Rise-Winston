using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

/*
 * Canvas reads the corresponding text file.
 * The script then parses out the file's contents and instantiates lines and line contents.
 */

public class CanvasScript : MonoBehaviour {

    public float curSpacing;
    public const float lineSpacing = -5f;

    public GameObject paper;
    public PaperScript paperScript;

    public GameObject line;
    public LineScript lineScript;

    public string noteContent;

    private const char delimiterNewline = '\n';
    private const char delimiterSpace = ' ';
    private Regex re = new Regex(@"(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})|(\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\{[A-Za-z]+\|[A-Za-z]+\})|([A-Za-z]+'[a-z]+)([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z]+)");
	private Regex braceRe = new Regex(@"\*([0-9]+)\*\{([A-Za-z]+)\|([A-Za-z]+)\}|\{([A-Za-z]+)\|([A-Za-z]+)\}");

    public List<string> wordList = new List<string>();
    public string[] words;
    public string[] lines;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	//May or may not be useful... k
	public List<string> displayWords = new List<string>();
	
	public bool wordOptionClicked = false;
	
	public int linePosCount;
	public List<string> lineIDList = new List<string>();

	public int quickFixNum = 0;
	
	void Start () {
		// Canvas gets the parent paper object
        paper = transform.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();
		
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();

        if (!paperScript.start && !paperScript.exit)
        {
            curSpacing = 10;
        }
        else
        {
            curSpacing = 0;
        }
		
		// Note's contents are carried over from parent paper object and parsed
		//linePosCount = 1;
        noteContent = paperScript.noteContent;
        //Debug.Log(noteContent);

        Parser();

        /*
        // Debugging
        for (int i = 0; i < wordList.Count; i++)
        {
            print(wordList[i]);
        }
        */
	}
	
	public void Parser(){
        // 'lines' array gets every single line with spaces
        lines = noteContent.Split(Environment.NewLine.ToCharArray());
		int lineCount = lines.Length;
		int lineCounter = 1;
		foreach (string s in lines)
        {
			int arrayCount = 0;
			displayWords.Clear();
            // Instantiates a new line and modifies its values accordingly
            if (!paperScript.start && !paperScript.exit)
            {

				//Creating newLine for paper on screen
                GameObject newLine = Instantiate(line, paper.transform.position + (paper.transform.up * curSpacing), paper.transform.rotation) as GameObject;
				newLine.name = "Line" + linePosCount;
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

                // Increment curSpacing to add deviation to the line positions
                curSpacing += lineSpacing;
				/*for (int i =0; i<=3; i++){
					print(linePosArray[i]);
					linePosCount++;
				}*/
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
			if(!paperScript.start && !paperScript.exit){
				print(words.Length);
				
				foreach(string t in words){
					//print(t);

					WordStructure wordStructure = new WordStructure();
					Match secRes = braceRe.Match(t);
					if(secRes.Success){
						//*wordID*{word|alt}
						//current = word
						//alt = alt
						//dependencies[] = [wordID]
						if (secRes.Groups[1].Value != "" && secRes.Groups[2].Value != "" && secRes.Groups[3].Value != ""){
							//print(secRes.Groups[1].Value);
							//print(secRes.Groups[2].Value);
							//print(secRes.Groups[3].Value);
							textBoxScript.dependenciesList.Add(int.Parse(secRes.Groups[1].Value));
							wordStructure.current = secRes.Groups[2].Value;
							wordStructure.alt = secRes.Groups[3].Value;
							wordStructure.dependencies = textBoxScript.dependenciesList.ToArray();
						}
						//Assigns current word and alternate word
						//{word|alt}
						//current = word
						//alt = alt
						else if (secRes.Groups[4].Value != "" && secRes.Groups[5].Value != ""){
							wordStructure.current = secRes.Groups[4].Value;
							wordStructure.alt = secRes.Groups[5].Value;
							//print("Dep = N/A");
						}
					}
					else{
						wordStructure.current = t;
					}
					wordStructure.wordID = textBoxScript.wordStructCount;
					textBoxScript.wordStructCount++;
					
					// Debug.Log(t + " word ID:" + wordStructure.wordID + " Current word: " + wordStructure.current + " Alt word: " + wordStructure.alt);
					// if(wordStructure.dependencies != null){
					// foreach(int num in wordStructure.dependencies){
					// Debug.Log("Dependency " + num);
					// }
					// }
					
					textBoxScript.structList.Add(wordStructure);
					displayWords.Add(wordStructure.current);
					//print(wordStructure.current);
					if (arrayCount == words.Length - 1 && lineCount != lineCounter){
						wordStructure.newLine = true;
						wordStructure.lastWord = false;
					}
					else if (arrayCount == words.Length -1 && lineCount == lineCounter) {
						wordStructure.newLine = false;
						wordStructure.lastWord = true;
					}
					else {
						wordStructure.newLine = false;
						wordStructure.lastWord = false;
					}
					arrayCount++;
				}
				//print(lineScript.words.Length);
				lineScript.words = displayWords.ToArray();
				
			}

			lineCounter++;
        }
	}
}
