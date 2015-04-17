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

    void OnMouseDown()
    {
        gameControllerScript.ToTray();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
