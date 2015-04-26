using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class Tray : MonoBehaviour
{
    public GameObject gameController;
    public GameController gameControllerScript;

	void Start ()
	{
	    gameController = GameObject.FindGameObjectWithTag("GameController");
	    gameControllerScript = gameController.GetComponent<GameController>();
	}

    public void OnMouseDown()
    {
        // Only trays/moves curNote iff paper is unfocused and the last paper has arrived at its destination
        if (!gameControllerScript.curNote.GetComponent<PaperScript>().focused)
        {
            if (gameControllerScript.curNote == gameControllerScript.noteArray[gameControllerScript.curAct - 1][0] || gameControllerScript.noteArray[gameControllerScript.curAct - 1][gameControllerScript.curNoteID - 2].GetComponent<PaperScript>().atDestination || gameControllerScript.noteArray[gameControllerScript.curAct - 1][gameControllerScript.curNoteID - 2].GetComponent<PaperScript>().inTray)
            {
                gameControllerScript.curNote.GetComponent<PaperScript>().inTray = true;
                gameControllerScript.ToTray();
            }
        }

        // TODO: Add Weights
    }
}
