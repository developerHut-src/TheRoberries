using UnityEngine;
using System.Collections;

public class WeaponBullet : MonoBehaviour {
	public float bulletSpeed = 5f,forwardCollisionLength = 0.3f,lifeTime = 2f;
	public LayerMask collisionMask = new LayerMask();
	public int damage = 0;
	Transform thisTransform;
	RaycastHit hitInfo;
	public string bulletSender = "";
	float deactivationTime = -1f;
	Vector3 moveDir = Vector3.zero,initLocalScale = new Vector3(0.2f,1f,0.4f);

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		//initLocalScale = thisTransform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(deactivationTime<0)
			deactivationTime = Time.time+lifeTime;

		if(Time.time>deactivationTime){
			DeactivateBullet();
			//Debug.Log (bulletSender+": Bullet "+thisTransform.name+"deactivated by time!");
			return;
		}
		if(Physics.Raycast (thisTransform.position,thisTransform.forward,out hitInfo,forwardCollisionLength,collisionMask)){
			SendDamage();
			DeactivateBullet();
			//Debug.Log (bulletSender+": Bullet "+thisTransform.name+"deactivated by collider:"+hitInfo.transform.name);
		}
		moveDir = thisTransform.InverseTransformDirection(thisTransform.forward);
		thisTransform.Translate(moveDir*Time.deltaTime*bulletSpeed);
		thisTransform.localScale = initLocalScale;
	}


	void DeactivateBullet(){
		deactivationTime = -1f;
		thisTransform.localPosition = Vector3.zero;
		thisTransform.gameObject.SetActive(false);
	}


	void SendDamage(){
		if(hitInfo.transform == null)
			return;
		WorldInfo.GunShotParameters shotInfo = new WorldInfo.GunShotParameters(damage,Vector3.zero,hitInfo.point,hitInfo.transform.name,bulletSender);
		hitInfo.transform.SendMessageUpwards("Hit",shotInfo,SendMessageOptions.DontRequireReceiver);
	}
}
