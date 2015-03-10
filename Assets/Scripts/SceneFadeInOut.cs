using UnityEngine;
using System.Collections;
//using UnityEditor;

public class SceneFadeInOut : MonoBehaviour {
	public float fadeSpeed = 1.5f;
	private bool sceneStarting = true;
	string currScene;
		
	void Awake(){
		currScene = Application.loadedLevelName;
		guiTexture.pixelInset = new Rect(0f,0f, Screen.width, Screen.height);
		//print(currScene);
		if(currScene == "Main_menu"){
			guiTexture.color = Color.clear;
		}
	}
	
	void Update(){
		if(sceneStarting && currScene != "Main_menu"){
			StartScene();
		}
	}
	
	void FadeToClear(){
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	void FadeToBlack(){
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	void StartScene(){
		FadeToClear();
		
		if(guiTexture.color.a <= .05f){
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			sceneStarting = false;
		}
	}
	
	public void EndScene(){		
		guiTexture.enabled = true;
		FadeToBlack();
		
		if(guiTexture.color.a >= .75f){
			Application.LoadLevel("noteClickZoom");
		}
	}
}