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
    public string branchType;
    public string pathTaken = "";
    public int countA = 0;
    public int countB = 0;

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

        // Delayed coroutine to allow CanvasScript to load before checking for branchTypes
        StartCoroutine(InsertNotes());
	}

    IEnumerator InsertNotes()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < notes.transform.childCount; i++)
        {
            noteArray[i] = new GameObject[notes.transform.GetChild(i).childCount];
            branchA[i] = new GameObject[notes.transform.GetChild(i).childCount];
            branchB[i] = new GameObject[notes.transform.GetChild(i).childCount];
            int countA = 0;
            int countB = 0;

            // Insert notes into the act's array and index noteIDs in order starting from 0
            for (int j = 0; j < notes.transform.GetChild(i).childCount; j++)
            {
                noteArray[i][j] = notes.transform.GetChild(i).GetChild(j).gameObject;
                print(noteArray[i][j].GetComponent<PaperScript>().branchType);
                if (notes.transform.GetChild(i).GetChild(j).GetComponent<PaperScript>().branchType == "a")
                {
                    branchA[i][countA] = notes.transform.GetChild(i).GetChild(j).gameObject;
                    //print(branchA[i][counterA].name);
                    countA++;
                }
                if (notes.transform.GetChild(i).GetChild(j).GetComponent<PaperScript>().branchType == "b")
                {
                    branchB[i][countB] = notes.transform.GetChild(i).GetChild(j).gameObject;
                    //print(branchB[i][counterB].name);
                    countB++;
                }
            }

            ToggleAct(curAct);
            UpdateCurNote(curNoteID, null);
        }
    }

    public void Update()
    {
        // 'A' submits curNote; 'S' jumps to note 1.59
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            curNoteID = 69;
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
        // If curNote is not the last note in the act:
        if (branchSuffix != "final")
        {
            if (branchSuffix == null || branchSuffix == "")
            {
                // Get note normally
                Debug.Log("Normal Note");
                curNote = noteArray[curAct - 1][noteID - 1];
                curNoteName = curNote.gameObject.name;
            }
            else
            {
                // Step through the A/B note respectively
                if (branchSuffix == "a")
                {
                    curNote = branchA[curAct - 1][noteID - 1];
                    curNoteName = curNote.gameObject.name;
                }
                else if (branchSuffix == "b")
                {
                    curNote = branchB[curAct - 1][noteID - 1];
                    curNoteName = curNote.gameObject.name;
                }
            }
        }
        else
        {
            // Increments act and resets note ID if curNote was the last
            pathTaken = "";
            curNoteID = 1;
            curAct++;
            ToggleAct(curAct);
            curNote = noteArray[curAct - 1][0];
            GetNote(curNote.name);
            GameObject.FindGameObjectWithTag("CameraController").GetComponent<PlayCutscene>().Play(curAct);
        }
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
                StartCoroutine(MoveToCenter(curAct - 1, i));
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

    IEnumerator MoveToCenter(int actIndex, int i)
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
        // Record branchState and branchType for this note
        branchState = curNote.transform.GetComponentInChildren<CanvasScript>().branchState;
        branchType = curNote.transform.GetComponent<PaperScript>().branchType;

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

            print("last curNoteID: " + curNoteID + "; curNote: " + curNote.name + "; branchState: " + curNote.transform.GetComponentInChildren<CanvasScript>().branchState);
            if (!branchState && branchType == "")
            {
                curNoteID++;
                UpdateCurNote(curNoteID, null);
                print("new curNoteID: " + curNoteID + "; curNote: " + curNote.name);
                GetNote(curNoteName);
            }
            // If a path is taken:
            else if (pathTaken != "")
            {
                // Step through A/B notes depending on chosen path
                curNoteID++;
                if (!curNote.GetComponent<PaperScript>().lastInAct)
                {
                    UpdateCurNote(curNoteID, pathTaken);
                    GetNote(curNoteName);
                }
                else
                {
                    UpdateCurNote(curNoteID, "final");
                }
            }
            // If $BRANCH flag is detected in pre-branch note:
            else if (branchState)
            {
                // Zero out curNoteID to begin at beginning of branch array
                curNoteID = 1;
                if (totalWeight >= 0)
                {
                    pathTaken = "a";
                    UpdateCurNote(curNoteID, pathTaken);
                }
                else if (totalWeight < 0)
                {
                    pathTaken = "b";
                    UpdateCurNote(curNoteID, pathTaken);
                }
                GetNote(curNoteName);
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

        print("last curNoteID: " + curNoteID + "; curNote: " + curNote.name + "; branchState: " + curNote.transform.GetComponentInChildren<CanvasScript>().branchState);
        if (!branchState && branchType == "")
        {
            curNoteID++;
            UpdateCurNote(curNoteID, null);
            print("new curNoteID: " + curNoteID + "; curNote: " + curNote.name);
            GetNote(curNoteName);
        }
        // If a path is taken:
        else if (pathTaken != "")
        {
            // Step through A/B notes depending on chosen path
            curNoteID++;
            if (!curNote.GetComponent<PaperScript>().lastInAct)
            {
                UpdateCurNote(curNoteID, pathTaken);
                GetNote(curNoteName);
            }
            else
            {
                UpdateCurNote(curNoteID, "final");
            }
        }
        // If $BRANCH flag is detected in pre-branch note:
        else if (branchState)
        {
            // Zero out curNoteID to begin at beginning of branch array
            curNoteID = 1;
            if (totalWeight >= 0)
            {
                pathTaken = "a";
                UpdateCurNote(curNoteID, pathTaken);
            }
            else if (totalWeight < 0)
            {
                pathTaken = "b";
                UpdateCurNote(curNoteID, pathTaken);
            }
            GetNote(curNoteName);
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

        print("last curNoteID: " + curNoteID + "; curNote: " + curNote.name + "; branchState: " + curNote.transform.GetComponentInChildren<CanvasScript>().branchState);
        if (!branchState && branchType == "")
        {
            curNoteID++;
            UpdateCurNote(curNoteID, null);
            print("new curNoteID: " + curNoteID + "; curNote: " + curNote.name);
            GetNote(curNoteName);
        }
        // If a path is taken:
        else if (pathTaken != "")
        {
            // Step through A/B notes depending on chosen path
            curNoteID++;
            if (!curNote.GetComponent<PaperScript>().lastInAct)
            {
                UpdateCurNote(curNoteID, pathTaken);
                GetNote(curNoteName);
            }
            else
            {
                UpdateCurNote(curNoteID, "final");
            }
        }
        // If $BRANCH flag is detected in pre-branch note:
        else if (branchState)
        {
            // Zero out curNoteID to begin at beginning of branch array
            curNoteID = 1;
            if (totalWeight >= 0)
            {
                pathTaken = "a";
                UpdateCurNote(curNoteID, pathTaken);
            }
            else if (totalWeight < 0)
            {
                pathTaken = "b";
                UpdateCurNote(curNoteID, pathTaken);
            }
            GetNote(curNoteName);
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

        print("last curNoteID: " + curNoteID + "; curNote: " + curNote.name + "; branchState: " + curNote.transform.GetComponentInChildren<CanvasScript>().branchState);
        if (!branchState && branchType == "")
        {
            curNoteID++;
            UpdateCurNote(curNoteID, null);
            print("new curNoteID: " + curNoteID + "; curNote: " + curNote.name);
            GetNote(curNoteName);
        }
        // If a path is taken:
        else if (pathTaken != "")
        {
            // Step through A/B notes depending on chosen path
            curNoteID++;
            if (!curNote.GetComponent<PaperScript>().lastInAct)
            {
                UpdateCurNote(curNoteID, pathTaken);
                GetNote(curNoteName);
            }
            else
            {
                UpdateCurNote(curNoteID, "final");
            }
        }
        // If $BRANCH flag is detected in pre-branch note:
        else if (branchState)
        {
            // Zero out curNoteID to begin at beginning of branch array
            curNoteID = 1;
            if (totalWeight >= 0)
            {
                pathTaken = "a";
                UpdateCurNote(curNoteID, pathTaken);
            }
            else if (totalWeight < 0)
            {
                pathTaken = "b";
                UpdateCurNote(curNoteID, pathTaken);
            }
            GetNote(curNoteName);
        }
    }
	
	// void addToPastNoteReference(GameObject currNoteCanvas){
		// storySoFar += currNoteCanvas.GetComponent<CanvasScript>().noteContent;
		// print(storySoFar);
	// }
}
