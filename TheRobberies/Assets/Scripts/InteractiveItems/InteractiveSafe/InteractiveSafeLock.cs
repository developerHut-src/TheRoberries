using UnityEngine;
using System.Collections;

public class InteractiveSafeLock : MonoBehaviour {
	public float lockRotationSpeed = 3f;
	public int curRotationDir = 0,startRotDir = -1,curNumber = 0,curNumberID = 0;
	public int[] safeCode = new int[]{3,9,2,5};
	Transform lockPivot;
	float desiredAngle = Mathf.Infinity;
	bool canRotateLock = true;
	Quaternion targetRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		lockPivot = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		if(lockPivot == null)
			return;
		if(Input.GetAxis("Horizontal")>0){
			curRotationDir = 1;
		}else if(Input.GetAxis("Horizontal")>0){
			curRotationDir = -1;
		}
	
	}

	void RotateLock(){
		canRotateLock = false;
		if(desiredAngle == Mathf.Infinity){
			desiredAngle = lockPivot.localEulerAngles.y;
			desiredAngle +=35*curRotationDir;
			targetRotation.eulerAngles = new Vector3(lockPivot.localEulerAngles.x,desiredAngle,lockPivot.localEulerAngles.z);
		}else{
			lockPivot.localRotation = Quaternion.RotateTowards(lockPivot.localRotation,targetRotation,Time.deltaTime*lockRotationSpeed);
			if(Quaternion.Angle(lockPivot.localRotation,targetRotation)<1.5f){
				desiredAngle = Mathf.Infinity;
				canRotateLock = true;
			}
		}
	}
}
