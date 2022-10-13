using UnityEngine;
using System.Collections;

public class Ears : MonoBehaviour {
	public float baseHearingRadius = 7f,earsUpdateInterval = 0.2f;
	//randomize hearing radius as follows: baseHearingRadius*Random.Range(randomFactor,1f);
	public float randomFactor = 1f;
	//time intervals for random update of the hearing
	public float minHearingRandomizeTime = 5f,maxHearingRandomizeTime = 40f;
	public string[] searchForEvents = new string[]{"sound"};
	float updateEarsTime = 0f,updateRandomFactorTime = -1f,hearingRadius = 0f;
	public WorldInfo.GameEvent[] soundEvents = new WorldInfo.GameEvent[0];
	ArrayList tempArray = new ArrayList();
	Transform thisTransform;

	// Use this for initialization
	void Awake () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		RandomizeHearingSensitivity();
		if(Time.time>=updateEarsTime)
			UpdateHearing();
	}


	void UpdateHearing(){
		soundEvents = new WorldInfo.GameEvent[0];
		if(WorldInfo.events.Length<1){
			//Debug.Log (transform.name+":There is no one event in the game world.");
			return;
		}
		updateEarsTime = Time.time+earsUpdateInterval;
		tempArray = new ArrayList();
		foreach(WorldInfo.GameEvent e in WorldInfo.events){
			foreach(string eType in searchForEvents){
				if(e.type == eType){
					if(Vector3.Distance (thisTransform.position,e.position)<=(e.radius+hearingRadius)){
						tempArray.Add (e);
						//Debug.Log (thisTransform.name+": i heard "+e.type+",event time :"+e.destrTime+" cur Time:"+Time.time);
					}
				}
			}
		}
		soundEvents = (WorldInfo.GameEvent[]) tempArray.ToArray(typeof(WorldInfo.GameEvent));
	}


	void RandomizeHearingSensitivity(){
		if(Time.time<updateRandomFactorTime)
			return;
		updateRandomFactorTime = Time.time+Random.Range (minHearingRandomizeTime,maxHearingRandomizeTime);
		hearingRadius = baseHearingRadius*Random.Range(randomFactor,1f);
	}



	public Vector3 GetClosestEventPosition(){
		Vector3 result = Vector3.zero;
		if(soundEvents.Length<1)
			return result;
		float minDist = Mathf.Infinity,curDist;
		int eventId = -1;
		for(int i=0;i<soundEvents.Length;i++){
			curDist = Vector3.Distance(thisTransform.position,soundEvents[i].position);
			if(curDist<minDist){
				eventId = i;
				minDist = curDist;
			}
		}
		if(eventId>-1)
			result = soundEvents[eventId].position;
		return result;
	}
}
