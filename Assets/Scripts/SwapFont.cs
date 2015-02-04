using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;

public class SwapFont: MonoBehaviour {

	private bool isOver;
	public bool translateable = false;

    public int beforeSize=28;
    public int afterSize=20;

	public Color highlightColor;
	public Font defaultFont;
	public Font translatedFont;

    private Text text;
    private bool isTranslated = false;
    private bool isVisible = true;

	void Start(){
        text = GetComponent<Text>();

		// Sets default alpha to 1.0f.
		Color color = text.color;
		color.a = 1.0f;
		text.color = color;

		// Sets isOver to false to prevent auto-changing.
		//isOver = false;
	}
    /*
	void Update(){
		if (!isOver) {
			untranslate();
		}
		else if (isOver){
			translate();
		}
	}
    */
	public void OnEnter(BaseEventData e){
        //Debug.Log ("Over");
		// Sets isOver to true in order to start Update()'s translation process.
		//isOver = true;
        StopAllCoroutines();
        StartCoroutine(translate());
	}

	public void OnExit(BaseEventData e){
		//Debug.Log ("Exit");
		// Sets isOver to false in order to let Update() revert the font back.
		//isOver = false;
		/*(Color color = renderer.material.color;
		color.a = 1.0f;
		renderer.material.color = color;*/
        StopAllCoroutines();
        StartCoroutine(untranslate());
	}

	/***Arbitrary Functions***/

    IEnumerator translate()
    {
        if (isVisible)
        {
            isVisible = false;
            yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.clear).WaitForCompletion());
        }

        if (!isTranslated)
        {
            text.font = translatedFont;
            // Add corrections after this.
            text.fontSize = afterSize;
            isTranslated = true;
        }

        if (!isVisible)
        {
            isVisible = true;
            if (translateable)
            {
                yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.yellow).WaitForCompletion());
            }
            else
            {
                yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.white).WaitForCompletion());
            }
        }
    }

     IEnumerator untranslate()
     {
         if (isVisible)
         {
             isVisible = false;
             yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.clear).WaitForCompletion());
         }

         if (isTranslated)
         {
             text.font = defaultFont;
             // Add corrections after this.
             text.fontSize = beforeSize;
             isTranslated = false;
         }

         if (!isVisible)
         {
             isVisible = true;
             yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.white).WaitForCompletion());
         }
    }

    /*
	void translate(){
		if(!isTranslated()){
			fadeOut();
		}
		else if(isTranslated()){
			fadeIn();
		}
	}
    
	void untranslate(){
		if(isTranslated()){
			fadeOut();
		}
		else if(!isTranslated()){
			fadeIn();
		}
	}

	void fadeIn(){
		if(renderer.material.color.a < 1.0f ){
			Color color = renderer.material.color;
			color.a += alphaStep;
			renderer.material.color = color;
		}
	}
	
	void fadeOut(){
		print (renderer.material.color.a);
		if(renderer.material.color.a >= 0.0f){
			Color color = renderer.material.color;
			color.a -= alphaStep;
			renderer.material.color = color;
		}
		// Inverts translation state at 0 alpha.
		else {
			fontInvert();
		}
	}

	void fontInvert(){
		Debug.Log ("Inverted");
		if(GetComponent<TextMesh>().font == defaultFont){
			GetComponent<TextMesh>().font = translatedFont;
		}
		else if(GetComponent<TextMesh>().font == translatedFont){
			GetComponent<TextMesh>().font = defaultFont;
		}
	}

	bool isTranslated(){
		if(GetComponent<TextMesh>().font == translatedFont){
			//Debug.Log("is translated");
			return true;
		}
		else{
			return false;
		}
	}
	*/
	
}