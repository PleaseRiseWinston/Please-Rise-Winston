using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;

public class NoteFocus : MonoBehaviour {

    public GameObject focusedNote;
    public Camera mainCamera;

    private bool scrollDown;
    private bool isOffScreen;
    private bool mouseOver;
    private bool focused;

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;
    private Vector3 cameraFront;

    void Start()
    {
        defaultNotePos = transform.position;
        defaultCameraPos = mainCamera.transform.position;
        defaultCameraPos += new Vector3(0, 0, 10);
        cameraFront = defaultCameraPos;
        Debug.Log("defaultCameraPos = " + defaultCameraPos);

        focused = false;
        mouseOver = true;
    }
    
    public void OnMouseDown()
    {
        Debug.Log("OnClick");
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
        if (Input.GetMouseButtonDown(0) && !mouseOver)
        {
            Debug.Log("OffClick");
            StartCoroutine(Unfocus());
        }
    }
	
    // Coroutine called when focusing onto a note
    IEnumerator Focus()
    {
        yield return StartCoroutine(HOTween.To(transform, 0.8f, "rotation", new Vector3(0, 0, 0), false).WaitForCompletion());
        yield return StartCoroutine(HOTween.To(transform, 0.6f, "position", cameraFront, false).WaitForCompletion());
        focused = true;
    }

    // Coroutine called when unfocusing away from a note
    IEnumerator Unfocus()
    {
        focused = false;
        yield return StartCoroutine(HOTween.To(transform, 0.6f, "position", defaultNotePos, false).WaitForCompletion());
        yield return StartCoroutine(HOTween.To(transform, 0.8f, "rotation", new Vector3(80, 0, 0), false).WaitForCompletion());
    }
}
