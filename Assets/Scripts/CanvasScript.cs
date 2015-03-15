using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
 * Canvas reads the corresponding text file.
 * The script then parses out the file's contents and instantiates lines and line contents.
 */

public class CanvasScript : MonoBehaviour {

    public float curSpacing;
    public float lineSpacing = 0.035f;

    public GameObject paper;
    public PaperScript paperScript;

    public GameObject line;
    public LineScript lineScript;

    public string noteContent;
	public GameObject textBox;
	public TextBox textBoxScript;

    private char delimiterNewline = '\n';
    private char delimiterSpace = ' ';
    private Regex re = new Regex(@"(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\*[0-9]+\*\{[A-Za-z]+\|[A-Za-z]+\})|(\{[A-Za-z]+\|[A-Za-z]+\})([^\w\s'])|(\{[A-Za-z]+\|[A-Za-z]+\})|([A-Za-z]+'[a-z]+)([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z]+)");
	private Regex braceRe = new Regex(@"\*([0-9]+)\*\{([A-Za-z]+)\|([A-Za-z]+)\}|\{([A-Za-z]+)\|([A-Za-z]+)\}");

    public List<string> wordList = new List<string>();
    public string[] words;
    public string[] lines;

	void Start () {
        // Canvas gets the parent paper object
        paper = transform.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();
		
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();

        curSpacing = -.9f;
		
		//Parser();

        /*
        // Debugging
        for (int i = 0; i < wordList.Count; i++)
        {
            print(wordList[i]);
        }
        */
	}
	
	public void Parser(){
		// Note's contents are carried over from parent paper object and parsed
		noteContent = paperScript.noteContent;
		//noteContent = textBoxScript.editString;
        //Debug.Log(noteContent);

        // 'lines' array gets every single line with spaces
        lines = noteContent.Split(delimiterNewline);
		
		foreach (string s in lines)
        {
            // Instantiates a new line and modifies its values accordingly
            if (noteContent != "Start" || noteContent != "Exit")
            {
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
            }
            else
            {
                GameObject newLine = Instantiate(line, (paper.transform.position) + (paper.transform.forward * -0.1f), paper.transform.rotation) as GameObject;

                lineScript = newLine.GetComponent<LineScript>();
                newLine.transform.SetParent(transform);
                
                foreach(string word in words){
                    wordList.Add(word);
                }
            }
            lineScript.words = wordList.ToArray();
			
			if(!paperScript.start && !paperScript.exit){
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
							wordStructure.fullWord = t;
							wordStructure.current = secRes.Groups[4].Value;
							wordStructure.alt = secRes.Groups[5].Value;
							//print("Dep = N/A");
						}
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
				}
			}
        }
	}
	
	void Update(){
		//print(noteContent);
		//print("Start bool is " + paperScript.start);
		//print("Exit bool is " + paperScript.exit);
	}
}
