using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	Text counterText;

	// Use this for initialization
	void Start () {
		counterText = transform.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
		if(counterText){
			counterText.text = (1f/Time.deltaTime).ToString ();
		}
	}
}
