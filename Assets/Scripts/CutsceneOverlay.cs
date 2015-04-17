using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Collections;

public class CutsceneOverlay : MonoBehaviour {

    public GameObject cameraController;
    public PlayCutscene playCutscene;

    public GameObject[][] overlays;
    private Color transparent;
    private Color solid;

    private float waitTime;
    private float staggerTime;

	// Use this for initialization
	void Start () {
        // Reference CameraController
        cameraController = GameObject.Find("CameraController");
        playCutscene = cameraController.GetComponent<PlayCutscene>();

        // Declarations for alpha states
        transparent = new Color(1f, 1f, 1f, 0f);
        solid = new Color(1f, 1f, 1f, 1f);
        waitTime = 2f;
	    staggerTime = 5f;

	    overlays = new GameObject[5][];

        // Insert all children planes into array and make transparent
        for (int i = 0; i < transform.childCount; i++)
        {
			//print(transform.childCount);
            //print(transform.GetChild(i).childCount);
            overlays[i] = new GameObject[transform.GetChild(i).childCount];
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                //print(transform.GetChild(i).GetChild(j).gameObject.name);
                overlays[i][j] = transform.GetChild(i).GetChild(j).gameObject;
                overlays[i][j].GetComponent<Image>().color = transparent;
				//print(overlays[i][j].name);
            }
        }
	}

    public void RunOverlay(int actNumber)
    {
        // Staggers each consecutive overlay by 5s each
        for (int i = 0; i < transform.GetChild(actNumber - 1).childCount; i++)
        {
            //Debug.Log("Overlaying " + i);
            StartCoroutine(FadeText(overlays[actNumber - 1][i].transform, staggerTime * i));
        }
    }

    IEnumerator FadeText(Transform text, float stallTime)
    {
        yield return new WaitForSeconds(stallTime);
        //Debug.Log(text.name);
        StartCoroutine(HOTween.To(text.GetComponent<Image>(), 0.5f, "color", solid, false).WaitForCompletion());
        yield return new WaitForSeconds(5f);
        StartCoroutine(HOTween.To(text.GetComponent<Image>(), 0.5f, "color", transparent, false).WaitForCompletion());
    }
}

