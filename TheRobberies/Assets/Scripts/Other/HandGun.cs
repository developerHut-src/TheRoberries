using UnityEngine;
using System.Collections;

public class HandGun : MonoBehaviour {
	public float fireRate = 0.3f,bulletSpeed = 5f,spread = 0.5f;
	public int damage = 5;
	public Transform bulletsClip,npcRoot;
	float nextFireTime = 0f;
	Transform curBullet,thisTransform;
	WeaponBullet bullet;
	bool enableFiring = false;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		npcRoot = thisTransform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time<nextFireTime || !enableFiring)
			return;
		Shot();
	}

	void Shot(){
		if(npcRoot == null){
			Debug.Log ("NPC root not found!");
			return;
		}
		if(bulletsClip == null){
			Debug.Log (npcRoot.name+": Can't shot - bullets clip transform is not assigned!");
			return;
		}
		if(Time.time<nextFireTime)
			return;
		nextFireTime = Time.time+fireRate;
		curBullet = GetFreeBullet();
		if(curBullet)
			ShotBullet();
		else Debug.Log (npcRoot.name+": Can't shot - can't find free bullet! Maybe should increase number of pre created bullets for this fire rate:"+fireRate);
	}


	void ShotBullet(){
		curBullet.parent = thisTransform;
		curBullet.localPosition = Vector3.zero;
		curBullet.LookAt(GetSpreadPosition());
		curBullet.gameObject.SetActive(true);
		curBullet.parent = bulletsClip;
		bullet = curBullet.GetComponent<WeaponBullet>();
		if(bullet){
			bullet.damage = damage;
			bullet.bulletSpeed = bulletSpeed;
			bullet.bulletSender = npcRoot.name;
		}
	}


	Vector3 GetSpreadPosition(){
		Vector3 result = Vector3.zero;
		result = thisTransform.position+thisTransform.forward*10;
		result +=Vector3.right*Random.Range (-spread,spread);
		return result;
	}


	Transform GetFreeBullet(){
		Transform result = null;
		if(bulletsClip.childCount<1)
			return result;
		foreach(Transform bullet in bulletsClip){
			if(bullet.gameObject.activeSelf == false){
				result = bullet;
				break;
			}
		}
		return result;
	}

	void SetFiring(bool f){
		enableFiring = f;
	}

	public void SetGunParameters(float newFireRate,float newSpread,float newBulletSpeed){
		fireRate = newFireRate;
		spread = newSpread;
		bulletSpeed = newBulletSpeed;
	}
}
