using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameController : MonoBehaviour
{

    public int curAct;
    public GameObject curNote;
    public int curNoteID;
    public GameObject curBackground;

    public GameObject[] backgroundArray;
    public GameObject[][] noteArray;

    public GameObject backgrounds;
    public GameObject notes;

    private Color transparent;
    private Color solid;

    // Use this for initialization
	void Start ()
	{
        // Defaults current Act and Note to 1, 1
        // 'curAct - 1' accounts for indexing convention
	    curAct = 1;
	    curNote = noteArray[curAct - 1][0];
        curBackground = GameObject.Find("game_bg");

        // Declarations for alpha states
        transparent = new Color(1f, 1f, 1f, 0f);
        solid = new Color(1f, 1f, 1f, 1f);

	    backgrounds = GameObject.Find("Backgrounds").gameObject;
        notes = GameObject.Find("Notes").gameObject;

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
                noteArray[i][j] = notes.transform.GetChild(j).gameObject;
	            noteArray[i][j].GetComponent<PaperScript>().noteID = j;
	        }
	    }

        curNoteID = curNote.GetComponent<PaperScript>().noteID;
	    GetNote(curNoteID);
	}

    public void ChangeBackgroundTo(GameObject background)
    {
        curBackground.GetComponent<SpriteRenderer>().color = transparent;
        curBackground = background;
        curBackground.GetComponent<SpriteRenderer>().color = solid;
    }

    // Note flies in from left
    public IEnumerator GetNote(int noteID)
    {
        // Finds the corresponding note with the correct ID and brings it to focus
        foreach (Transform child in GameObject.Find("Notes").transform.GetChild(curAct - 1))
        {
            if (GameObject.Find("Notes").transform.GetChild(curAct - 1).transform.GetComponent<PaperScript>().noteID == noteID)
            yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(0, 1330, -396), false).WaitForCompletion());
        }
    }

    // Send note to tray on desk, increment noteID, and call for new note
    public IEnumerator ToTray()
    {
        curNote.gameObject.GetComponent<PaperScript>().inTray = true;
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(130, 1330, -396), false).WaitForCompletion());

        curNoteID++;

        if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'w')
        {
            ToWinston();
        }
        else if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'p')
        {
            ToProsecutor();
        }
        else if (curNote.transform.GetChild(0).GetComponent<CanvasScript>().submitPaperTo == 'j')
        {
            ToJudge();
        }
        else
        {
            GetNote(curNoteID);
        }
    }

    // Send tray to Winston (right)
    IEnumerator ToWinston()
    {
        foreach (Transform child in GameObject.Find("Notes").transform.GetChild(curAct - 1))
        {
            if (transform.GetComponent<PaperScript>().inTray == true)
            {
                HOTween.To(curNote.gameObject, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.2f, "position", new Vector3(244, 1396, -25), false).WaitForCompletion());
            }
        }
        GetNote(curNoteID);
    }

    // Send tray to Prosecutor (left)
    IEnumerator ToProsecutor()
    {
        foreach (Transform child in GameObject.Find("Notes").transform.GetChild(curAct - 1))
        {
            if (transform.GetComponent<PaperScript>().inTray == true)
            {
                HOTween.To(curNote.gameObject, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.2f, "position", new Vector3(-285, 1396, -79), false).WaitForCompletion());
            }
        }
        GetNote(curNoteID);
    }

    // Send tray to Judge (behind)
    IEnumerator ToJudge()
    {
        foreach (Transform child in GameObject.Find("Notes").transform.GetChild(curAct - 1))
        {
            if (transform.GetComponent<PaperScript>().inTray == true)
            {
                HOTween.To(curNote.gameObject, 0.2f, "rotation", new Vector3(35, 12, -3), false);
                yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.2f, "position", new Vector3(114, 1360, -481), false).WaitForCompletion());
            }
        }
        GetNote(curNoteID);
    }
}
