using UnityEngine;
using System.Collections;

public class ExitZone : MonoBehaviour {
	public string playerName = "Player";


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.name == playerName || c.name == PlayerInfo.curPlayerName){
			if(LevelInfo.executedObjectives == LevelInfo.objectivesCount){
				LevelInfo.missionComplete = true;
				LevelInfo.levelEndTime = Time.time;
			}
		}
	}
}
