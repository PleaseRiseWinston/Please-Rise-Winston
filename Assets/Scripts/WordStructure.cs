using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordStructure : MonoBehaviour {

    public int wordID;
    public string current, alt, lineID;
    public int[] dependencies;

    // Default Constructor
    public WordStructure()
    {
        wordID = 0;
        current = "N/A";
        alt = "N/A";
        dependencies = null;
		lineID = "N/A";
    }

    // Non-default(?) Constructor
    public WordStructure(int wordID, string current, string alt, int[] dependencies, string lineID)
    {
        this.wordID = wordID;
        this.current = current;
        this.alt = alt;
        this.dependencies = dependencies;
		this.lineID = lineID;
    }
}
