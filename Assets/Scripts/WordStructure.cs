using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordStructure : MonoBehaviour {

    public int wordID, noteID, dependencies, actID;
	public int? wordWeightCurr, wordWeightAlt;
    public string current, alt, lineID, branchAB;
	public bool newLine, lastWord, isPunctuation, isClicked, isChangeable;

    // Default Constructor
    public WordStructure()
    {
		actID = 0;
		isClicked = false;
        wordID = -1;
        current = "N/A";
        alt = "N/A";
        dependencies = -1;
		lineID = "N/A";
		newLine = false;
		lastWord = false;
		wordWeightCurr = null;
		wordWeightAlt = null;
		isPunctuation = false;
		noteID = 0;
		isChangeable = false;
		branchAB = "";
    }

    // Non-default(?) Constructor
    public WordStructure(int actID, bool isPunctuation, int wordID, string current, string alt, int dependencies, string lineID, bool newLine, bool lastWord, int wordWeightCurr, int wordWeightAlt, bool isClicked, bool isChangeable, string branchAB)
    {
		this.actID = actID;
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
		this.isChangeable = isChangeable;
		this.branchAB = branchAB;
    }
}
