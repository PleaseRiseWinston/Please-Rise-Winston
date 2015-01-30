/*
 * Requires a sprite that's set up with at least two animations. Let's the animation run for about 10 seconds (or whatever 10.0f is) and then loads 
 * another scene.
 */

using UnityEngine;
using System.Collections;

public class noteZoom : MonoBehaviour {
	private Animator animator;
	bool animationPlaying = false;
	float timeLeft = 10.0f;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animationPlaying == true) {
			//print(timeLeft);
			timeLeft -= Time.deltaTime;

			if(timeLeft <= 0){
				Application.LoadLevel ("workInProgress");
			}

		}
	}

	void OnMouseDown(){
		//print ("hey");

		if (animationPlaying == false) {
			animator.SetInteger("spriteState", 1);
			animationPlaying = true;
		}
		/*else{
			animator.SetInteger("spriteState", 0);
			animationPlaying = false;
		}*/
	}

}
