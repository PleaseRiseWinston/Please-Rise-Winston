using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {
	
	public bool translateable;
	public bool backdrop = false;

	public float alphaStep = 0.05f;
	public float timeLeft = 1.0f;
	
	public Color OnMouseOverColor;
	public Font defaultFont;
	public Font translatedFont;

	//private GameObject lines = GameObject.FindGameObjectsWithTag("Line");
	
	void Start(){
		if (translateable) {
			OnMouseOverColor = Color.yellow;
		}
	}

	void OnMouseOver(){
		if(!backdrop){
			// Drops alpha if current font is defaultFont.
			if(GetComponent<TextMesh>().font == defaultFont){
				fontInvert();
				//fadeOut();
			}
			// Increases alpha if current font is translatedFont.
			else if(GetComponent<TextMesh>().font == translatedFont){
				fontInvert();
				//fadeIn();
			}
		}
		else if (backdrop) {

			//lines.GetComponent<TextMesh>().font = defaultFont;
		}
	}

	void OnMouseExit(){
		Debug.Log ("Fuck this.");

		// Drops alpha if current font is translatedFont.
		if(GetComponent<TextMesh>().font == translatedFont){
			fontInvert();
			//fadeOut();
		}
		// Increases alpha if current font is untranslatedFont.
		else if(GetComponent<TextMesh>().font == defaultFont){
			fontInvert();
			//fadeIn();
		}
	}

	/***Arbitrary Functions***/
	/*
	void fadeIn(){
		Color color = renderer.material.color;
		color.a += alphaStep;
		renderer.material.color = color;
	}
	
	void fadeOut(){
		print (renderer.material.color.a);
		if(renderer.material.color.a > 0.0f){
			Color color = renderer.material.color;
			color.a -= alphaStep;
			renderer.material.color = color;
			print(color.a);
			Debug.Log("over");
		}
		// Inverts translation state at 0 alpha.
		else {
			fontInvert();
		}
	}
	*/
	void fontInvert(){
		if(GetComponent<TextMesh>().font = defaultFont){
			GetComponent<TextMesh>().font = translatedFont;
			//fadeIn();
		}
		else if(GetComponent<TextMesh>().font = translatedFont){
			GetComponent<TextMesh>().font = defaultFont;
			//fadeIn();
		}
	}
}