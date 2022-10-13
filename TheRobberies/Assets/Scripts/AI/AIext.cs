using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AIext : MonoBehaviour {

[System.Serializable]
 public class Factor
 {
		public float value = 1f;
		public float updateInterval = 1f;
		[HideInInspector]
		public float updateTime = 0f;

		public Factor(float newValue,float newUpdateInterval){
			value = newValue;
			updateInterval = newUpdateInterval;
		}

		public void UpdateTime(float curTime){
			updateTime = curTime+updateInterval;
		}

 }

	public static  Vector3 GetClosestPointOnNavMesh(Vector3 destination,float distance){
		Vector3 result = destination;
		if(result == Vector3.zero)
			return result;
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(result,out navMeshHit,distance,1);
		if(navMeshHit.hit){
			result = navMeshHit.position;
		}else result = Vector3.zero;
		return result;
	}
}
