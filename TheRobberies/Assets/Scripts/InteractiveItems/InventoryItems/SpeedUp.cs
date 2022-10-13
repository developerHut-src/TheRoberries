using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour {
	public float turboSpeed = 5f,effectTime = 10f;
	float effectEndTime = -1f;
	PlayerController pC;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(pC == null){
			pC = transform.parent.GetComponent<PlayerController>();
			if(pC == null)
				return;
		}
		if(effectEndTime<0)
			effectEndTime = Time.time+effectTime;
		else if(effectEndTime>Time.time){
			pC.curSpeed = turboSpeed;
		}else{
			pC.curSpeed = pC.movementSpeed;
			Destroy (gameObject);
		}
	}
}
