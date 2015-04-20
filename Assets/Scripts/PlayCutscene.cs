using System;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayCutscene : MonoBehaviour {

    public Camera gameCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    public GameObject overlayCanvas;
    public CutsceneOverlay cutsceneOverlay;

    public GameObject gameController;
    public GameController gameControllerScript;

    public Vector3 cutsceneCamPos;
    public float cutsceneTime;

    public bool playingCutscene = false;

    private Vector3[] camPoints;
    private Vector3 direction;

    public AudioClip cutsceneMusic;
	
    void Start() {
        gameCamera.enabled = false;
        mainCamera.enabled = true;
        cutsceneCamera.enabled = false;

        overlayCanvas = GameObject.FindGameObjectWithTag("CutsceneOverlay");
        cutsceneOverlay = overlayCanvas.GetComponent<CutsceneOverlay>();

        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();

        cutsceneTime = .5f;
    }

	void Update () {
        cutsceneCamPos = cutsceneCamera.transform.position;
	}

    public IEnumerator Play(int actNumber)
    {
        playingCutscene = true;
        //Debug.Log(actNumber);

        gameObject.AddComponent<AudioSource>();
        AudioSource cutsceneAudio = gameObject.GetComponent<AudioSource>();
        cutsceneAudio.clip = cutsceneMusic;

        SwitchToCutscene();

        StopAllCoroutines();
        cutsceneAudio.Play();
        Transform camTransform = cutsceneCamera.transform;

        switch (actNumber)
        {
            case 1:
                //cutsceneOverlay.RunOverlay(actNumber);
                yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
                HOTween.To(GameObject.Find("Cutscene1-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
                HOTween.To(GameObject.Find("Cutscene1-2").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(100f, 20f, 0), true).WaitForCompletion());
                HOTween.To(GameObject.Find("Cutscene1-3").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(60f, -180f, 0), true).WaitForCompletion());
                yield return StartCoroutine(HOTween.To(GameObject.Find("Cutscene1-4").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false).WaitForCompletion());
                gameControllerScript.GetNote("1.1");
                //print("End Cutscene");
                break;
            case 2:
                gameControllerScript.GetNote("2.1");
                break;
            case 3:
                gameControllerScript.GetNote("3.1");
                break;
            case 4:
                gameControllerScript.GetNote("4.1");
                break;
            case 5:
                gameControllerScript.GetNote("5.1");
                break;
        }

        cutsceneAudio.Stop();
        playingCutscene = false;
        //SwitchToGame();
        Vector3 gameCameraPos = gameCamera.transform.position;
        mainCamera.transform.position = gameCameraPos;
        SwitchToMain();
    }

    // TODO: Add fade sequences at beginning and ends of each of these.

    public void SwitchToMain()
    {
		Destroy(GameObject.Find("MenuPaper_Start"));
		Destroy(GameObject.Find("MenuPaper_Exit"));
        //Debug.Log("Switching to Main");
        mainCamera.tag = "MainCamera";
        mainCamera.enabled = true;
        gameCamera.enabled = false;
        cutsceneCamera.enabled = false;
    }

    public void SwitchToGame()
    {
        Debug.Log("Switching to Game");
        mainCamera.enabled = false;
        gameCamera.tag = "MainCamera";
        gameCamera.enabled = true;
        cutsceneCamera.enabled = false;
    }
    public void SwitchToCutscene()
    {
        //Debug.Log("Switching to Cutscene");
        mainCamera.enabled = false;
        gameCamera.enabled = false;
        cutsceneCamera.tag = "MainCamera";
        cutsceneCamera.enabled = true;
    }
}
