using System;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class PlayCutscene : MonoBehaviour {

    public Camera gameCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    public GameObject overlayCanvas;
    public CutsceneOverlay cutsceneOverlay;

    public GameObject gameController;
    public GameController gameControllerScript;

    public GameObject cameraController;
    public CameraScript cameraControllerScript;

    public Vector3 cutsceneCamPos;
    public float cutsceneTime;

    public bool playingCutscene = false;

    private Vector3[] camPoints;
    private Vector3 direction;

    public AudioClip cutsceneMusic;

    public bool skipCutscene;
	
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

        yield return new WaitForSeconds(2.5f);
        cutsceneAudio.Play();
        yield return new WaitForSeconds(2.5f);
        Transform camTransform = cutsceneCamera.transform;

        switch (actNumber)
        {
            case 1:
                if (!skipCutscene)
                {
                    cutsceneOverlay.RunOverlay(actNumber);
                    
                    yield return StartCoroutine(HOTween.To(camTransform, 20f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene1-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 10f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene1-2").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 10f, "position", new Vector3(100f, 20f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene1-3").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 17f, "position", new Vector3(60f, -180f, 0), true).WaitForCompletion());
                }

                cutsceneAudio.Stop();
                playingCutscene = false;
                SwitchToGame();

                yield return new WaitForSeconds(5);
                //GameObject.FindGameObjectWithTag("BlackGame").SetActive(false);
                gameControllerScript.UpdateCurNote(1, null);
                gameControllerScript.GetNote("1.1");
                //gameControllerScript.GetNote("1." + GameObject.FindGameObjectWithTag("Notes").transform.GetChild(gameControllerScript.curAct - 1).childCount);
                print("End Cutscene");
                break;
            case 2:
                if (!skipCutscene)
                {
                    cutsceneOverlay.RunOverlay(actNumber);

                    yield return StartCoroutine(HOTween.To(camTransform, 15f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene2-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 20f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene2-2").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 20f, "position", new Vector3(100f, 20f, 0), true).WaitForCompletion());
                }

                cutsceneAudio.Stop();
                playingCutscene = false;
                SwitchToGame();

                yield return new WaitForSeconds(5);

                gameControllerScript.UpdateCurNote(2, null);
                Debug.Log("Starting Act 2");
                gameControllerScript.GetNote("2.1");
                break;
            case 3:
                if (!skipCutscene)
                {
                    cutsceneOverlay.RunOverlay(actNumber);

                    yield return StartCoroutine(HOTween.To(camTransform, 15f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene3-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 17f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
                }

                cutsceneAudio.Stop();
                playingCutscene = false;
                SwitchToGame();

                yield return new WaitForSeconds(5);

                gameControllerScript.UpdateCurNote(3, null);
                Debug.Log("Starting Act 3");
                gameControllerScript.GetNote("3.1");
                break;
            case 4:
                if (!skipCutscene)
                {
                    cutsceneOverlay.RunOverlay(actNumber);

                    yield return StartCoroutine(HOTween.To(camTransform, 25f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
                    HOTween.To(GameObject.Find("Cutscene4-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
                    yield return StartCoroutine(HOTween.To(camTransform, 25f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
                }

                cutsceneAudio.Stop();
                playingCutscene = false;
                SwitchToGame();

                yield return new WaitForSeconds(5);

                gameControllerScript.UpdateCurNote(4, null);
                Debug.Log("Starting Act 4");
                gameControllerScript.GetNote("4.1");
                break;
        }

        GameObject.Find("MenuPaper_Start").SetActive(false);
        GameObject.Find("MenuPaper_Exit").SetActive(false);
    }

    // TODO: Add fade sequences at beginning and ends of each of these.

    public void SwitchToMain()
    {
        Debug.Log("Switching to Main");
        StartCoroutine(FadeOut("main"));
    }

    public void SwitchToGame()
    {
        Debug.Log("Switching to Game");
        StartCoroutine(FadeOut("game"));
    }
    public void SwitchToCutscene()
    {
        Debug.Log("Switching to Cutscene");
        StartCoroutine(FadeOut("cutscene"));
    }

    IEnumerator FadeOut(string target)
    {
        // Fades in corresponding black screens
        switch (GameObject.FindGameObjectWithTag("MainCamera").name)
        {
            case "MainCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackMain").GetComponent<SpriteRenderer>(), 2f, "color", new Color(0, 0, 0, 1));
                break;
            case "GameCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackGame").GetComponent<SpriteRenderer>(), 2f, "color", new Color(0, 0, 0, 1));
                break;
            case "CutsceneCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackCutscene").GetComponent<SpriteRenderer>(), 2f, "color", new Color(0, 0, 0, 1));
                break;
        }

        // Increase vignette value of that camera
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 2.5f, "intensity", 7.0f).WaitForCompletion());
        yield return new WaitForSeconds(1f);

        // Swaps cameras by reassigning tags and enable booleans
        switch (target)
        {
            case "main":
                mainCamera.tag = "MainCamera";
                mainCamera.enabled = true;
                gameCamera.tag = "GameCamera";
                gameCamera.enabled = false;
                cutsceneCamera.tag = "CutsceneCamera";
                cutsceneCamera.enabled = false;
                break;
            case "game":
                mainCamera.tag = "MenuCamera";
                mainCamera.enabled = false;
                gameCamera.tag = "MainCamera";
                gameCamera.enabled = true;
                cutsceneCamera.tag = "CutsceneCamera";
                cutsceneCamera.enabled = false;
                break;
            case "cutscene":
                mainCamera.tag = "MenuCamera";
                mainCamera.enabled = false;
                gameCamera.tag = "GameCamera";
                gameCamera.enabled = false;
                cutsceneCamera.tag = "MainCamera";
                cutsceneCamera.enabled = true;
                break;
        }

        // Fade out corresponding black screens
        switch (GameObject.FindGameObjectWithTag("MainCamera").name)
        {
            case "MainCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackMain").GetComponent<SpriteRenderer>().GetComponent<SpriteRenderer>(), 2f, "color", Color.clear);
                break;
            case "GameCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackGame").GetComponent<SpriteRenderer>().GetComponent<SpriteRenderer>(), 2f, "color", Color.clear);
                break;
            case "CutsceneCamera":
                HOTween.To(GameObject.FindGameObjectWithTag("BlackCutscene").GetComponent<SpriteRenderer>().GetComponent<SpriteRenderer>(), 2f, "color", Color.clear);
                break;
        }

        // Returns vignette effect to default
        yield return StartCoroutine(HOTween.To(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteAndChromaticAberration>(), 2.5f, "intensity", 1.5f).WaitForCompletion());
    }
}
