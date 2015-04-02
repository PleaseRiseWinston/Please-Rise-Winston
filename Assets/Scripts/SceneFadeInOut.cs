using UnityEngine;
using System.Collections;
//using UnityEditor;

public class SceneFadeInOut : MonoBehaviour {
	public float fadeSpeed = 1.5f;
	private bool sceneStarting = true;
	string currScene;
		
	void Awake(){
		currScene = Application.loadedLevelName;
		GetComponent<GUITexture>().pixelInset = new Rect(0f,0f, Screen.width, Screen.height);
		//print(currScene);
		if(currScene == "Main_menu"){
			GetComponent<GUITexture>().color = Color.clear;
		}
	}
	
	void Update(){
		if(sceneStarting && currScene != "Main_menu"){
			StartScene();
		}
	}
	
	void FadeToClear(){
		GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	void FadeToBlack(){
		GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	void StartScene(){
		FadeToClear();
		
		if(GetComponent<GUITexture>().color.a <= .05f){
			GetComponent<GUITexture>().color = Color.clear;
			GetComponent<GUITexture>().enabled = false;
			sceneStarting = false;
		}
	}
	
	public void EndScene(){		
		GetComponent<GUITexture>().enabled = true;
		FadeToBlack();
		
		if(GetComponent<GUITexture>().color.a >= .75f){
			Application.LoadLevel("noteClickZoom");
		}
	}
}