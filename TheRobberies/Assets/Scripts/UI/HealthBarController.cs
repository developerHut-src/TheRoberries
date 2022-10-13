using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
	public RectTransform healthBarLine;
	public SimpleHealthController healthController;
	public float minHealthBarValue = 0f,maxHealthBarValue = 250f;
	public int lastHitPoints = -1000;
	Vector3 tempV3;
	Vector2 tempV2;
	public float pixelsPerHitPoint = 1f,healthBarLength = 250f;

	// Use this for initialization
	void Start () {
		Debug.Log ("Healthbar parameters:"+healthBarLine.sizeDelta);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateHealthBar();
	}

	void UpdateHealthBar(){
		if(healthBarLine == null){
			return;
		}
		if(healthController == null){
			return;
		}
		if(lastHitPoints != healthController.curHitPoints){
			pixelsPerHitPoint = maxHealthBarValue/healthController.maxHitPoints;
			healthBarLength = Mathf.Clamp (pixelsPerHitPoint*healthController.curHitPoints,minHealthBarValue,maxHealthBarValue);
			lastHitPoints = healthController.curHitPoints;
			tempV3 = healthBarLine.anchoredPosition3D;
			tempV3.x = (maxHealthBarValue - healthBarLength)*-0.5f;
			healthBarLine.anchoredPosition3D = tempV3;
			tempV2 = healthBarLine.sizeDelta;
			tempV2.x = healthBarLength;
			healthBarLine.sizeDelta = tempV2;
		}
	}
}
