using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwapFont: MonoBehaviour {

	private bool isOver;
	public bool translateable;

	public float alphaStep = 0.05f;
	
	public Color OnMouseOverColor;
	public Font defaultFont;
	public Font translatedFont;

	void Start(){
		// Sets default alpha to 1.0f.
		Color color = renderer.material.color;
		color.a = 1.0f;
		renderer.material.color = color;

		// Sets isOver to false to prevent auto-changing.
		isOver = false;
	}

	void Update(){
		if (!isOver) {
			untranslate();
		}
		else if (isOver){
			translate();
		}
	}

	void OnMouseOver(){
		//Debug.Log ("Over");
		// Sets isOver to true in order to start Update()'s translation process.
		isOver = true;
	}

	void OnMouseExit(){
		//Debug.Log ("Exit");
		// Sets isOver to false in order to let Update() revert the font back.
		isOver = false;
		Color color = renderer.material.color;
		color.a = 1.0f;
		renderer.material.color = color;
	}

	/***Arbitrary Functions***/

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
	
	
}