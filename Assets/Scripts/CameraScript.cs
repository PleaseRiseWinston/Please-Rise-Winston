using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityStandardAssets.ImageEffects;

public class CameraScript : MonoBehaviour {

    private GameObject gameController;
    private GameController gameControllerScript;

    public float fadeTime;
    public float defaultVignette;
    public float fadeVignette;

	void Awake () {
        gameController = GameObject.FindGameObjectWithTag("GameController");
	    gameControllerScript = gameController.GetComponent<GameController>();
	}

    void Start(){
        fadeTime = 1.2f;
        fadeVignette = 7.0f;

        if (transform.tag == "MainCamera")
        {
            defaultVignette = 1.5f;
        }
        else if (transform.tag == "GameCamera" || transform.tag == "CutsceneCamera")
        {
            defaultVignette = 7.0f;
        }
    }
    /*
    void Update()
    {
        if (gameControllerScript.curNote.GetComponent<PaperScript>().focused)
        {
            
        }
    }
     */
}
