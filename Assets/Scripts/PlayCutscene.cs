using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PlayCutscene : MonoBehaviour {

    public Camera menuCamera;
    public Camera mainCamera;
    public Camera cutsceneCamera;
    public float xDirection;
    public float yDirection;
    public float zDirection;

    private bool playingCutscene = false;
    private Vector3[] camPoints;
    private Vector3 direction;
	
    void Start() {
        menuCamera.enabled = true;
        mainCamera.enabled = false;
        cutsceneCamera.enabled = false;

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
    }

	void Update () {
        // Default camera while out of cutscene
        if (!playingCutscene)
        {
            mainCamera.enabled = true;
            cutsceneCamera.enabled = false;
        }
        // Switch to cutscene camera during cutscenes
        else if (playingCutscene)
        {
            // Set camera start location here if needed
            mainCamera.enabled = false;
            cutsceneCamera.enabled = true;
            cutsceneCamera.transform.position += direction*Time.deltaTime;
        }
	}
    */

    public IEnumerator Play()
    {
        // TODO: Fade to black here

        mainCamera.enabled = false;
        menuCamera.enabled = false;
        cutsceneCamera.enabled = true;

        // Visits each camera point to hit and pauses at each
        foreach (Vector3 point in camPoints)
        {
            StartCoroutine(HOTween.To(cutsceneCamera, 6.0f, "position", point).WaitForCompletion());
            //new WaitForSeconds(2.0f);
        }

        // TODO: Fade to black here

        cutsceneCamera.enabled = false;
        yield return mainCamera.enabled = true;
    }
}
