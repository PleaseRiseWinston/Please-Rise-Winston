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

    public float cutsceneTime;

    public float xDirection;
    public float yDirection;
    public float zDirection;

    public bool playingCutscene = false;

    private Vector3[] camPoints;
    private Vector3 direction;

    public AudioClip cutsceneMusic;
	
    void Start() {
        gameCamera.enabled = false;
        mainCamera.enabled = true;
        cutsceneCamera.enabled = false;


        cutsceneTime = 6.0f;

        direction = new Vector3(xDirection, yDirection, zDirection);

        // Add other checkpoints for camera to hit here
        camPoints[0] = new Vector3(0, 0, 0);
    }
    /*
    void OnMouseDown()
    {
        // Toggle cutscene state with a click
        Debug.Log("Clicked Button");
        playingCutscene = true;
    }*/

	void Update () {
        Debug.Log(playingCutscene);
        // Default camera while out of cutscene
        if (!playingCutscene)
        {
            SwitchToMain();
        }
        // Switch to cutscene camera during cutscenes
        else if (playingCutscene)
        {
            float timer = Time.deltaTime;
            while (timer < 5)
            {
                SwitchToCutscene();
                cutsceneCamera.transform.position += direction * Time.deltaTime;
            }
            SwitchToGame();
        }
	}

    public IEnumerator Play()
    {
        Debug.Log("Entering Play");
        print(gameObject.name);

        gameObject.AddComponent<AudioSource>();
        AudioSource cutsceneAudio = gameObject.GetComponent<AudioSource>();
        cutsceneAudio.clip = cutsceneMusic;

        SwitchToCutscene();

        StopAllCoroutines();
        cutsceneAudio.Play();
        Transform camTransform = cutsceneCamera.transform;

        HOTween.To(camTransform, cutsceneTime, "position", new Vector3(150f, 100f, 0), true);
        yield return StartCoroutine(HOTween.To(camTransform, cutsceneTime, "position", new Vector3(150f, 100f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene1").GetComponent<MeshRenderer>().material, 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, cutsceneTime, "position", new Vector3(-100f, 50f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene2").GetComponent<MeshRenderer>().material, 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, cutsceneTime, "position", new Vector3(100f, 20f, 0), true).WaitForCompletion());
        HOTween.To(GameObject.Find("Cutscene3").GetComponent<MeshRenderer>().material, 3f, "color", Color.clear, false);
        yield return StartCoroutine(HOTween.To(camTransform, cutsceneTime, "position", new Vector3(50f, -180f, 0), true).WaitForCompletion());

        cutsceneAudio.Stop();
        //SwitchToGame();
        Vector3 gameCameraPos = gameCamera.transform.position;
        mainCamera.transform.position = gameCameraPos;
        SwitchToMain();


        /*
        // Visits each camera point to hit and pauses at each
        foreach (Vector3 point in camPoints)
        {
            StartCoroutine(HOTween.To(cutsceneCamera, 6.0f, "position", point).WaitForCompletion());
            //new WaitForSeconds(2.0f);
        }
        */
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
