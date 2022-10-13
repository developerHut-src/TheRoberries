using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloudStateManager : MonoBehaviour {
	public bool disableCloudsAtStart = true;
	Transform thisTransform;
	public CloudData[] clouds = new CloudData[0];
	public Vector3 playerPosition = Vector3.zero;
	int i;

	[System.Serializable]
	public class CloudData
	{
		public Transform cloudTransform;
		public bool active = false;
	}

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		FindCloudsTransforms();
		if(disableCloudsAtStart)
			DisableAllClouds();
	}
	

	// Update is called once per frame
	void Update () {
		UpdateCloudsPosition();
	}

	void DisableAllClouds(){
		if(clouds.Length<1)
			return;
		for(i=0;i<clouds.Length;i++){
			clouds[i].cloudTransform.gameObject.SetActive(false);
			clouds[i].active = false;
			//Debug.Log ("child disabled:"+clouds[i].cloudTransform.name);
		}
	}

	void FindCloudsTransforms(){
		if(thisTransform.childCount<1){
			return;
		}
		clouds = new CloudData[thisTransform.childCount];
		for(i = 0;i<clouds.Length;i++){
			clouds[i] = new CloudData();
			clouds[i].cloudTransform = thisTransform.GetChild (i);
			//Debug.Log ("child name:"+clouds[i].cloudTransform.name);
		}
	}


	void UpdateCloudsPosition(){
		if(playerPosition == Vector3.zero)
			return;
		if(clouds.Length<1)
			return;
		for(i=0;i<clouds.Length;i++){
			clouds[i].cloudTransform.position = playerPosition;
		}
	}


	public void ShowCloud(int cloudID){
		if(cloudID<0){
			Debug.Log (thisTransform.name+":Invalid cloud state id: cloudID<0 !");
			return;
		}
		if(clouds.Length<=cloudID){
			Debug.Log (thisTransform.name+":Number of clouds is lower than cloudID!");
			return;
		}
		if(clouds[cloudID].active == true)
			return;
		clouds[cloudID].cloudTransform.gameObject.SetActive(true);
		clouds[cloudID].active = true;
		Debug.Log (thisTransform.name+": Show cloud '"+clouds[cloudID].cloudTransform.name+"'");
	}


	public void HideCloud(int cloudID){
		if(cloudID<0){
			Debug.Log (thisTransform.name+":Invalid cloud state id: cloudID<0 !");
			return;
		}
		if(clouds.Length<=cloudID){
			Debug.Log (thisTransform.name+":Number of clouds is lower than cloudID!");
			return;
		}
		if(clouds[cloudID].active == false)
			return;
		clouds[cloudID].cloudTransform.gameObject.SetActive(false);
		clouds[cloudID].active = false;
		Debug.Log (thisTransform.name+": Hide cloud '"+clouds[cloudID].cloudTransform.name+"'");
	}


}
