using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MeshRenderManager))]
public class MeshRenderManagerEditor : Editor {
	MeshRenderManager mRM;
	public bool disableMeshRenderers = false;
	public Transform parentTransform;

	// Use this for initialization
	void Awake () {
		mRM = (MeshRenderManager)target;
	}
	
	// Update is called once per frame
	public override void OnInspectorGUI () {
		parentTransform = EditorGUILayout.ObjectField(parentTransform,typeof(Transform),true) as Transform;
		disableMeshRenderers = EditorGUILayout.Toggle("Disable mesh renderers",disableMeshRenderers);
		if(GUILayout.Button ("Set renderers activity")){
			if(parentTransform)
				mRM.SetRenderersActivity(parentTransform,disableMeshRenderers);
			else Debug.Log ("Root transform for mesh renderers not found!");
		}
	}
}
