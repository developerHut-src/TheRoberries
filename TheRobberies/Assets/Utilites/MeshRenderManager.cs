using UnityEngine;
using System.Collections;

public class MeshRenderManager : MonoBehaviour {

	MeshRenderer curMeshRenderer;

	// Use this for initialization
	void Awake() {

	}


	public void SetRenderersActivity(Transform tParent,bool rActivity){
		curMeshRenderer = tParent.GetComponent<MeshRenderer>();
		if(curMeshRenderer!=null)
			curMeshRenderer.enabled = rActivity;
		if(tParent.childCount>0){
			foreach(Transform child in tParent){
				SetRenderersActivity(child,rActivity);
			}
		}
	}
}
