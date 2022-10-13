using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour {
	public Transform spectatorCamera;
	Transform curTarget;
	int curTargetIndex = 0,lastTargetIndex = -1;
	bool fpsMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (WorldInfo.players.Length < 1) {
			Debug.Log ("No one player registered, nothing to spectate.");
			return;
				}
		if(!WorldInfo.enableSpectation)
			return;
		DefineTarget ();
		SetTarget ();
		ChangeView ();
	}

	void DefineTarget(){
		if (Input.GetMouseButtonDown (0))
				curTargetIndex++;
		else if(Input.GetMouseButtonDown(1))
		        curTargetIndex --;
		if(curTargetIndex<0)
			curTargetIndex = WorldInfo.players.Length - 1;
		else if(curTargetIndex>WorldInfo.players.Length - 1)
			curTargetIndex = 0;
	}

	void SetTarget(){
		if (curTargetIndex == lastTargetIndex)
						return;
		curTarget = WorldInfo.players [curTargetIndex].pTransform;
		WorldInfo.DisableSpectationForOthers (curTargetIndex);
		if (spectatorCamera)
			spectatorCamera.SendMessage ("SetCameraTarget", curTarget, SendMessageOptions.DontRequireReceiver);
		curTarget.SendMessage("ChangeSpectatorView",fpsMode,SendMessageOptions.DontRequireReceiver);
		curTarget.SendMessage("SetSpectation",true,SendMessageOptions.DontRequireReceiver);
		//Debug.Log ("got new target");
		lastTargetIndex = curTargetIndex;
	}

	void ChangeView(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			AudioListener listener;
						if (fpsMode){
								fpsMode = false;
						if (spectatorCamera){
					if(spectatorCamera.GetComponent<Camera>())
						if(spectatorCamera.GetComponent<Camera>().enabled == false)
							spectatorCamera.GetComponent<Camera>().enabled = true;
					listener = spectatorCamera.GetComponent<AudioListener>();
					if(listener)
						listener.enabled = true;
				}
				}else{
				fpsMode = true;
				if(spectatorCamera){
					if(spectatorCamera.GetComponent<Camera>())
						if(spectatorCamera.GetComponent<Camera>().enabled == true)
							spectatorCamera.GetComponent<Camera>().enabled = false;
					listener = spectatorCamera.GetComponent<AudioListener>();
					if(listener)
						listener.enabled = false;
				}
			}
			curTarget.SendMessage("ChangeSpectatorView",fpsMode,SendMessageOptions.DontRequireReceiver);
		}
	}



	void RenewSpectation(){
		curTarget = null;
		curTargetIndex = 0;
		lastTargetIndex = -1;
	}
}
