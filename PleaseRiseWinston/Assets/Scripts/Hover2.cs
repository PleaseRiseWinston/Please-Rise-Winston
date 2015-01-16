using UnityEngine;
using System.Collections;

public class Hover2 : MonoBehaviour {

	public GameObject transLine;
	public Material textMesh;

	private float textSpeed = 2.0f;
	private float timer = 0.0f;
	public float OnMouseOverAlpha = 1.0f;
	public float OnMouseExitAlpha = 0.0f;

	void OnMouseOver(){
		//GetComponent<TextMesh>().color.a = OnMouseOverAlpha;
	}
	
	void OnMouseExit(){
		//GetComponent<TextMesh>().color.a = OnMouseExitAlpha;
	}

	void Update(){
		timer += Time.deltaTime;
		renderer.material.color.a += textSpeed * Time.deltaTime;
	}
}