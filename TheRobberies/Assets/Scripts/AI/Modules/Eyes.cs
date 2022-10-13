using UnityEngine;
using System.Collections;

public class Eyes : MonoBehaviour {
	public float fieldOfView = 70f,distance = 100f, sightUpdateInterval = 0.2f;
	public bool autoUpdateSight = true;
	public int[] searchForTeamIDs = new int[0];
	public LayerMask layerMask = new LayerMask();
	public Transform eyesObject;
	//[HideInInspector]
	public Transform[] visibleCharacters = new Transform[0];
	[HideInInspector]
	public Transform[] visibleItems = new Transform[0];
	public WorldInfo.GameEvent[] visibleEvents = new WorldInfo.GameEvent[0];
	float nextSightUpdateTime= 0f;
	ArrayList tempArray = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(eyesObject==null){
			Debug.Log ("Eyes object not assigned!");
			return;
		}
	if(nextSightUpdateTime<Time.time && autoUpdateSight){
			UpdateSight();
			nextSightUpdateTime = Time.time+sightUpdateInterval;
		}
	}


	void UpdateSight(){
		if(searchForTeamIDs.Length>0){
			CheckCharactersVisibility(searchForTeamIDs);
		}else CheckCharactersVisibility();
	}


	public void CheckCharactersVisibility(int[] teamIDs){
		if(WorldInfo.players.Length<1){
			Debug.Log (transform.root.name+": WorldInfo.players.Length<1 !");
			return;
		}
		tempArray  = new ArrayList();
		foreach(WorldInfo.GlobalPlayerInfo pInfo in WorldInfo.players){
			foreach(int teamID in teamIDs){
				if(pInfo.teamId == teamID){
					if(!Physics.Linecast(pInfo.pTransform.position,eyesObject.position,layerMask)){
						if(Vector3.Angle (eyesObject.forward,pInfo.pTransform.position - eyesObject.position)<=fieldOfView)
							tempArray.Add (pInfo.pTransform);
					}
				}
			}
		}
		if(tempArray.Count>0){
			visibleCharacters = (Transform[]) tempArray.ToArray (typeof(Transform));
		}else visibleCharacters = new Transform[0];
	}


	public void CheckCharactersVisibility(){
		if(WorldInfo.players.Length<1){
			Debug.Log (transform.root.name+": WorldInfo.players.Length<1 !");
			return;
		}
		tempArray  = new ArrayList();
		foreach(WorldInfo.GlobalPlayerInfo pInfo in WorldInfo.players){
			if(!Physics.Linecast(pInfo.pTransform.position,eyesObject.position)){
					tempArray.Add (pInfo.pTransform);
				}
		}
		if(tempArray.Count>0){
			visibleCharacters = (Transform[]) tempArray.ToArray (typeof(Transform));
		}else visibleCharacters = new Transform[0];
	}


	public Transform GetClosestVisibleCharacter(){
		Transform result = null;
		if(eyesObject == null){
			Debug.Log (transform.name+": Can't get closest visible character, because eyesObject is not assigned!");
		}
		if(visibleCharacters.Length<1){
			return result;
		}
		float minDist = Mathf.Infinity,curDist;
		int charID = -1;

		for(int i=0;i<visibleCharacters.Length;i++){
			curDist = Vector3.Distance (eyesObject.position,visibleCharacters[i].position);
			if(curDist<minDist){
				minDist = curDist;
				charID = i;
			}
		}
		if(charID>-1)
			result = visibleCharacters[charID];

		return result;
	}


	public Transform GetClosestVisibleCharacterInRadius(float radius){
		Transform result = null;
		if(eyesObject == null){
			Debug.Log (transform.name+": Can't get closest visible character in radius, because eyesObject is not assigned!");
		}
		if(visibleCharacters.Length<1){
			return result;
		}
		float minDist = Mathf.Infinity,curDist;
		int charID = -1;
		
		for(int i=0;i<visibleCharacters.Length;i++){
			curDist = Vector3.Distance (eyesObject.position,visibleCharacters[i].position);
			if(curDist<minDist && curDist<=radius){
				minDist = curDist;
				charID = i;
			}
		}
		if(charID>-1)
			result = visibleCharacters[charID];
		
		return result;
	}
}
