using UnityEngine;
using System.Collections;

public class FlapStick : MonoBehaviour {
	public float soundRadius = 10f;
	public string eventType = "sound";
	AudioSource audioSource;
	float soundLength = 0.3f;

	// Use this for initialization
	void Start () {
		transform.parent = null;
		audioSource = transform.GetComponent<AudioSource>();
		UseFlapStick();
	}

	void OnEnable(){

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UseFlapStick(){
		if(audioSource){
			//Debug.Log ("Audio source assigned!");
			if(audioSource.clip){
				audioSource.Play();
				soundLength = audioSource.clip.length;
				//Debug.Log ("Sound length:"+soundLength);
			}
		}
		WorldInfo.AddEvent(new WorldInfo.GameEvent(eventType,soundRadius,Time.time+0.3f,transform.position,transform.name),false);
		Destroy(gameObject,soundLength);
	}
}
