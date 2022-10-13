using UnityEngine;
using System.Collections;

public class SimpleHealthController : MonoBehaviour {
	public int maxHitPoints = 100;
	public int curHitPoints = 100;


	// Use this for initialization
	void Start () {
		LevelInfo.startPlayerHealth = curHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerInfo.playerHealth = curHitPoints;
		if(LevelInfo.gameOver || LevelInfo.missionComplete)
			LevelInfo.endPlayerHealth = curHitPoints;
	}

	public void RenewHitPoints(){
		curHitPoints = maxHitPoints;
	}

	public void Heal(int hpAmount){
		curHitPoints+=hpAmount;
		curHitPoints = Mathf.Clamp(curHitPoints,0,maxHitPoints);
	}

	public void Hit(WorldInfo.GunShotParameters hitInfo){
		curHitPoints-= hitInfo.damage;
		if(curHitPoints<1)
			LevelInfo.gameOver = true;
	}
}
