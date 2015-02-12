using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;

public class NoteFocus : MonoBehaviour {

    public GameObject focusedNote;
    public Camera mainCamera;

    public bool focused;
    private bool scrollDown;
    private bool isOffScreen;
    private bool mouseOver;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    private Vector3 cameraFront;

    private GameObject text;

    void Start()
    {
        defaultNotePos = transform.position;
        defaultCameraPos = mainCamera.transform.position;

        // Places the camera default position at 10z units in front of camera
        defaultCameraPos += new Vector3(0, 0, 10);
        cameraFront = defaultCameraPos;
        //Debug.Log("defaultCameraPos = " + defaultCameraPos);

        focused = false;
        mouseOver = true;

        text = transform.GetChild(0).gameObject;
        text.transform.position = transform.position + (transform.forward * -0.1f);
        text.transform.rotation = transform.rotation;
        text.transform.localScale = transform.localScale * 0.065f;
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
