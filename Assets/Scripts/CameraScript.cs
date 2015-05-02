using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityStandardAssets.ImageEffects;

public class CameraScript : MonoBehaviour {

    private Bloom bloom;
    private VignetteAndChromaticAberration vignette;

    public float fadeTime;
    public float defaultVignette;
    public float fadeVignette;

	void Awake () {
        bloom = gameObject.GetComponent<Bloom>() as Bloom;
        vignette = gameObject.GetComponent<VignetteAndChromaticAberration>() as VignetteAndChromaticAberration;
	}

    void Start(){
        fadeTime = 1.2f;
        fadeVignette = 7.0f;

        if (transform.tag == "MainCamera")
        {
            defaultVignette = 1.5f;
        }
        else if (transform.tag == "GameCamera" || transform.tag == "CutsceneCamera")
        {
            defaultVignette = 7.0f;
        }

    }
}
