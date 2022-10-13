using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public bool useKeysInput = true;
	public Vector3 inputAxes = Vector3.zero,buttonCenter = new Vector3(91,0,91);
	PlayerController pC;
	Touch curTouch;

	// Use this for initialization
	void Start () {
		pC = transform.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(useKeysInput)
		inputAxes = new Vector3(Input.GetAxis ("Vertical")*-1,0,Input.GetAxis ("Horizontal"));
		if(pC)
			pC.agent.velocity = inputAxes*pC.agent.speed; 
	}

	public void DefineTouches(int buttonID){
		Debug.Log ("Handling button was activated!");
		inputAxes = new Vector3(Input.mousePosition.x-buttonCenter.x,0,Input.mousePosition.y-buttonCenter.z);
	}
}
