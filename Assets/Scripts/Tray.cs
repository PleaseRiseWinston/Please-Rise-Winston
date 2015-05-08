using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class Tray : MonoBehaviour
{
    public GameObject gameController;
    public GameController gameControllerScript;

    public float trayCooldown;

	void Start ()
	{
	    gameController = GameObject.FindGameObjectWithTag("GameController");
	    gameControllerScript = gameController.GetComponent<GameController>();

        trayCooldown = Time.time + 1.5f;
	}

    public void OnMouseDown()
    {
        // Only trays/moves curNote iff paper is unfocused and the last paper has arrived at its destination
        if (!gameControllerScript.curNote.GetComponent<PaperScript>().focused && !gameControllerScript.curNoteInMotion && trayCooldown <= Time.time)
        {
            print("curNote name: " + gameControllerScript.curNote.name);
            gameControllerScript.curNote.GetComponent<PaperScript>().inTray = true;
            gameControllerScript.ToTray();
        }

        // TODO: Add Weights
    }
}
