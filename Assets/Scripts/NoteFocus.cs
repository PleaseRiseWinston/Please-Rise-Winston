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

    private Vector3 defaultNotePos;
    private Vector3 defaultCameraPos;

    void Start()
    {
        defaultNotePos = transform.position;
        defaultCameraPos = mainCamera.transform.position;
    }

    public void OnMouseDown()
    {
        Debug.Log("OnClick");
        StartCoroutine(ScrollDown());
    }

    IEnumerator ScrollDown()
    {
        yield return StartCoroutine(HOTween.To(transform, 0.2f, "position", new Vector3(0,-10,0), true).WaitForCompletion());
        //transform.position -= new Vector3(0.0f,-0.5f, 0.0f);
    }
}
