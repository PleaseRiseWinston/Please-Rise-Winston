/*
 * Requires a sprite that's set up with at least two animations. Let's the animation run for about 10 seconds (or whatever 10.0f is) and then loads 
 * another scene. 
 * 
 * This is tied to Jump N Shoot Man. If you click on him, it sets a bool that checks for animation to true. In Update(), if the bool
 * checking for animation is true, it moves Jump N Shoot Man down the screen. Once he is off screen OnBecameInvisible() executes. It starts a timer
 * (which is also being detected on screenOverlay.cs). Once the timer reaches 0, code in screenOverlay.cs executes.
 */

using UnityEngine;
using System.Collections;

public class noteZoom : MonoBehaviour {
	public static bool moveSprite = false;
	bool goingDown = false;
	public static bool offScreen = false;
	
	void Update(){
		
		//move down
		if(moveSprite == true && goingDown == true){
			transform.Translate((4 + 1/2) * Vector3.down * Time.deltaTime * 10, Space.World);
		}
		//move up
		else if(moveSprite == true && goingDown == false && screenOverlay.onScreen == false){
			transform.Translate((4 + 1/2) * Vector3.up * Time.deltaTime * 10, Space.World);	
		}
		
		if(transform.position.y > 0){
			//print("hey");
			moveSprite = false;
			offScreen = false;
		}
		
	}
	
	void OnMouseDown(){
		if(moveSprite == false && goingDown == false){
			moveSprite = true;
			goingDown = true;
		}
	}
	
	void OnBecameInvisible(){
		moveSprite = false;
		goingDown = false;
		offScreen = true;
		screenOverlay.stopAnimation = false;
	}
	
	/*private Animator animator;
	public static bool offScreen = false;
	bool animationPlaying = false;
	bool stopAnimation = false;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animationPlaying == true) {
			transform.Translate((4 + 1/2) * Vector3.down * Time.deltaTime, Space.World);
		}
		
		if(screenOverlay.onScreen == false && stopAnimation == false){
			//print("test");
			transform.Translate((4 + 1/2) * Vector3.up * Time.deltaTime, Space.World);
		}
		
		if(transform.position.y > 0){
			//print("hey");
			stopAnimation = true;
		}
	}

	void OnMouseDown(){
		if (animationPlaying == false) {
			animator.SetInteger("spriteState", 1);
			animationPlaying = true;
			stopAnimation = false;
		}
	}
	
	void OnBecameInvisible(){
		animationPlaying = false;
		offScreen = true;
		animator.SetInteger("spriteState", 0);
	}*/

}
