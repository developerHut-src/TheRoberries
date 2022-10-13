using UnityEngine;
using System.Collections;

public class TransformLock : MonoBehaviour {
	public bool readOrthoCameraSize = true;
	public Transform lockTransform,debugSphere,debugSphere1;
	Transform thisTransform;
	public Bounds lockArea ;
	public Vector3 levelMaxLimits = Vector3.zero,levelMinLimits = Vector3.zero;
	public Vector3 relativePos = Vector3.zero;
	public Vector3 tempV3,curMaxLimits = Vector3.zero,curMinLimits = Vector3.zero;
	Camera curCam;
	public float camSize = 2f,worldScreenSizeX = 0f, worldScreenSizeY = 0f;
	float lastCamSize = -1f;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		if(lockTransform){
			relativePos = thisTransform.position - lockTransform.position; 
		}
		curCam = transform.GetComponent<Camera>();
		if(curCam == null)
			Debug.Log ("Camera on is not found on lockTransform!");
		else{
			ReadWorldScreenSize();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(relativePos==Vector3.zero)
			return;

		if(curCam){
			ReadWorldScreenSize();
				RecalculateLimits();
		}
		tempV3 = lockTransform.position+relativePos;
		tempV3.x = Mathf.Clamp(tempV3.x,curMinLimits.x,curMaxLimits.x);
		tempV3.y = Mathf.Clamp(tempV3.y,curMinLimits.y,curMaxLimits.y);
		tempV3.z = Mathf.Clamp(tempV3.z,curMinLimits.z,curMaxLimits.z);
		thisTransform.position = tempV3;

	}


	void RecalculateLimits(){
		lastCamSize = camSize;
		curMinLimits.x = levelMinLimits.x+worldScreenSizeY;
		curMinLimits.y = levelMinLimits.y;
		curMinLimits.z = levelMinLimits.z+worldScreenSizeX;

		curMaxLimits.x = levelMaxLimits.x-worldScreenSizeY;
		curMaxLimits.y = levelMaxLimits.y;
		curMaxLimits.z = levelMaxLimits.z-worldScreenSizeX;
	}


	void ReadWorldScreenSize(){
		Vector3 screenCenter,screenBorder;
		screenCenter = curCam.ScreenToWorldPoint(new Vector3(curCam.pixelRect.xMax*0.5f,curCam.pixelRect.yMax*0.5f+curCam.pixelRect.y*0.5f,curCam.nearClipPlane));
		screenBorder = curCam.ScreenToWorldPoint(new Vector3(curCam.pixelRect.xMax,curCam.pixelRect.yMax*0.5f,curCam.nearClipPlane));
		//screenCenter = curCam.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height*0.5f,curCam.nearClipPlane));
		//screenBorder = curCam.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height*0.5f,curCam.nearClipPlane));
		if(debugSphere)
			debugSphere.position = new Vector3(screenBorder.x,debugSphere.position.y,screenBorder.z);
		worldScreenSizeX = Vector3.Distance (screenCenter,screenBorder);
		screenBorder = curCam.ScreenToWorldPoint(new Vector3(Screen.width*0.5f,Screen.height,curCam.nearClipPlane));
		if(debugSphere1)
			debugSphere1.position = new Vector3(screenBorder.x,debugSphere1.position.y,screenBorder.z);
		worldScreenSizeY = Vector3.Distance (screenCenter,screenBorder);
		//Debug.Log ("Camera rect:"+curCam.pixelRect);
	}
	
}
