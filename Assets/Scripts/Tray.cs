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
            print("curNote name: " + gameControllerScript.curNote.name);
            gameControllerScript.curNote.GetComponent<PaperScript>().inTray = true;
            gameControllerScript.ToTray();
        }

        // TODO: Add Weights
    }
}
