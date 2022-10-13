using UnityEngine;
using System.Collections;

public class Invisibility : MonoBehaviour {
	public float effectTime = 10f;
	float effectEndTime = -1f;
	MeshRenderer meshR;
	Material characterMat;
	PlayerController pC;
	
	// Use this for initialization
	void Start () {
		meshR = transform.parent.GetComponent<MeshRenderer>();
		if(meshR){
			characterMat = meshR.sharedMaterial;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(pC == null){
			pC = transform.parent.GetComponent<PlayerController>();
			if(pC == null)
				return;
		}
		if(effectEndTime<0){
			effectEndTime = Time.time+effectTime;
			if(characterMat){
				characterMat.SetColor("_Color",new Color(characterMat.color.r,characterMat.color.g,characterMat.color.b,0.3f));
			}
			WorldInfo.UnregisterPlayer(new WorldInfo.GlobalPlayerInfo(transform.parent,0,true));
		}else if(effectEndTime<Time.time){
			if(LevelInfo.missionComplete!=true)
			WorldInfo.RegisterPlayer(new WorldInfo.GlobalPlayerInfo(transform.parent,0,true));
			characterMat.SetColor("_Color",new Color(characterMat.color.r,characterMat.color.g,characterMat.color.b,1f));
			Destroy (gameObject);
		}
	}
}
