using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class OfficeWorker : MonoBehaviour {
	public float targetStoppingDistance = 1.5f,pointStoppingDistance = 1.5f,rotationSpeed = 5000f,chaseSpeed = 3.5f,walkSpeed = 2.5f;
	Eyes eyes;
	Ears ears;
	Memory memory;
	Transform thisTransform;
	public Transform target,weaponHolder,stateInfoTransform;
	NavMeshAgent agent;
	Vector3 warningPoint = Vector3.zero,lookDir = Vector3.zero;
	Quaternion targetRot = Quaternion.identity;
	public RouteManager.RouteData routeData = new RouteManager.RouteData();
	float curSpeed;
	public string[] routes = new string[0];
	bool enableFiring = false,detectionRegistered = false,targetIsVisible = false;
	//action IDs: 0- follow route, 1- scream,3 - smoke,4 - talk, 5 - spectate
	int actionID = 0;
	float preferedActionTime = -1f;
	CloudStateManager cloudStateManager;
	public WorldInfo.GameEvent detectionEvent;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		agent = thisTransform.GetComponent<NavMeshAgent>();
		eyes = thisTransform.GetComponent<Eyes>();
		ears = thisTransform.GetComponent<Ears>();
		memory = thisTransform.GetComponent<Memory>();
		if(stateInfoTransform)
			cloudStateManager = stateInfoTransform.GetComponent<CloudStateManager>();
		WorldInfo.RegisterPlayer(new WorldInfo.GlobalPlayerInfo(thisTransform,1,true));
		//if(agent)
			//agent.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!agent){
			Debug.Log (thisTransform.name+" : NavMeshAgent is not found!");
			return;
		}
		target = null;
		targetIsVisible = false;
		UpdateEyes ();
		UpdateEars();
		if(target){
			targetIsVisible = true;
		}
		
		if(memory){
			if(!target){
				if(memory.lastTarget)
					target  = memory.lastTarget;
			}else{
				memory.SetTarget(target,Time.time);
			}

		}
		if(!target)
			DisableScream ();

		if(Time.time>preferedActionTime && preferedActionTime>0)
			preferedActionTime = -1f;
		if(preferedActionTime<0){
			if(target){
				actionID = 1;
			}else{
				actionID = 0;
			}
		}
		InititateAction();

		if(cloudStateManager)
			cloudStateManager.playerPosition = new Vector3(thisTransform.position.x,5f,thisTransform.position.z);
	}


	void UpdateEyes(){
		if(eyes == null)
			return;
		target = eyes.GetClosestVisibleCharacter();
	}

	void UpdateEars(){
		if(ears == null)
			return;
		if(warningPoint == Vector3.zero)
			warningPoint = ears.GetClosestEventPosition();
	}


	void InititateAction(){
		if(actionID == 0){
			if(detectionRegistered){
				detectionRegistered = false;
				if(!LevelInfo.gameOver && !LevelInfo.missionComplete)
					LevelInfo.SkipDetection();
			}
			FollowRoute();
		}else if(actionID == 1){
			Scream();
		}else if(actionID == 3){
			Smoke();
		}else if(actionID == 5){
			Spectate ();
		}
	}


	void Scream(){
		if(target == null)
			return;
		if(cloudStateManager){
			cloudStateManager.ShowCloud(0);
			cloudStateManager.HideCloud(1);
		}
		if(routeData.routeID>-1){
			routeData.ResetRoute();
			Debug.Log (thisTransform.name+": Target detected, route was resetted!");
		}
		if(!detectionRegistered){
			detectionRegistered = true;
			if(!LevelInfo.gameOver && !LevelInfo.missionComplete)
				LevelInfo.RegisterDetection();
			Debug.Log(thisTransform.name+": alerts count = "+LevelInfo.alertsCount);
		}

		if(agent.hasPath)
			agent.Stop();
		if(target){
			lookDir = target.position - thisTransform.position;
			agent.updateRotation = false;
			RotateToTarget();
			if(targetIsVisible){
				detectionEvent = new WorldInfo.GameEvent("targetDetected",100f,Time.time+0.3f,target.position,thisTransform.name);
			}else detectionEvent.destrTime = Time.time+0.3f;
			WorldInfo.AddEvent(detectionEvent,true);
		}

		
	}


	void Smoke(){
		if(cloudStateManager){
			cloudStateManager.ShowCloud(1);
		}
		if(agent.hasPath)
			agent.Stop();
	}

	void Spectate(){
		if(cloudStateManager){
			cloudStateManager.HideCloud(0);
			cloudStateManager.HideCloud(1);
			cloudStateManager.ShowCloud(2);
		}
		if(agent.hasPath)
			agent.Stop();
	}


	void DisableScream(){
		if(lookDir!=Vector3.zero)
			lookDir = Vector3.zero;
		if(agent.updateRotation == false)
		agent.updateRotation = true;
	}
	


	void RotateToTarget(){
		if(lookDir == Vector3.zero)
			return;
		targetRot = Quaternion.LookRotation(lookDir);
		thisTransform.localRotation = Quaternion.RotateTowards(thisTransform.localRotation,targetRot,rotationSpeed);
	}


	float MoveToDestination(Vector3 destPoint){
			if(agent.hasPath)
				agent.Resume();
			agent.SetDestination(destPoint);
		return Vector3.Distance (thisTransform.position,destPoint);	
	}


	void FollowRoute(){
		if(cloudStateManager){
			cloudStateManager.HideCloud(0);
			cloudStateManager.HideCloud(1);
		}
		if(routeData.routeID<0){
			if(routes.Length>0){
				routeData.routeID = RouteManager.GetRouteID(routes[Random.Range (0,routes.Length-1)]);
				Debug.Log (thisTransform.name+": selected route: "+routeData.routeID);
			}else
			routeData.routeID = RouteManager.GetRouteID(routeData.routeName);
		}
		if(routeData.routeID<0)
			return;
		if(routeData.waypointPos != Vector3.zero){
			if(agent.speed>walkSpeed)
				agent.speed = walkSpeed;
			agent.SetDestination(routeData.waypointPos);
			if(agent.hasPath)
				agent.Resume();
			if(Vector3.Distance (thisTransform.position,routeData.waypointPos)<pointStoppingDistance)
				routeData.waypointPos = Vector3.zero;
			return;
		}

		RouteManager.GetRouteWaypoint(routeData);

	}
	


	public void ActivateAction(InteractiveZone.ActionInfo newActionInfo){
		actionID = newActionInfo.actionID;
		preferedActionTime = Time.time+newActionInfo.actionDuration;
		Debug.Log (thisTransform.name+": action "+newActionInfo.actionID+" with duration of "+newActionInfo.actionDuration+" seconds has been activated!");
	}
}
