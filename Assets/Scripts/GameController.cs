using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public int curAct;
    public int curNote;

    public GameObject[] backgroundArray;
    public GameObject[][] noteArray;

    public GameObject backgrounds;
    public GameObject notes;

    // Use this for initialization
	void Start ()
	{
        // Defaults current Act and Note to 1, 1
	    curAct = 1;
	    curNote = 1;

	    backgrounds = GameObject.Find("Backgrounds").gameObject;
        notes = GameObject.Find("Notes").gameObject;

        // Insert all backgrounds into array
        for (int i = 0; i < backgrounds.transform.childCount; i++)
	    {
	        backgroundArray[i] = backgrounds.transform.GetChild(i).gameObject;
	    }

	    // Insert all notes into a jagged array
        noteArray = new GameObject[notes.transform.childCount][];
        for (int i = 0; i < notes.transform.childCount; i++)
	    {
            noteArray[i] = new GameObject[notes.transform.GetChild(i).childCount];

            // Insert notes into the act's array
            for (int j = 0; i < notes.transform.GetChild(j).childCount; j++)
	        {
                noteArray[i][j] = notes.transform.GetChild(j).gameObject;
	        }
	    }
	}

    public void ChangeBackgroundTo(MeshFilter background)
    {
        
    }

    IEnumerator NewNote
    {
        
    }

    IEnumerator ToWinston
    {
        
    }

    IEnumerator ToProsecutor
    {
        
    }

    IEnumerator ToJudge
    {
        
    }
}
