﻿using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CutsceneOverlay : MonoBehaviour {

    public GameObject cameraController;
    public PlayCutscene playCutscene;

    public GameObject[] overlays;
    private Color transparent;
    private Color solid;

    private float waitTime;

	// Use this for initialization
	void Start () {
        // Reference CameraController
        cameraController = GameObject.Find("CameraController");
        playCutscene = cameraController.GetComponent<PlayCutscene>();

        // Declarations for alpha states
        transparent = new Color(1f, 1f, 1f, 0f);
        solid = new Color(1f, 1f, 1f, 1f);
        waitTime = 2f;

        // Insert all children planes into array and make transparent
        overlays = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            overlays[i] = transform.GetChild(i).gameObject;
            overlays[i].GetComponent<Image>().color = transparent;
            Debug.Log(overlays[i].name);
        }
	}

    public void RunOverlay()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            StartCoroutine(FadeText(overlays[i].transform, 5f * i));
        }
    }

    IEnumerator FadeText(Transform text, float stallTime)
    {
        Debug.Log("Fading: " + text.name);
        yield return new WaitForSeconds(stallTime);
        StartCoroutine(HOTween.To(text.GetComponent<Image>(), 0.5f, "color", solid, false).WaitForCompletion());
        yield return new WaitForSeconds(5f);
        StartCoroutine(HOTween.To(text.GetComponent<Image>(), 0.5f, "color", transparent, false).WaitForCompletion());
    }
}

