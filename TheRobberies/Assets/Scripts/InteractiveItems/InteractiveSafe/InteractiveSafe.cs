using UnityEngine;
using System.Collections;

public class InteractiveSafe : MonoBehaviour {
	public float lastClickTime = -1f,activationRadius = 1.3f;
	public Transform safeLockTransform,safeLockTransformEasy,safeLockTransformNormal,safeLockTransformHard;
	public bool unlocked = false;
	public string notification = "---",notification1 = "";


	void OnEnable(){
		if(GameInfo.safeLockDifficulty ==0){
			if(safeLockTransformEasy)
				safeLockTransform = safeLockTransformEasy;
			else Debug.Log ("'Easy' transform of lock not assigned!");
		}else if(GameInfo.safeLockDifficulty ==1){
			if(safeLockTransformNormal)
				safeLockTransform = safeLockTransformNormal;
			else Debug.Log ("'Normal' transform of lock not assigned!");
		}else if(GameInfo.safeLockDifficulty ==2){
			if(safeLockTransformHard)
				safeLockTransform = safeLockTransformHard;
			else Debug.Log ("'Hard' transform of lock not assigned!");
		}
	}

	// Use this for initialization
	void Start () {
		LevelInfo.objectivesCount++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Click(ScreenClicker.ClickInfo clickInfo){
		if(ScreenClicker.DoubleClick(lastClickTime,Time.time,clickInfo.doubleClickInterval)){
			if(clickInfo.sender){
				if(Vector3.Distance (clickInfo.sender.position,transform.position)>activationRadius){
				clickInfo.sender.SendMessageUpwards("SetTargetPosition",transform.position,SendMessageOptions.DontRequireReceiver);
				}else{
					clickInfo.sender.SendMessageUpwards("SetTargetPosition",Vector3.zero,SendMessageOptions.DontRequireReceiver);
					if(!unlocked)
					ActivateLockHacking();
				
				}
				LevelInfo.StopShowNotification();
				Debug.Log (transform.name+": i'm was clicked by double click!");
			}
		}else{
				if(clickInfo.sender)
					clickInfo.sender.SendMessageUpwards("SetTargetPosition",Vector3.zero,SendMessageOptions.DontRequireReceiver);
				LevelInfo.ShowNotification(notification,5f);
				Debug.Log (transform.name+": i'm was clicked by single click!");
		
		}
		lastClickTime = Time.time;
	}
	


	void ActivateLockHacking(){
		if(safeLockTransform == null){
			Debug.Log (transform.name+": Can't start lock hacking: safe lock wasn't assigned!");
			return;
		}
		LevelInfo.hackingEnabled = true;
		//if(LevelInfo.worldCameraTransform)
			//safeLockTransform.position = new Vector3(LevelInfo.worldCameraTransform.position.x,safeLockTransform.position.y,LevelInfo.worldCameraTransform.position.z);

		safeLockTransform.gameObject.SetActive (true);
		safeLockTransform.SendMessage ("SetSafeTransform",transform,SendMessageOptions.RequireReceiver);
	}


	void HackingDisabled(bool success){
		LevelInfo.hackingEnabled = false;
		if(success){
			LevelInfo.executedObjectives++;
			SetLockStatus(success);
		}
		safeLockTransform.gameObject.SetActive (false);
	}


	void SetLockStatus(bool unlockedStatus){
		unlocked = unlockedStatus;
	}
}
