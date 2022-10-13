using UnityEngine;
using System.Collections;

public class SkinEventsSender : MonoBehaviour {
	Transform npcTransform;

	// Use this for initialization
	void Start () {
		npcTransform = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendFootstepEvent(){
		if(npcTransform){
			npcTransform.SendMessage ("PlayFootstepSound",SendMessageOptions.DontRequireReceiver);
		}
	}
}
