using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MenuButton : MonoBehaviour {

    public bool lifted;
    private bool mouseOver;

    private Vector3 defaultNotePos;
    private Vector3 liftedPos;
    private Quaternion defaultNoteRot;

	void Start () {
        defaultNotePos = transform.position;
        liftedPos = defaultNotePos + (2 * transform.forward);
        defaultNoteRot = transform.rotation;
	}

    public void Update()
    {
        // Detects clicks off the object
        if (Input.GetMouseButtonDown(0) && !mouseOver && lifted)
        {
            //Debug.Log("Unfocusing");
            StartCoroutine(Drop());
        }
    }

    public void OnMouseDown()
    {
        //Debug.Log("OnClick");
        StartCoroutine(Lift());
    }

    public void OnMouseOver()
    {
        // Toggles mouseover state while not in focused mode
        if (lifted)
        {
            mouseOver = true;
        }
    }

    public void OnMouseExit()
    {
        // Toggles mouseover state while not in focused mode
        if (lifted)
        {
            mouseOver = false;
        }
    }


    // Coroutine called when focusing onto a note
    IEnumerator Lift()
    {
        //Debug.Log("Focusing");
        yield return StartCoroutine(HOTween.To(transform, 0.5f, "position", liftedPos, false).WaitForCompletion());
        lifted = true;
    }

    // Coroutine called when unfocusing away from a note
    IEnumerator Drop()
    {
        //Debug.Log("Unfocusing");
        lifted = false;
        HOTween.To(transform, 0.7f, "position", defaultNotePos, false);
        yield return StartCoroutine(HOTween.To(transform, 0.5f, "rotation", defaultNoteRot, false).WaitForCompletion());
    }
}
