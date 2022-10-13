using UnityEngine;
using System.Collections;

public class Signalization : MonoBehaviour {
	public LayerMask detectionMask = new LayerMask();
	public string eventType = "sound";
	public string[] detectionFilter = new string[]{"Player"};
	public string[] npcDeactivationFilter = new string[]{"SecurityGuard","SecurityGuard 1"};
	public string[] npcDeactivationFilter2 = new string[]{"Player"};
	public string activationItem = "";
	public float beamLength = 3f,signalSoundRadius = 10f,laserOnTime = 1f,laserOffTime = 1f;
	public bool signalizationEnabled = false,signalizationON = true;
	AudioSource audioSource;
	RaycastHit hitInfo;
	Transform thisTransform;
	public Transform laserBeamTransform;
	float laserStateTime = 0f;
	bool laserEnabled = true,beamRenderDeactivated = false;
	WorldInfo.GameEvent soundEvent = new WorldInfo.GameEvent();

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		if(thisTransform.childCount>0 && laserBeamTransform == null)
			laserBeamTransform = thisTransform.GetChild (0);
		audioSource = transform.GetComponent<AudioSource>();
		soundEvent = new WorldInfo.GameEvent(eventType,signalSoundRadius,0f,thisTransform.position,thisTransform.name);
	}
	
	// Update is called once per frame
	void Update () {
		if(signalizationON == false)
			return;
		if(signalizationEnabled == false){
			if(Time.time<laserStateTime){
				if(laserEnabled){
					ActivateLaserBeam();
					CheckLaserBeam();
				}else
					DeactivateLaserBeam();
			}else{
				laserStateTime = Time.time;
				if(laserEnabled){
					laserEnabled = false;
					laserStateTime+=laserOffTime;
				}else{
					laserEnabled = true;
					laserStateTime+=laserOnTime;
				}
			}


		}else if(signalizationEnabled){
			Signalize();
		}
	}

	void Signalize(){
		soundEvent.destrTime = Time.time+0.3f;
		WorldInfo.AddEvent(soundEvent,true);
	}


	void CheckLaserBeam(){
		if(Physics.Raycast(thisTransform.position,transform.forward,out hitInfo,beamLength,detectionMask)){
			if(IsInFilter(detectionFilter,hitInfo.collider.name)){
				signalizationEnabled = true;
				LevelInfo.RegisterAlert();
				if(audioSource){
					audioSource.Play();
					Debug.Log (thisTransform.name+": audio source assigned!");
				}
			}
		}

	}


	public void SignalizationTurnON(bool enabled){
		if(enabled == false){
			if(signalizationEnabled){
				signalizationEnabled = false;
				if(audioSource)
					audioSource.Stop();
			}
			beamRenderDeactivated = false;
			DeactivateLaserBeam();
			signalizationON = false;
		}else{
			  signalizationEnabled = false;
			  signalizationON = true;
		}
	}

	void DeactivateLaserBeam(){
		if(beamRenderDeactivated)
			return;
		if(laserBeamTransform){
			laserBeamTransform.gameObject.SetActive(false);
			//Debug.Log ("Laser beam turned off");
			beamRenderDeactivated = true;
		}
	}

	void ActivateLaserBeam(){
		if(beamRenderDeactivated == false)
			return;
		if(laserBeamTransform){
			laserBeamTransform.gameObject.SetActive(true);
			beamRenderDeactivated = false;
		}
	}


	bool IsInFilter(string[] filter,string curName){
		bool result = false;
		if(filter.Length<1){
			Debug.Log (transform.name+": current filter is empty!");
			return result;
		}
		for(int i=0;i<filter.Length;i++){
			if(curName == filter[i]){
				result = true;
				break;
			}
		}
		return result;
	}

	void OnTriggerEnter(Collider c){
		//Debug.Log (thisTransform.name+": siren trigger was activated!");

		if(IsInFilter(npcDeactivationFilter,c.name)){
			signalizationEnabled = false;
			if(audioSource)
				audioSource.Stop();
		}else if(IsInFilter(npcDeactivationFilter2,c.name)){
			bool applyActivationItem = true;
			if(string.IsNullOrEmpty(activationItem) == false){
				if(LevelInfo.ItemPicked(activationItem)){
					applyActivationItem = true;
					//Debug.Log ("Applying key:"+activationItem);
				}else {
					applyActivationItem = false;
					//Debug.Log ("Key '"+activationItem+"' wasn't picked!");
				}
			}
			if(signalizationON && applyActivationItem){
				SignalizationTurnON(false);
			}
		}
	}
}
