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

        overlayCanvas = GameObject.Find("CutsceneOverlay");
        cutsceneOverlay = overlayCanvas.GetComponent<CutsceneOverlay>();

        cutsceneTime = .5f;
    }

	void Update () {
        cutsceneCamPos = cutsceneCamera.transform.position;
	}

    public IEnumerator Play()
    {
        playingCutscene = true;
        Debug.Log("Entering Play");

        //cutsceneOverlay.RunOverlay();

        gameObject.AddComponent<AudioSource>();
        AudioSource cutsceneAudio = gameObject.GetComponent<AudioSource>();
        cutsceneAudio.clip = cutsceneMusic;

        SwitchToCutscene();

        StopAllCoroutines();
        cutsceneAudio.Play();
        Transform camTransform = cutsceneCamera.transform;

        yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene1-1").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene1-2").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(100f, 20f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene1-3").GetComponent<SpriteRenderer>(), 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, 0f, "position", new Vector3(70f, -180f, 0), true).WaitForCompletion());

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
        Debug.Log("Switching to Main");
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
        Debug.Log("Switching to Cutscene");
        mainCamera.enabled = false;
        gameCamera.enabled = false;
        cutsceneCamera.tag = "MainCamera";
        cutsceneCamera.enabled = true;
    }
}
