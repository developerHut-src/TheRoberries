using UnityEngine;
using System.Collections;

public class InteractiveDoor : MonoBehaviour {
	public string keyName = "keyA";
	public bool locked = true;
	public Transform playerT;
	public float doorOpeningSpeed = 1.5f;
	float curAnimTime = 0f;
	bool doorOpened = false;
	Animation doorAnimation;

	// Use this for initialization
	void Start () {
		doorAnimation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.name == PlayerInfo.curPlayerName ){
			playerT = c.transform;
			if(LevelInfo.ItemPicked(keyName))
				locked = false;
			if(!locked){
				SetDoorOpening(true);
			}
		}
	}

	void OnTriggerExit(Collider c){
		if(playerT == null)
			return;
		if(playerT == c.transform){
			playerT = null;
			if(doorOpened)
				SetDoorOpening(false);
		}
	}

	void SetDoorOpening(bool open){
		if(doorAnimation == null)
			return;
		doorAnimation["DoorAOpening"].wrapMode = WrapMode.Clamp;
		if(open){
			doorAnimation["DoorAOpening"].speed = doorOpeningSpeed;
			doorAnimation.Play("DoorAOpening");
			doorOpened = true;
		}else{
			doorAnimation["DoorAOpening"].speed = -doorOpeningSpeed;
			doorAnimation.Play("DoorAOpening");
			doorOpened = false;
		}
	}
}
