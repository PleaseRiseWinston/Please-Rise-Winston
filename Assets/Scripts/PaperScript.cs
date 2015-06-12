﻿using System;
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

    [SerializeField]
    private bool _focused;
    public bool Focused { get { return _focused; } }
    public bool mouseOver;
    public bool inTray;
    public bool atDestination;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    public Vector3 cameraFront;
	
	public static int wordIDNum = 0;

    public AudioClip paper1;
    public AudioClip paper2;
    public AudioClip paper3;

    public string fontFlag;
    public Font winstonFont;
    public Font bookFont;
    public Font prosecutorFont;
    public Font selectedFont;

    public Canvas canvas;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	GameObject gameController;
	GameController gameControllerScript;

    public Color defaultColor;
    public Color transparent;
	
	public bool isClickable;

    private Coroutine glowCoroutine = null;
    private bool _focusing;

    private SpriteRenderer _spriteRenderer;
	public string branchType;
    public bool lastInAct;

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        AudioSource audio = gameObject.GetComponent<AudioSource>();

		textBox = GameObject.FindGameObjectWithTag("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
		isClickable = true;
        inTray = false;
        atDestination = false;
		
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        canvas = Instantiate(canvas, transform.position, transform.rotation) as Canvas;
		if(!start && !exit)
        {
            canvas.name = "GameCanvas";
		}
        canvas.sortingLayerName = "Text";
        canvas.transform.localScale = transform.localScale;
        canvas.transform.SetParent(transform);
    }

    public void OnMouseDown()
    {
        //Debug.Log("Focusing");
        if (!inTray && isClickable && !Focused)
        {
            StartCoroutine(Focus());
        }
    }

    public void OnMouseEnter()
    {
        // Toggles mouseover state while not in focused mode
        if (Focused){
            mouseOver = true;
        }

        if (!Focused && !_focusing)
        {
            if(glowCoroutine != null)
            {
                StopCoroutine(glowCoroutine);
            }
            glowCoroutine = StartCoroutine(Glow());
        }
    }

    public void OnMouseExit()
    {
        // Toggles mouseover state while not in focused mode
        if (Focused)
        {
            mouseOver = false;
        }
        else if (!Focused)
        {
            if (glowCoroutine != null)
            {
                StopCoroutine(glowCoroutine);
            }
            glowCoroutine = StartCoroutine(Unglow());
        }
    }
    
    IEnumerator Glow()
    {
        if (!Focused)
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
        glowCoroutine = null;
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
        glowCoroutine = null;
    }

    public void Update()
    {
        // Detects clicks off the object
        if (Input.GetMouseButtonDown(0) && !mouseOver && Focused && !inTray && !gameControllerScript.overlayActive)
        {
            //Debug.Log("Unfocusing");
            StartCoroutine(Unfocus());
        }
		
		if(noteContent != "Start" && noteContent != "Exit"){
			noteContent = textBoxScript.editString;
		}
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
        // Flag to stop glow from turning back on during animation --BG
        _focusing = true;
        _spriteRenderer.sortingLayerName = "Focused Paper";
        canvas.sortingLayerName = "Focused Text";
        StartCoroutine(Unglow());
        PlayAudio();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = true;
        HOTween.To(transform, 0.7f, "rotation", new Vector3(0, 0, 0), false);
        HOTween.To(transform, 0.7f, "position", cameraFront, false);
        HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Bloom>(), 0.7f, "bloomIntensity", 0.5f);
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 0.7f, "blur", 0.2f).WaitForCompletion());
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = false;
        _focused = true;
        _focusing = false;
    }


    // Coroutine called when unfocusing away from a note
    IEnumerator Unfocus()
    {
        Debug.Log("Unfocusing");
        PlayAudio();
        _focused = false;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = true;
        HOTween.To(transform, 0.7f, "position", defaultNotePos, false); HOTween.To(transform, 0.7f, "rotation", new Vector3(80, 0, 0), false);
        HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Bloom>(), 0.7f, "bloomIntensity", 2.0f);
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 0.7f, "blur", 0.0f).WaitForCompletion());
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().curNoteInMotion = false;
        _spriteRenderer.sortingLayerName = "Desk Stuff";
        canvas.sortingLayerName = "Text";
    }
}
