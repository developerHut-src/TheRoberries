using UnityEngine;
using System.Collections;

public class DeviceScreenOrientationManager : MonoBehaviour {
	public float fixedUpdateInterval = -1f;

	// Use this for initialization
	void Start () {
		if(Application.isMobilePlatform)
			Cursor.visible = false;
		Screen.orientation = ScreenOrientation.LandscapeRight;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
		if(fixedUpdateInterval<0){
			fixedUpdateInterval = 0.05f;
			Time.fixedDeltaTime = fixedUpdateInterval;
		}
	}
}
