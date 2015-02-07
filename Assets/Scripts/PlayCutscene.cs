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


    void onMouseDown()
    {

    }

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            playingCutscene = true;
            mainCamera.enabled = false;
            cutsceneCamera.enabled = true;

        }

        if (playingCutscene)
        {
            cutsceneCamera.transform.position += direction*Time.deltaTime;
        }
	}
}
