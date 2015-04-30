using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameController : MonoBehaviour
{
    public int curAct;
    public GameObject curNote;
    public int curNoteID;
    public string curNoteName;
    public GameObject curBackground;

    public GameObject[] backgroundArray;
    public GameObject[][] noteArray;

    public GameObject backgrounds;
    public GameObject notes;

    private Color transparent;
    private Color solid;

    public int totalWeight;
    
    void Start ()
    {
        backgrounds = GameObject.FindGameObjectWithTag("Backgrounds").gameObject;
        notes = GameObject.FindGameObjectWithTag("Notes").gameObject;
        backgroundArray = new GameObject[backgrounds.transform.childCount];

        totalWeight = 0;

        // Defaults current Act and Note to 1, 1
        // 'curAct - 1' accounts for reverse indexing convention
        curAct = 1;
        curNoteID = 1;
        //curNoteID = notes.transform.GetChild(curAct - 1).childCount;
        curBackground = GameObject.FindGameObjectWithTag("GameBackground");

        // Declarations for alpha states
        transparent = new Color(1f, 1f, 1f, 0f);
        solid = new Color(1f, 1f, 1f, 1f);

        // Insert all backgrounds into array
        for (int i = 0; i < backgrounds.transform.childCount; i++)
	    {
	        backgroundArray[i] = backgrounds.transform.GetChild(i).gameObject;
	    }

	    // Insert all notes into jagged array
        noteArray = new GameObject[notes.transform.childCount][];

        for (int i = 0; i < notes.transform.childCount; i++)
	    {
            noteArray[i] = new GameObject[notes.transform.GetChild(i).childCount];

            // Insert notes into the act's array and index noteIDs in order starting from 0
            for (int j = 0; j < notes.transform.GetChild(i).childCount; j++)
	        {
                noteArray[i][j] = notes.transform.GetChild(i).GetChild(j).gameObject;
	        }
	    }

        UpdateCurNote(curNoteID);
	}

    public void ChangeBackgroundTo(GameObject background)
    {
        curBackground.GetComponent<SpriteRenderer>().color = transparent;
        curBackground = background;
        curBackground.GetComponent<SpriteRenderer>().color = solid;
    }

    // Converts input int to string for future searching and matching
    public void UpdateCurNote(int noteID)
    {
        string noteIDstr = noteID.ToString();
        curNoteName = curAct + "." + noteIDstr;
        curNote = noteArray[curAct - 1][noteID - 1];
    }

    // Note flies in from left
    public void GetNote(string noteID)
    {
        Debug.Log("Getting Note: " + noteID);
        // Finds the corresponding note with the correct ID and brings it to center
        for (int i = 0; i < notes.transform.GetChild(curAct - 1).childCount; i++)
        {
            if (GameObject.FindGameObjectWithTag("Notes").transform.GetChild(curAct - 1).GetChild(i).gameObject.name == noteID)
            {
                StartCoroutine(MoveToCenter(curAct - 1, i));
                print(curAct - 1 + ", " + i);
            }
        }
    }

    IEnumerator MoveToCenter(int actIndex, int i)
    {
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("Notes").transform.GetChild(actIndex).GetChild(i).transform, 0.8f, "position", new Vector3(0, 1330, -400), false).WaitForCompletion());
    }

    // Send note to tray on desk, increment noteID, and call for new note
    public void ToTray()
    {
        curNote.gameObject.GetComponent<PaperScript>().inTray = true;

        if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'w')
        {
            StartCoroutine(ToWinston());
        }
        else if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'p')
        {
            StartCoroutine(ToProsecutor());
        }
        else if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'j')
        {
            StartCoroutine(ToJudge());
        }
        else
        {
            StartCoroutine(MoveToTray());
            curNoteID++;
            UpdateCurNote(curNoteID);

            GetNote(curNoteName);
        }
    }

    IEnumerator MoveToTray()
    {
        HOTween.To(curNote.transform, 0.15f, "rotation", new Vector3(85, 0, 0), false);
        yield return StartCoroutine(HOTween.To(curNote.transform, 0.15f, "position", new Vector3(85, 1330, -400), false).WaitForCompletion());
        curNote.GetComponent<PaperScript>().atDestination = true;
    }

    // Send tray to Winston (right)
    IEnumerator ToWinston()
    {
        for (int i = 0; i < notes.transform.GetChild(curAct - 1).childCount; i++)
        {
            if (notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray == true)
            {
                print("To Winston");
                notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(244, 1396, -25), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
            }
        }
        curNoteID++;
        UpdateCurNote(curNoteID);
        GetNote(curNoteName);
    }

    // Send tray to Prosecutor (left)
    IEnumerator ToProsecutor()
    {
        for (int i = 0; i < notes.transform.GetChild(curAct - 1).childCount; i++)
        {
            if (notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray == true)
            {
                print("To Prosecutor");
                notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(-285, 1396, -79), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
            }
        }
        curNoteID++;
        UpdateCurNote(curNoteID);
        GetNote(curNoteName);
    }

    // Send tray to Judge (behind)
    IEnumerator ToJudge()
    {
        for (int i = 0; i < notes.transform.GetChild(curAct - 1).childCount; i++)
        {
            if (notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray == true)
            {
                print("To Judge");
                notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().inTray = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(35, 12, -3), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(114, 1360, -581), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
            }
        }
        curNoteID++;
        UpdateCurNote(curNoteID);
        GetNote(curNoteName);
    }
}
