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
        gameControllerScript.curNote.GetComponent<PaperScript>().inTray = true;
        gameControllerScript.ToTray();

        // TODO: Add Weights
    }
}
