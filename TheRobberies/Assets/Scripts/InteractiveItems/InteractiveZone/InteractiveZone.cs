using UnityEngine;
using System.Collections;

public class InteractiveZone : MonoBehaviour {
	public string zoneName = "SmokingArea";
	public string[] npcNames = new string[0];
	// action id is used for activation desired action, for example 0 - means smoking action
	public ActionInfo actionInfo = new ActionInfo(0,5f);

	[System.Serializable]
	public class ActionInfo
	{
		public int actionID =0;
		public float actionDuration = 5f;

		public ActionInfo(){
			actionID =0;
			actionDuration = 5f;
		}

		public ActionInfo(int newActionID,float newActionDuration){
			actionID = newActionID;
			actionDuration = newActionDuration;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool AcceptableName(string curName){
		bool result = false;
		if(npcNames.Length<1){
			Debug.Log ("Can't validate NPC's name because npcNames array is empty!");
			return result;
		}
		for(int i=0;i<npcNames.Length;i++){
			if(curName == npcNames[i]){
				result = true;
				break;
			}
		}
		return result;
	}

	void OnTriggerEnter(Collider c){
		if(AcceptableName(c.name)){
			c.SendMessage("ActivateAction",actionInfo,SendMessageOptions.DontRequireReceiver);
		}
	}
}
