using UnityEngine;
using System.Collections;

public class Medicine : MonoBehaviour {
	public Transform target;
	public int hpAmount = 30;


	// Use this for initialization
	void Start () {
		target = transform.parent;
		HealTarget();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HealTarget(){
		if(target){
			target.SendMessage("Heal",hpAmount,SendMessageOptions.DontRequireReceiver);
		}else Debug.Log (transform.name+": can't heal target because it's not found!");
		Destroy (gameObject);
	}
}
