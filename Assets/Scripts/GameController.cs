﻿using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameController : MonoBehaviour
{

    public int curAct;
    public GameObject curNote;
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
	    curAct = 1;
	    curNote = noteArray[curAct][1];
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

	    // Insert all notes into a jagged array
        noteArray = new GameObject[notes.transform.childCount][];
        for (int i = 0; i < notes.transform.childCount; i++)
	    {
            noteArray[i] = new GameObject[notes.transform.GetChild(i).childCount];

            // Insert notes into the act's array
            for (int j = 0; j < notes.transform.GetChild(i).childCount; j++)
	        {
                noteArray[i][j] = notes.transform.GetChild(j).gameObject;
	        }
	    }
	}

    public void ChangeBackgroundTo(GameObject background)
    {
        curBackground.GetComponent<SpriteRenderer>().color = transparent;
        curBackground = background;
        curBackground.GetComponent<SpriteRenderer>().color = solid;
    }

    // Note flies in from left
    IEnumerator GetNote()
    {
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(0, 1330, -396), false).WaitForCompletion());
    }

    // Send note to tray on deskd
    IEnumerator ToTray()
    {
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(130, 1330, -396), false).WaitForCompletion());
    }

    IEnumerator ToWinston()
    {
        // Send note to Winston (right)
        HOTween.To(curNote.gameObject, 0.4f, "rotation", new Vector3(83, 31, -161), false);
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(244, 1396, -25), false).WaitForCompletion());
    }

    IEnumerator ToProsecutor()
    {
        // Send note to Prosecutor (left)
        HOTween.To(curNote.gameObject, 0.4f, "rotation", new Vector3(83, 31, -161), false);
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.4f, "position", new Vector3(-285, 1396, -79), false).WaitForCompletion());
    }

    IEnumerator ToJudge()
    {
        // Send note to Judge (behind)
        HOTween.To(curNote.gameObject, 0.2f, "rotation", new Vector3(35, 12, -3), false);
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.2f, "position", new Vector3(114, 1360, -381), false).WaitForCompletion());
        yield return StartCoroutine(HOTween.To(curNote.gameObject, 0.2f, "position", new Vector3(0, 0, -100), true).WaitForCompletion());
    }
}
