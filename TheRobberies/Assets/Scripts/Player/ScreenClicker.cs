using UnityEngine;
using System.Collections;

public class ScreenClicker : MonoBehaviour {
	public Camera mainCam;
	RaycastHit hitInfo;
	public Vector3 cur3DPoint = Vector3.zero;
	public float doubleClickInterval = 0.3f;
	PlayerController pC;

	// Use this for initialization
	void Start () {
		pC = transform.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(mainCam == null){
			Debug.Log ("Main camera not found!");
			return;
		}
		if(LevelInfo.gameOver)
			return;
		if(LevelInfo.hackingEnabled || LevelInfo.missionComplete)
			return;
		if(Input.GetMouseButtonDown(0)){
			Ray newRay = mainCam.ScreenPointToRay(Input.mousePosition);
			//curPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
			if(Physics.Raycast(newRay,out hitInfo)){
				cur3DPoint = hitInfo.point;
				if(pC)
					cur3DPoint.y = pC.defaultHeight;
			}else{ 
				cur3DPoint = Vector3.zero;
			}
			if(pC){
				pC.SetTargetPosition(cur3DPoint);
				if(cur3DPoint!=Vector3.zero)
					hitInfo.collider.SendMessageUpwards("Click",new ClickInfo(pC.thisTransform,hitInfo.point,doubleClickInterval),SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public class ClickInfo
	{
		public Transform sender;
		public Vector3 clickPoint = Vector3.zero;
		public float doubleClickInterval = 0.3f;

		public ClickInfo(Transform newSender,Vector3 newClickPoint,float newDoubleClickInterval){
			sender = newSender;
			clickPoint = newClickPoint;
			doubleClickInterval = newDoubleClickInterval;
		}
	}


	public static bool DoubleClick(float lastClickTime,float curClickTime,float doubleClickInterval){
		bool result = false;
		if(lastClickTime<0 || curClickTime <0)
			return result;
		if(curClickTime-lastClickTime<=doubleClickInterval)
			result = true;
		return result;
	}
}
