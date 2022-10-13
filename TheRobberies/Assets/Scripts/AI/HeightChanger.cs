using UnityEngine;
using System.Collections;

public class HeightChanger : MonoBehaviour {
	 float baseY,targetY,nextUpdateTime,speed;
	public float maxY = 4f,minY = -0.5f,minSpeed = 0.4f,maxSpeed = 0.4f,updateYIntervalMax = 6f,updateYIntervalMin = 2f;
	Vector3 newPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		baseY = transform.localPosition.y;
		targetY = baseY;
	}
	
	// Update is called once per frame
	void Update () {
		newPos = transform.localPosition;
		newPos.y = Mathf.Lerp(newPos.y,targetY,Time.deltaTime*speed);
		transform.localPosition = newPos;
		if(Time.time<nextUpdateTime)
			return;
		targetY = Random.Range(minY,maxY);
		targetY = Mathf.Clamp (targetY,baseY+minY,baseY+maxY);
		speed = Random.Range (minSpeed,maxSpeed);
		nextUpdateTime = Time.time+Random.Range (updateYIntervalMin,updateYIntervalMax);
	}
}
