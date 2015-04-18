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
	public int wordWeightCurr;
	public int wordWeightAlt;

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
		wordWeightCurr = 0;
		wordWeightAlt = 0;
    }

    // Non-default(?) Constructor
    public WordStructure(int wordID, string current, string alt, int dependencies, string lineID, bool newLine, bool lastWord, bool isClicked, int wordWeightCurr, int wordWeightAlt)
    {
        this.wordID = wordID;
        this.current = current;
        this.alt = alt;
        this.dependencies = dependencies;
		this.lineID = lineID;
		this.newLine = newLine;
		this.lastWord = lastWord;
		this.isClicked = isClicked;
		this.wordWeightCurr = wordWeightCurr;
		this.wordWeightAlt = wordWeightAlt;
    }
}
