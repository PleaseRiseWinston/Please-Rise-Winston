using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/* 
 * This script is attached to each Paper object.
 * The Paper object will hold positions and focus states of each individual note.
 * All tweening from desktop to focused state is done in here.
*/

public class PaperScript : MonoBehaviour {

    public bool start, exit;

    public Camera gameCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    public TextAsset note;
    public string noteContent;

    public bool focused;
    private bool scrollDown;
    private bool isOffScreen;
    private bool mouseOver;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    private Vector3 cameraFront;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	public static int wordIDNum = 0;

    public AudioClip paper1, paper2, paper3;

    public Canvas canvas;

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        AudioSource audio = gameObject.GetComponent<AudioSource>();

		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
		
        focused = false;

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
        }
        
        defaultNotePos = transform.position;

        // Sets camera default position depending on the intended camera
		if(!start && !exit){
			//Destroy(GameObject.Find("MenuPaper_Start"));
			//Destroy(GameObject.Find("MenuPaper_Exit"));
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

        /*
        text = transform.GetChild(0).gameObject;
        text.transform.position = transform.position + (transform.forward * -0.1f);
        text.transform.rotation = transform.rotation;
        text.transform.localScale = transform.localScale * 0.065f;
        */

        // Instantiates a canvas at the paper's position
        Canvas newCanvas = Instantiate(canvas, transform.position, transform.rotation) as Canvas;
        newCanvas.transform.localScale = transform.localScale;
        newCanvas.transform.SetParent(transform);
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
                audio.clip = paper1;
                break;
            case 2:
                audio.clip = paper2;
                break;
            case 3:
                audio.clip = paper3;
                break;
        }
        //print("Playing Audio...");
        audio.Play();
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
