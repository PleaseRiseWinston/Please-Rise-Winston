using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordStructure : MonoBehaviour {

    public int wordID;
    public string current, alt, fullWord;
    public int[] dependencies;

    // Default Constructor
    public WordStructure()
    {
		fullWord = "N/A";
        wordID = 0;
        current = "N/A";
        alt = "N/A";
        dependencies = null;
    }

    // Non-default(?) Constructor
    public WordStructure(int wordID, string current, string alt, int[] dependencies, string fullWord)
    {
		this.fullWord = fullWord;
        this.wordID = wordID;
        this.current = current;
        this.alt = alt;
        this.dependencies = dependencies;
    }
}
