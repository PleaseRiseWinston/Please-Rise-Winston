using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class instantiateText : MonoBehaviour {
	public Text textPrefab;
	public Transform putWordsHere;
	public TextAsset asset;
	string assetText;
	string[] testArray = {"Hey", "you"};

	// Use this for initialization
	void Start () {
		assetText = asset.text;
		for(int i = 0; i < testArray.Length; i++){
			Text textInstance;
			textInstance = Instantiate(textPrefab, putWordsHere.position, putWordsHere.rotation) as Text;
			textInstance.transform.parent = GameObject.Find("Canvas").transform;
			textInstance.text = testArray[i];
		}	
	}
}
