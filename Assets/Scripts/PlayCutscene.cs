using UnityEngine;
using System.Collections;

public class PlayCutscene : MonoBehaviour {

    public Camera mainCamera;
    public Camera cutsceneCamera;
    public float xDirection;
    public float yDirection;
    public float zDirection;

    private bool playingCutscene = false;
    private Vector3 direction;
	
    void Start() {
        mainCamera.enabled = true;
        cutsceneCamera.enabled = false;

        direction = new Vector3(xDirection, yDirection, zDirection);
    }


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
}
