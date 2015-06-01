using System;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityStandardAssets.ImageEffects;

/* 
 * This script is attached to each Paper object.
 * The Paper object will hold positions and focus states of each individual note.
 * All tweening from desktop to focused state is done in here.
*/

public class PaperScript : MonoBehaviour
{
    //public int noteID;
    //public string noteIDstr;
    public bool start, exit;

    public Camera gameCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    public TextAsset note;
    public string noteContent;

    public bool focused;
    public bool mouseOver;
    public bool inTray;
    public bool atDestination;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    private Vector3 cameraFront;
	
	public static int wordIDNum = 0;

    public AudioClip paper1;
    public AudioClip paper2;
    public AudioClip paper3;

    public Canvas canvas;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	GameObject gameController;
	GameController gameControllerScript;

    public Color defaultColor;
    public Color transparent;
	
	public bool isClickable;

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        AudioSource audio = gameObject.GetComponent<AudioSource>();

		textBox = GameObject.FindGameObjectWithTag("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();

		isClickable = true;
        focused = false;
        inTray = false;
        atDestination = false;
		
		gameController = GameObject.FindGameObjectWithTag("GameController");
		gameControllerScript = gameController.GetComponent<GameController>();

        // If there is no content or file not given, this paper is a menu button. Otherwise, read content from .txt file
        if (start)
        {
            noteContent = "Start";
            defaultNotePos = transform.position;
        }
        else if (exit)
        {
            noteContent = "Exit";
            defaultNotePos = transform.position;
        }
        else
        {
            //noteContent = note.text;
            noteContent = textBoxScript.editString;
            defaultNotePos = new Vector3(0, 1330, -400);
        }

        // Sets camera default position depending on the intended camera
		if(!start && !exit){
			//print("start and exit is false: " + noteContent);
			defaultCameraPos = gameCamera.transform.position;
		}
		else{
			defaultCameraPos = mainCamera.transform.position;
		}

        // Places the camera default position at 10z units in front of camera
        defaultCameraPos += new Vector3(0, 0, 60);
        cameraFront = defaultCameraPos;
        //Debug.Log("defaultCameraPos = " + defaultCameraPos);

        // Instantiates a canvas at the paper's position
        Canvas newCanvas = Instantiate(canvas, transform.position, transform.rotation) as Canvas;
		if(!start && !exit)
        {
			newCanvas.name = "GameCanvas";
		}
        newCanvas.transform.localScale = transform.localScale;
        newCanvas.transform.SetParent(transform);
    }

    public void OnMouseDown()
    {
        //Debug.Log("Focusing");
        if (!inTray && isClickable && !focused)
        {
            StartCoroutine(Focus());
        }
    }

    public void OnMouseEnter()
    {
        // Toggles mouseover state while not in focused mode
        if (focused){
            mouseOver = true;
        }

        if (!focused)
        {
            StopAllCoroutines();
            StartCoroutine(Glow());
        }
    }

    public void OnMouseExit()
    {
        // Toggles mouseover state while not in focused mode
        if (focused)
        {
            mouseOver = false;
        }
        else if (!focused)
        {
            StopAllCoroutines();
            StartCoroutine(Unglow());
        }
    }

    IEnumerator Glow()
    {
        if (!focused)
        {
            if (start)
            {
                yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlowStart").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.solid).WaitForCompletion());
            }
            else if (exit)
            {
                yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlowExit").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.solid).WaitForCompletion());
            }
            else if (gameObject == gameControllerScript.curNote)
            {
                yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlow").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.solid).WaitForCompletion());
            }
        }
    }

    IEnumerator Unglow()
    {
        if (start)
        {
            yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlowStart").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.transparent).WaitForCompletion());
        }
        else if (exit)
        {
            yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlowExit").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.transparent).WaitForCompletion());
        }
        else if (gameObject == gameControllerScript.curNote)
        {
            yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("PaperGlow").GetComponent<SpriteRenderer>(), 0.8f, "color", gameControllerScript.transparent).WaitForCompletion());
        }
    }

    public void LateUpdate()
    {
        // Detects clicks off the object
        if (Input.GetMouseButtonDown(0) && !mouseOver && focused && !inTray && !gameControllerScript.overlayActive)
        {
            //Debug.Log("Unfocusing");
            StartCoroutine(Unfocus());
        }
		
		if(noteContent != "Start" && noteContent != "Exit"){
			noteContent = textBoxScript.editString;
		}
    }

    public void ForceUnfocus()
    {
        StartCoroutine(Unfocus());
    }

    void PlayAudio()
    {
        int i = Mathf.Abs(Random.Range(1, 4));
        //Debug.Log(gameObject.name + " " + i);
        switch (i)
        {
            case 1:
                GetComponent<AudioSource>().clip = paper1;
                break;
            case 2:
                GetComponent<AudioSource>().clip = paper2;
                break;
            case 3:
                GetComponent<AudioSource>().clip = paper3;
                break;
        }
        //Debug.Log("Playing Audio...");
        GetComponent<AudioSource>().Play();
    }

    // Coroutine called when focusing onto a note
    IEnumerator Focus()
    {
        //Debug.Log("Focusing");
        StartCoroutine(Unglow());
        PlayAudio();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = true;
        HOTween.To(transform, 0.7f, "rotation", new Vector3(0, 0, 0), false);
        HOTween.To(transform, 0.7f, "position", cameraFront, false);
        HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Bloom>(), 0.7f, "bloomIntensity", 0.5f);
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 0.7f, "blur", 0.2f).WaitForCompletion());
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = false;
        focused = true;
    }

    // Coroutine called when unfocusing away from a note
    IEnumerator Unfocus()
    {
        //Debug.Log("Unfocusing");
        PlayAudio();
        focused = false;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = true;
        HOTween.To(transform, 0.7f, "position", defaultNotePos, false); HOTween.To(transform, 0.7f, "rotation", new Vector3(80, 0, 0), false);
        HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Bloom>(), 0.7f, "bloomIntensity", 2.0f);
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 0.7f, "blur", 0.0f).WaitForCompletion());
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = false;
    }
}
