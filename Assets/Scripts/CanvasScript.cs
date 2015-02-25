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

        curSpacing = -.9f;

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
        }

        /*
        // Debugging
        for (int i = 0; i < wordList.Count; i++)
        {
            print(wordList[i]);
        }
        */
	}
}
