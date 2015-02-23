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

    public Camera mainCamera;
    public TextAsset note;
    public string noteContent;

    public bool focused;
    private bool scrollDown;
    private bool isOffScreen;
    private bool mouseOver;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    private Vector3 cameraFront;

    //private GameObject text;
    public Canvas canvas;

    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();

        noteContent = note.text;

        defaultNotePos = transform.position;
        defaultCameraPos = mainCamera.transform.position;

        // Places the camera default position at 10z units in front of camera
        defaultCameraPos += new Vector3(0, 0, 10);
        cameraFront = defaultCameraPos;
        //Debug.Log("defaultCameraPos = " + defaultCameraPos);

        focused = false;
        mouseOver = true;

        /*
        text = transform.GetChild(0).gameObject;
        text.transform.position = transform.position + (transform.forward * -0.1f);
        text.transform.rotation = transform.rotation;
        text.transform.localScale = transform.localScale * 0.065f;
        */

        // Instantiates a canvas at the paper's position
        Canvas newCanvas = Instantiate(canvas, transform.position, transform.rotation) as Canvas;
        newCanvas.transform.localScale = transform.localScale * 0.03f;
        newCanvas.transform.SetParent(transform);
    }

    public void OnMouseDown()
    {
        //Debug.Log("OnClick");
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
        //Debug.Log("focused = " + focused + ", mouseOver = " + mouseOver);
        // Detects clicks off the object
        if (Input.GetMouseButtonDown(0) && !mouseOver && focused)
        {
            //Debug.Log("Unfocusing");
            StartCoroutine(Unfocus());
        }
    }

    // Coroutine called when focusing onto a note
    IEnumerator Focus()
    {
        //Debug.Log("Focusing");
        HOTween.To(transform, 0.7f, "rotation", new Vector3(0, 0, 0), false);
        yield return StartCoroutine(HOTween.To(transform, 0.7f, "position", cameraFront, false).WaitForCompletion());
        focused = true;
    }

    // Coroutine called when unfocusing away from a note
    IEnumerator Unfocus()
    {
        //Debug.Log("Unfocusing");
        focused = false;
        HOTween.To(transform, 0.7f, "position", defaultNotePos, false);
        yield return StartCoroutine(HOTween.To(transform, 0.7f, "rotation", new Vector3(80, 0, 0), false).WaitForCompletion());
    }
}
