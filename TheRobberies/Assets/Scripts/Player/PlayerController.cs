using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	public Vector3 curPoint = Vector3.zero;
	Vector3 moveDir = Vector3.zero;
	public float stoppingRadius = 0.2f, collisionRadius = 0.4f,rotationSpeed = 5f,movementSpeed = 3f, movementSpeed1 = 3f,defaultHeight = -1f;
	//public Camera mainCam;
	[HideInInspector]
	public Transform thisTransform;
	GameObject cursorInstance;
	public float curSpeed = 0f;
	RaycastHit hitInfo;
	public LayerMask collisionMask = new LayerMask();
	Quaternion rotDir = Quaternion.identity;
	public NavMeshAgent agent;
	public bool visibleDestinationOnly = true;
	public Transform cursorTransform,skin;
	Transform cursorTransformInstance;
	bool playerUnregistered = false;
	Animator animator;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		PlayerInfo.curPlayerName = thisTransform.name;
		PlayerInfo.playerTransform = thisTransform;
	if(defaultHeight>0)
			thisTransform.position = new Vector3(thisTransform.position.x,defaultHeight,thisTransform.position.z);
		curSpeed = movementSpeed;
		agent = thisTransform.GetComponent<NavMeshAgent>();
		WorldInfo.RegisterPlayer(new WorldInfo.GlobalPlayerInfo(thisTransform,0,true));
		if(cursorTransform){
			 cursorTransformInstance = Instantiate (cursorTransform,Vector3.zero,Quaternion.identity) as Transform;
			if(cursorTransformInstance){
				cursorInstance = cursorTransformInstance.GetChild (0).gameObject;
				cursorTransformInstance.transform.position = new Vector3(cursorInstance.transform.position.x,defaultHeight,cursorInstance.transform.position.z);
				Debug.Log ("Cursor name:"+cursorInstance.name);
			}
		}
		if(skin)
			animator = skin.GetComponent<Animator>();
		else Debug.Log (thisTransform.name+": Skin is not assigned, can't get Animator component!");
	}
	
	// Update is called once per frame
	void Update () {
		if(LevelInfo.gameOver || LevelInfo.missionComplete){
			if(playerUnregistered == false){
				WorldInfo.UnregisterPlayer(new WorldInfo.GlobalPlayerInfo(thisTransform,0,false));
				playerUnregistered = true;
			}
			return;
		}
		MoveToTarget ();
	}

	/*
	void ReadDestinationPoint(){
		if(mainCam == null)
			return;
		if(Input.GetMouseButtonDown(0)){
			Ray newRay = mainCam.ScreenPointToRay(Input.mousePosition);
			//curPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
			if(Physics.Raycast(newRay,out hitInfo)){
				curPoint = hitInfo.point;
				curPoint.y = defaultHeight;
				if(cursorInstance){
					cursorInstance.position = curPoint;
					cursorInstance.GetChild(0).gameObject.SetActive(true);
				}
			}else curPoint = Vector3.zero;
			if(curPoint !=Vector3.zero && visibleDestinationOnly){
				CheckDestinationVisibility();
			}

		}
	}
	*/

	public void SetTargetPosition(Vector3 pos){
		curPoint = pos;
		if(cursorTransformInstance)
			cursorTransformInstance.position = new Vector3(curPoint.x,defaultHeight,curPoint.z);
		if(curPoint !=Vector3.zero && visibleDestinationOnly){
			CheckDestinationVisibility();
		}

	}

	void MoveToTarget(){
		if(curPoint==Vector3.zero){
			if(cursorInstance){
				if(cursorInstance.activeInHierarchy)
					cursorInstance.SetActive(false);
			}
			if(animator)
				animator.SetFloat ("speed",0f);
			return;
		}
		if(cursorInstance){
			if(cursorInstance.activeInHierarchy==false)
				cursorInstance.SetActive(true);
			
		}
		if(animator)
			animator.SetFloat ("speed",1f);
		agent.speed = curSpeed;
	//	moveDir = (curPoint - thisTransform.position).normalized;
		//RotateToTarget();
		//moveDir = thisTransform.forward;
		//thisTransform.Translate(moveDir*Time.deltaTime*curSpeed);
		agent.SetDestination(curPoint);
		if(Vector3.Distance (thisTransform.position,curPoint)<stoppingRadius){
			curPoint = Vector3.zero;
		}

	}

	void RotateToTarget(){
		rotDir = Quaternion.LookRotation(thisTransform.TransformDirection((curPoint - thisTransform.position).normalized));
		thisTransform.localRotation = Quaternion.Lerp (thisTransform.localRotation,rotDir,Time.deltaTime*rotationSpeed);
	}

	public void CheckDestinationVisibility(){
		if(Physics.Linecast(thisTransform.position,curPoint,collisionMask))
			curPoint = Vector3.zero;
	}

	void CheckCollisionInForward(){
		if(Physics.CapsuleCast(thisTransform.position+thisTransform.up*0.3f,thisTransform.position+thisTransform.up*-0.3f,0.3f,moveDir,0.6f,collisionMask))
			curPoint = Vector3.zero;
	}

	public void ResetCurPoint(){
		curPoint = Vector3.zero;
		if(agent)
			if(agent.hasPath)
				agent.ResetPath();
	}
}
