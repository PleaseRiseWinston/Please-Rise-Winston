using UnityEngine;
using System.Collections;

public class pleaseLeavePaper : MonoBehaviour {
	public static bool clickedBG = false;

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown(){
		clickedBG = true;
		screenOverlay.onScreen = true;
	}
}
