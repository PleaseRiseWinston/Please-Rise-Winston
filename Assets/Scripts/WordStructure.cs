using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordStructure : MonoBehaviour {

    public int wordID, noteID, dependencies, wordWeightCurr, wordWeightAlt;
    public string current, alt, lineID;
	public bool newLine, lastWord, isPunctuation, isClicked;

    // Default Constructor
    public WordStructure()
    {
		isClicked = false;
        wordID = -1;
        current = "N/A";
        alt = "N/A";
        dependencies = -1;
		lineID = "N/A";
		newLine = false;
		lastWord = false;
		wordWeightCurr = 0;
		wordWeightAlt = 0;
		isPunctuation = false;
		noteID = 0;
    }

    // Non-default(?) Constructor
    public WordStructure(bool isPunctuation, int wordID, string current, string alt, int dependencies, string lineID, bool newLine, bool lastWord, int wordWeightCurr, int wordWeightAlt, bool isClicked)
    {
		this.noteID = noteID;
		this.isPunctuation = isPunctuation;
        this.wordID = wordID;
        this.current = current;
        this.alt = alt;
        this.dependencies = dependencies;
		this.lineID = lineID;
		this.newLine = newLine;
		this.lastWord = lastWord;
		this.wordWeightCurr = wordWeightCurr;
		this.wordWeightAlt = wordWeightAlt;
		this.isClicked = isClicked;
    }
}
