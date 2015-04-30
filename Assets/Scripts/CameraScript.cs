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
        defaultVignette = 1.5f;
        fadeVignette = 7.0f;
    }
	
	public void FadeIn(){
        StartCoroutine(Darken());
    }

    public void FadeOut()
    {
        StartCoroutine(Lighten());
    }
    /*
    public void Blur()
    {
        StartCoroutine(BlurBackground());
    }*/

    IEnumerator Darken(){
        yield return StartCoroutine(HOTween.To(vignette, fadeTime, "intensity", fadeVignette).WaitForCompletion());
    }

    IEnumerator Lighten(){
        yield return StartCoroutine(HOTween.To(vignette, fadeTime, "intensity", defaultVignette).WaitForCompletion());
    }
}
