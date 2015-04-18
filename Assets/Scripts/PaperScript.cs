using System;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/* 
 * This script is attached to each Paper object.
 * The Paper object will hold positions and focus states of each individual note.
 * All tweening from desktop to focused state is done in here.
*/

public class PaperScript : MonoBehaviour
{
    public int noteID;
    public string noteIDstr;
    public bool start, exit;

    public Camera gameCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    public TextAsset note;
    public string noteContent;

    public bool focused;
    public bool mouseOver;
    public bool inTray;

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

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        AudioSource audio = gameObject.GetComponent<AudioSource>();

		textBox = GameObject.FindGameObjectWithTag("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();

        // If there is no content or file not given, this paper is a menu button. Otherwise, read content from .txt file. 
        if (start)
        {
            noteContent = "Start";
        }
        else if (exit)
        {
            noteContent = "Exit";
        }
        else
        {
            //noteContent = note.text;
            noteContent = textBoxScript.editString;

            focused = false;
            inTray = false;
        }
        
        defaultNotePos = transform.position;

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
		if(!start && !exit){
			newCanvas.name = "GameCanvas";
		}
        newCanvas.transform.localScale = transform.localScale;
        newCanvas.transform.SetParent(transform);
    }

    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

    public void OnMouseDown()
    {
        //Debug.Log("Focusing");
        StartCoroutine(Focus());
    }

    public void OnMouseOver()
    {
        // Toggles mouseover state while not in focused mode
        if (focused)
        {
            mouseOver = true;
        }
    }

    public void OnMouseExit()
    {
        // Toggles mouseover state while not in focused mode
        if (focused)
        {
            mouseOver = false;
        }
    }

    public void Update()
    {
        // Detects clicks off the object
        if (Input.GetMouseButtonDown(0) && !mouseOver && focused)
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
        int i = Mathf.Abs(Random.Range(1, 3));
        //print(gameObject.name + " " + i);
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
        //print("Playing Audio...");
        GetComponent<AudioSource>().Play();
    }

    // Coroutine called when focusing onto a note
    IEnumerator Focus()
    {
        //Debug.Log("Focusing");
        PlayAudio();
        HOTween.To(transform, 0.7f, "rotation", new Vector3(0, 0, 0), false);
        yield return StartCoroutine(HOTween.To(transform, 0.7f, "position", cameraFront, false).WaitForCompletion());
        focused = true;
    }

    // Coroutine called when unfocusing away from a note
    IEnumerator Unfocus()
    {
        //Debug.Log("Unfocusing");
        PlayAudio();
        focused = false;
        HOTween.To(transform, 0.7f, "position", defaultNotePos, false);
        yield return StartCoroutine(HOTween.To(transform, 0.7f, "rotation", new Vector3(80, 0, 0), false).WaitForCompletion());
    }
}
