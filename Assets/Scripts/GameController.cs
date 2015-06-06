using System;
using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int curAct;
    public GameObject curNote;
    public int curNoteID;
    public string curNoteName;
    public GameObject curBackground;

    public GameObject[] backgroundArray;
    public GameObject[][] noteArray;
    public GameObject[][] branchA;
    public GameObject[][] branchB;
    public GameObject backgrounds;
    public GameObject notes;

    public Color transparent;
    public Color solid;

    public int totalWeight = 0;
    public int subtotalWeight = 0;
    public bool curNoteInMotion;

    public bool overlayActive = false;
    public bool onBranch = false;

    public int branchDiscrepancy = 0;
    public bool branchState;
    public char branchType;

    public float stackHeight = 0;

	public string storySoFar = null;
    
    void Start ()
    {
        backgrounds = GameObject.FindGameObjectWithTag("Backgrounds").gameObject;
        notes = GameObject.FindGameObjectWithTag("Notes").gameObject;
        backgroundArray = new GameObject[backgrounds.transform.childCount];

        curNoteInMotion = false;
		
        // Defaults current Act and Note to 1, 1
        // 'curAct - 1' accounts for reverse indexing convention
        curAct = 1;
        curNoteID = 1;
        //curNoteID = notes.transform.GetChild(curAct - 1).childCount;
        curBackground = GameObject.FindGameObjectWithTag("GameBackground");

        // Declarations for alpha states
        transparent = new Color(1f, 1f, 1f, 0f);
        solid = new Color(1f, 1f, 1f, 1f);
        GameObject.FindGameObjectWithTag("PaperGlow").transform.GetComponent<SpriteRenderer>().color = transparent;
        GameObject.FindGameObjectWithTag("PaperGlowStart").transform.GetComponent<SpriteRenderer>().color = transparent;
        GameObject.FindGameObjectWithTag("PaperGlowExit").transform.GetComponent<SpriteRenderer>().color = transparent;

        // Insert all backgrounds into array
        for (int i = 0; i < backgrounds.transform.childCount; i++)
	    {
	        backgroundArray[i] = backgrounds.transform.GetChild(i).gameObject;
	    }

	    // Instantiate, then insert all notes into jagged array
        noteArray = new GameObject[notes.transform.childCount][];
        branchA = new GameObject[notes.transform.childCount][];
        branchB = new GameObject[notes.transform.childCount][];

        for (int i = 0; i < notes.transform.childCount; i++)
	    {
            noteArray[i] = new GameObject[notes.transform.GetChild(i).childCount];
            branchA[i] = new GameObject[notes.transform.GetChild(i).childCount];
            branchB[i] = new GameObject[notes.transform.GetChild(i).childCount];
            int counterA = 0;
            int counterB = 0;

            // Insert notes into the act's array and index noteIDs in order starting from 0
            for (int j = 0; j < notes.transform.GetChild(i).childCount; j++)
	        {
                noteArray[i][j] = notes.transform.GetChild(i).GetChild(j).gameObject;
                print(noteArray[i][j].GetComponent<PaperScript>().branchType);
                if (notes.transform.GetChild(i).GetChild(j).GetComponent<PaperScript>().branchType == "a")
                {
                    branchA[i][counterA] = notes.transform.GetChild(i).GetChild(j).gameObject;
                    print(branchA[i][counterA].name);
                    counterA++;
                }
                if (notes.transform.GetChild(i).GetChild(j).GetComponent<PaperScript>().branchType == "b")
                {
                    branchB[i][counterB] = notes.transform.GetChild(i).GetChild(j).gameObject;
                    print(branchB[i][counterB].name);
                    counterB++;
                }
	        }
	    }

        ToggleAct(curAct);
        UpdateCurNote(curNoteID, null);
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            curNote.GetComponent<PaperScript>().inTray = true;
            ToTray();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            curNoteID = 59;
            curNote.GetComponent<PaperScript>().inTray = true;
            ToTray();
        }
    }

    public void ToggleAct(int curAct)
    {
        // Toggles all non-current acts false
        for(int i = 0; i < notes.transform.childCount; i++)
        {
            if (i == (curAct - 1))
            {
                for (int j = 0; j < notes.transform.GetChild(i).transform.childCount; j++)
                {
                    notes.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(true);
                    //switch (curAct)
                    //{
                    //    case 1:
                    //        branchDiscrepancy = 2;
                    //        break;
                    //    case 2:
                    //        branchDiscrepancy = 3;
                    //        break;
                    //    case 3:
                    //        branchDiscrepancy = 3;
                    //        break;
                    //    case 4:
                    //        branchDiscrepancy = 0;
                    //        break;
                    //}
                }
            }
            else if (i != (curAct - 1))
            {
                for (int j = 0; j < notes.transform.GetChild(i).transform.childCount; j++)
                {
                    notes.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    // Takes a string of the tag and focuses the corresponding background
    public void ChangeBackgroundTo(string backgroundName)
    {
        HOTween.To(curBackground.GetComponent<SpriteRenderer>(), 0.7f, "color", transparent);
        curBackground = GameObject.FindGameObjectWithTag(backgroundName);
        HOTween.To(curBackground.GetComponent<SpriteRenderer>(), 0.7f, "color", solid);
    }

    // Converts input int to string for future searching and matching
    public void UpdateCurNote(int noteID, string branchSuffix)
    {
        Debug.Log(branchSuffix + ", " + onBranch);
        string noteIDstr = noteID + branchSuffix;
        curNoteName = curAct + "." + noteIDstr;

        // At branch start:
        if (branchSuffix != null && !onBranch)
        {
            onBranch = true;
            Debug.Log("Branch Start.");
            switch (branchSuffix)
            {
                case "a":
                    curNote = noteArray[curAct - 1][noteID - 1 + branchDiscrepancy];
                    break;
                case "b":
                    curNote = noteArray[curAct - 1][noteID + branchDiscrepancy];
                    break;
            }
            branchDiscrepancy++;
        }
        // During branch:
        else if (branchSuffix != null && onBranch)
        {
            Debug.Log("During Branch.");
            switch (branchSuffix)
            {
                case "a":
                    curNote = noteArray[curAct - 1][noteID - 1 + branchDiscrepancy];
                    break;
                case "b":
                    curNote = noteArray[curAct - 1][noteID + branchDiscrepancy];
                    break;
            }
            branchDiscrepancy++;
        }
        // At branch end:
        else if (branchSuffix == null && onBranch)
        {
            Debug.Log("Branch End.");
            onBranch = false;
            curNote = noteArray[curAct - 1][noteID - 1 + branchDiscrepancy];
        }
        else
        {
            Debug.Log("Normal Note.");
            curNote = noteArray[curAct - 1][noteID - 1 + branchDiscrepancy];
        }
        Debug.Log("Update done: " + curNoteName);
    }

    // Note flies in from left
    public void GetNote(string noteID)
    {
        // Finds the corresponding note with the correct ID and brings it to center
        for (int i = 0; i < notes.transform.GetChild(curAct - 1).childCount; i++)
        {
            if (notes.transform.GetChild(curAct - 1).GetChild(i).gameObject.name == noteID)
            {
                Debug.Log("Getting Note: " + noteID + " at index " + (curAct - 1) + ", " + i);
                StartCoroutine(MoveToCenter(curAct - 1, i, false));
            }
            else if (notes.transform.GetChild(curAct - 1).GetChild(i).gameObject.name == noteID + "a")
            {
                Debug.Log("Getting Note: " + noteID + " at index " + (curAct - 1) + ", " + i);
                StartCoroutine(MoveToCenter(curAct - 1, i, true));
            }
            else if (notes.transform.GetChild(curAct - 1).GetChild(i).gameObject.name == noteID + "b")
            {
                Debug.Log("Getting Note: " + noteID + " at index " + (curAct - 1) + ", " + i);
                StartCoroutine(MoveToCenter(curAct - 1, i, true));
            }
        }

        // Changes background to witness and back on cue
        if (curNote.transform.GetComponentInChildren<CanvasScript>().witnessState != null)
        {
            switch (curNote.transform.GetComponentInChildren<CanvasScript>().witnessState)
            {
                case "WE":
                    ChangeBackgroundTo("Witness1");
                    break;
                case "WL":
                    ChangeBackgroundTo("GameBackground");
                    break;
            }
        }
    }

    IEnumerator MoveToCenter(int actIndex, int i, bool branch)
    {
		yield return StartCoroutine(HOTween.To(notes.transform.GetChild(actIndex).GetChild(i).transform, 0.8f, "position", new Vector3(0, 1330, -400), false).WaitForCompletion());
    }

    IEnumerator MoveToTray()
    {
        HOTween.To(curNote.transform, 0.15f, "rotation", new Vector3(85, 0, 0), false);
        yield return StartCoroutine(HOTween.To(curNote.transform, 0.15f, "position", new Vector3(85, 1330 + stackHeight, -400), false).WaitForCompletion());
        curNote.GetComponent<PaperScript>().atDestination = true;
        curNoteInMotion = false;
        stackHeight += 0.9f;
    }

    // Send note to tray on desk, increment noteID, and call for new note
    public void ToTray()
    {
        curNote.gameObject.GetComponent<PaperScript>().inTray = true;
		curNote.gameObject.GetComponent<PaperScript>().isClickable = false;
        curNoteInMotion = true;
        curNote.GetComponent<PaperScript>().canvas.sortingLayerName = "Desk Stuff";

        foreach(WordStructure wordStruct in GameObject.FindGameObjectWithTag("TextBox").GetComponent<TextBox>().structList){
			if(wordStruct.noteID == curNoteID && wordStruct.actID == curAct){
				// Adds up submitted weights
			    subtotalWeight += wordStruct.wordWeightCurr.GetValueOrDefault();
			}
		}
		
		//GameObject currNoteCanvas = transform.Find("Notes/" + curNoteName + "/GameCanvas").gameObject;

        if (curNote.transform.GetComponentInChildren<CanvasScript>().submitPaperTo == 'w')
        {
			//addToPastNoteReference(currNoteCanvas);
            stackHeight = 0;
            StartCoroutine(ToWinston());
        }
        else if (curNote.transform.GetComponentInChildren<CanvasScript>().submitPaperTo == 'p')
        {
            //addToPastNoteReference(currNoteCanvas);
            stackHeight = 0;
            StartCoroutine(ToProsecutor());
        }
        else if (curNote.transform.GetComponentInChildren<CanvasScript>().submitPaperTo == 'j')
        {
            //addToPastNoteReference(currNoteCanvas);
            stackHeight = 0;
            StartCoroutine(ToJudge());
        }
        else
        {
			//addToPastNoteReference(currNoteCanvas);
            StartCoroutine(MoveToTray());

            if ((curNoteID != notes.transform.GetChild(curAct - 1).childCount - branchDiscrepancy && totalWeight >= 0) || (curNoteID != notes.transform.GetChild(curAct - 1).childCount - 1 - branchDiscrepancy && totalWeight < 0))
            {
                print("last curNoteID: " + curNoteID + "; curNote: " + curNote.name + "; branchState: " + curNote.transform.GetComponentInChildren<CanvasScript>().branchState);
                if (!curNote.transform.GetComponentInChildren<CanvasScript>().branchState)
                {
                    curNoteID++;
                    UpdateCurNote(curNoteID, null);
                    print("new curNoteID: " + curNoteID + "; curNote: " + curNote.name);
                    GetNote(curNoteName);
                }
                else
                {
                    curNoteID++;
                    if (totalWeight >= 0)
                    {
                        UpdateCurNote(curNoteID, "a");
                    }
                    else if (totalWeight < 0)
                    {
                        UpdateCurNote(curNoteID, "b");
                    }
                    GetNote(curNoteName);
                }
            }
            else
            {
                // Increments act and resets note ID
                curNoteID = 1;
                curAct++;
                branchDiscrepancy = 0;
                ToggleAct(curAct);
                UpdateCurNote(curNoteID, null);
                GameObject.FindGameObjectWithTag("CameraController").GetComponent<PlayCutscene>().Play(curAct);
            }
        }
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
				notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().isClickable = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(244, 1396, -25), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
                curNoteInMotion = false;
            }
        }

        if ((curNoteID != notes.transform.GetChild(curAct - 1).childCount - branchDiscrepancy && totalWeight >= 0) || (curNoteID != notes.transform.GetChild(curAct - 1).childCount - 1 - branchDiscrepancy && totalWeight < 0))
        {
            /*
            if (subtotalWeight >= 5)
            {
                ChangeBackgroundTo("WinstonHappy");
            }
            else if (subtotalWeight <= 5)
            {
                ChangeBackgroundTo("ProsecutorHappy");
            }
            else
            {
                ChangeBackgroundTo("Neutral");
            }*/
            totalWeight += subtotalWeight;
            subtotalWeight = 0;

            if (!curNote.transform.GetComponentInChildren<CanvasScript>().branchState)
            {
                curNoteID++;
                UpdateCurNote(curNoteID, null);
                GetNote(curNoteName);
            }
            else
            {
                curNoteID++;
                if (totalWeight >= 0)
                {
                    UpdateCurNote(curNoteID, "a");
                }
                else if (totalWeight < 0)
                {
                    UpdateCurNote(curNoteID, "b");
                }
                GetNote(curNoteName);
            }
        }
        else
        {
            curNoteID = 1;
            curAct++;
            branchDiscrepancy = 0;
            ToggleAct(curAct);
            UpdateCurNote(curNoteID, null);
            GameObject.FindGameObjectWithTag("CameraController").GetComponent<PlayCutscene>().Play(curAct);
        }
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
				notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().isClickable = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(83, 31, -161), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(-285, 1396, -79), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
                curNoteInMotion = false;
            }
        }

        if ((curNoteID != notes.transform.GetChild(curAct - 1).childCount - branchDiscrepancy && totalWeight >= 0) || (curNoteID != notes.transform.GetChild(curAct - 1).childCount - 1 - branchDiscrepancy && totalWeight < 0))
        {
            /*
            if (subtotalWeight >= 5)
            {
                ChangeBackgroundTo("WinstonHappy");
            }
            else if (subtotalWeight <= 5)
            {
                ChangeBackgroundTo("ProsecutorHappy");
            }
            else
            {
                ChangeBackgroundTo("Neutral");
            }*/
            totalWeight += subtotalWeight;
            subtotalWeight = 0;

            if (!curNote.transform.GetComponentInChildren<CanvasScript>().branchState)
            {
                curNoteID++;
                UpdateCurNote(curNoteID, null);
                GetNote(curNoteName);
            }
            else
            {
                curNoteID++;
                if (totalWeight >= 0)
                {
                    UpdateCurNote(curNoteID, "a");
                }
                else if (totalWeight < 0)
                {
                    UpdateCurNote(curNoteID, "b");
                }
                GetNote(curNoteName);
            }
        }
        else
        {
            curNoteID = 1;
            curAct++;
            branchDiscrepancy = 0;
            ToggleAct(curAct);
            UpdateCurNote(curNoteID, null);
            GameObject.FindGameObjectWithTag("CameraController").GetComponent<PlayCutscene>().Play(curAct);
        }
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
				notes.transform.GetChild(curAct - 1).GetChild(i).GetComponent<PaperScript>().isClickable = false;
                HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "rotation", new Vector3(35, 12, -3), false);
                yield return StartCoroutine(HOTween.To(notes.transform.GetChild(curAct - 1).GetChild(i).transform, 0.2f, "position", new Vector3(114, 1360, -581), false).WaitForCompletion());
                curNote.GetComponent<PaperScript>().atDestination = true;
                curNoteInMotion = false;
            }
        }

        if ((curNoteID != notes.transform.GetChild(curAct - 1).childCount - branchDiscrepancy && totalWeight >= 0) || (curNoteID != notes.transform.GetChild(curAct - 1).childCount - 1 - branchDiscrepancy && totalWeight < 0))
        {
            /*
            if (subtotalWeight >= 5)
            {
                ChangeBackgroundTo("WinstonHappy");
            }
            else if (subtotalWeight <= 5)
            {
                ChangeBackgroundTo("ProsecutorHappy");
            }
            else
            {
                ChangeBackgroundTo("Neutral");
            }*/
            totalWeight += subtotalWeight;
            subtotalWeight = 0;

            if (!curNote.transform.GetComponentInChildren<CanvasScript>().branchState)
            {
                curNoteID++;
                UpdateCurNote(curNoteID, null);
                GetNote(curNoteName);
            }
            else
            {
                curNoteID++;
                if (totalWeight >= 0)
                {
                    UpdateCurNote(curNoteID, "a");
                }
                else if (totalWeight < 0)
                {
                    UpdateCurNote(curNoteID, "b");
                }
                GetNote(curNoteName);
            }
        }
        else
        {
            curNoteID = 1;
            curAct++;
            branchDiscrepancy = 0;
            ToggleAct(curAct);
            UpdateCurNote(curNoteID, null);
            GameObject.FindGameObjectWithTag("CameraController").GetComponent<PlayCutscene>().Play(curAct);
        }
    }
	
	// void addToPastNoteReference(GameObject currNoteCanvas){
		// storySoFar += currNoteCanvas.GetComponent<CanvasScript>().noteContent;
		// print(storySoFar);
	// }
}
