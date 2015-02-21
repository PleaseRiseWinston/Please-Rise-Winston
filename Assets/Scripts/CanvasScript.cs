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
    public float lineSpacing = 25;

    public GameObject paper;
    public PaperScript paperScript;

    public GameObject line;
    public LineScript lineScript;

    public string noteContent;

    private char delimiterNewline = '\n';
    private char delimiterSpace = ' ';
    private Regex re = new Regex(@"([A-Za-z]+'[a-z])([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z])");

    public List<string> wordList = new List<string>();
    public string[] words;
    public string[] lines;

	void Start () {
        // Canvas gets the parent paper object
        paper = transform.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();

        // Note's contents are carried over from parent paper object and parsed
        noteContent = paperScript.noteContent;

        // 'lines' array gets every single line with spaces
        lines = noteContent.Split(delimiterNewline);

        foreach (string s in lines)
        {
            curSpacing = 0;

            // Instantiates a new line and modifies its values accordingly
            GameObject newLine = Instantiate(line, transform.position + new Vector3(0, curSpacing, 0), transform.rotation) as GameObject;
            // Line becomes a child of this canvas
            newLine.transform.SetParent(transform);
            lineScript = newLine.GetComponent<LineScript>();

            words = s.Split(delimiterSpace);
            //lineScript.list = words;

            // Each word entry is parsed via regex
            foreach (string i in words)
            {
                //Debug.Log(i);

                Match result = re.Match(i);

                if (result.Success)
                {
                    // Parse conjunction + punctuation
                    if (result.Groups[1].Value != "" && result.Groups[2].Value != "")
                    {
                        wordList.Add(result.Groups[1].Value);
                        wordList.Add(result.Groups[2].Value);
                    }
                    // Parse normal word + punctuation
                    else if (result.Groups[3].Value != "" && result.Groups[4].Value != "")
                    {
                        wordList.Add(result.Groups[3].Value);
                        wordList.Add(result.Groups[4].Value);
                    }
                    // Parse conjunction
                    else if (result.Groups[5].Value != "")
                    {
                        wordList.Add(result.Groups[5].Value);
                    }
                }
                else
                {
                    wordList.Add(i);
                }
            }

            // Increment curSpacing to add deviation to the line positions
            curSpacing += lineSpacing;
        }

        
        // Debugging
        for (int i = 0; i < wordList.Count; i++)
        {
            print(wordList[i]);
        }
        
	}
}
