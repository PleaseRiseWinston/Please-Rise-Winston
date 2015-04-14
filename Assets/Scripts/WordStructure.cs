using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordStructure : MonoBehaviour {

    public int wordID;
    public string current, alt, lineID;
    public int dependencies;
	public bool newLine;
	public bool lastWord;
	public bool isClicked;

    // Default Constructor
    public WordStructure()
    {
        wordID = 0;
        current = "N/A";
        alt = "N/A";
        dependencies = -1;
		lineID = "N/A";
		newLine = false;
		lastWord = false;
		isClicked = false;

    }

    // Non-default(?) Constructor
    public WordStructure(int wordID, string current, string alt, int dependencies, string lineID, bool newLine, bool lastWord, bool isClicked)
    {
        this.wordID = wordID;
        this.current = current;
        this.alt = alt;
        this.dependencies = dependencies;
		this.lineID = lineID;
		this.newLine = newLine;
		this.lastWord = lastWord;
		this.isClicked = isClicked;
    }
}
