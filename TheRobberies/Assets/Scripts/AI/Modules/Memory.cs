using UnityEngine;
using System.Collections;

public class Memory : MonoBehaviour {
	public float baseLastTargetMemoryTime = 10f;
	public AIext.Factor lastTargetMemoryRandomFactor = new AIext.Factor(1f,4f);
	public int maxWarningPoints = 40;
	public float warningAreaRadius = 5f;
	public LayerMask warningPointVisibilityCheckingMask = new LayerMask();
	//[HideInInspector]
	public WarningPoint[] warningPoints = new WarningPoint[0];
	public Transform lastTarget;
	public float lastTargetDetectionTime = 0f;

	public float lastTargetMemoryTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(lastTargetMemoryRandomFactor.updateTime < Time.time)
			RandomizeLastTargetMemoryTime();
		CheckLastTarget();
	}
	

	public void SortWarningPoints(){

	}

	public void SetTarget(Transform t, float lastTime){
		lastTarget = t;
		lastTargetDetectionTime = lastTime;
	}

	public void CheckLastTarget(){
		if(lastTarget== null)
			return;
		if((lastTargetDetectionTime+lastTargetMemoryTime)<Time.time)
			lastTarget = null;
	}

	//randomize time during which Ai remembers target
	public void RandomizeLastTargetMemoryTime(){
		lastTargetMemoryTime = baseLastTargetMemoryTime*Random.Range (lastTargetMemoryRandomFactor.value,1f);
		lastTargetMemoryRandomFactor.updateTime = Time.time+lastTargetMemoryRandomFactor.updateInterval;
	}

	public void RegisterWarningPoint(Vector3 point,string type){
		if(maxWarningPoints<1){
			Debug.Log (transform.name+": Can't register warning point, because maxWarningPoints <1!");
			return;
		}
		ArrayList tempArray = new ArrayList();
		//the ID and priority of the existen warning point into which we can add current WP
		int bestWpID = -1,bestWpPriority = 0;
		float curDist = Mathf.Infinity;
		bool warningPointSaved = false;

		if(warningPoints.Length>0){
			for(int i=0;i<warningPoints.Length;i++){
				curDist = Vector3.Distance (warningPoints[i].position,point);
				if(!Physics.Linecast(warningPoints[i].position,point,warningPointVisibilityCheckingMask)){
					if(curDist<=warningAreaRadius){
						if(bestWpID == -1 ||warningPoints[i].GetPriority()>bestWpPriority){
							bestWpID = i;
							bestWpPriority = warningPoints[i].GetPriority();
						}
					}
				}
			}
			if(bestWpID>-1){
				warningPoints[bestWpID].RememberEventType(type);
				warningPointSaved = true;
				Debug.Log (transform.name+": priority of warning point "+bestWpID+" was increased by point:"+point+" ; event type:"+type);
				Debug.Log (transform.name+": current priority of warning point "+bestWpID+":"+warningPoints[bestWpID].GetPriority());
			}
		}
		if(!warningPointSaved){
			foreach(WarningPoint WP in warningPoints){
				tempArray.Add (WP);
			}
			if(tempArray.Count >= maxWarningPoints)
				tempArray[tempArray.Count-1] = new WarningPoint(point,type);
			else tempArray.Add (new WarningPoint(point,type));
			warningPoints = (WarningPoint[]) tempArray.ToArray (typeof(WarningPoint));
			warningPointSaved = true;
			Debug.Log (transform.name+": warning point was created :"+point+" ; event type:"+type);
		}
	}

	//Warning point class
	//=========================================================================
	[System.Serializable]
	public class WarningPoint
	{
		public Vector3 position = Vector3.zero;
		public EventCounter[] eventsTypes = new EventCounter[0];
		public float lastCheckTime = -1f;

		public WarningPoint(Vector3 pos,string newEventType){
			position = pos;
			eventsTypes = new EventCounter[]{new EventCounter(newEventType)};
		}

		public int GetPriority(){
			int result = 0;
			if(eventsTypes.Length<1)
				return result;
			for(int i=0;i<eventsTypes.Length;i++){
				result+=eventsTypes[i].counter;
			}
			return result;
		}

		public void RememberEventType(string eType){
			bool eventSaved = false;
			if(eventsTypes.Length<1){
				eventsTypes = new EventCounter[1]{new EventCounter(eType)};
			}else{
				for(int i=0;i<eventsTypes.Length;i++){
					if(eventsTypes[i].eventType == eType){
						eventsTypes[i].counter++;
						eventSaved = true;
					}
				}
				if(eventSaved == false){
					ArrayList tempArray = new ArrayList();
					foreach( EventCounter eCounter in eventsTypes){
						tempArray.Add (eCounter);
					}
					tempArray.Add (new EventCounter(eType));
					eventsTypes =  (EventCounter[]) tempArray.ToArray(typeof(EventCounter));
				}
			}
		}
	}

	//=========================================================================
	public struct EventCounter
	{
		public string eventType;
		public int counter;

		public EventCounter(string eType){
			eventType = eType;
			counter =1;
		}
	}

}

