using UnityEngine;
using System.Collections;

public class PickupableItem : MonoBehaviour {
	public bool autoPickup = true,registerAsBonus = true,showDescription = false;
	public string playerName = "";
	public string pickedItemName = "";
	public int itemQuantity = 1;
	//pickup types:0 - inventory item,1- special item
	public int pickupType = 0;
	public Transform playerT;
	float lastClickTime = -1f;

	// Use this for initialization
	void Start () {
		if(registerAsBonus)
			LevelInfo.bonusesOnLevel++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PickupThisItem(){
		if(PlayerInfo.playerInventory == null){
			Debug.Log (transform.name+": Can't pick this item: PlayerInfo.playerInventory is not assigned !");
			return;
		}
		PlayerInfo.playerInventory.AddItem(new Inventory.InventoryItem(pickedItemName,itemQuantity));
		Destroy (gameObject);
		if(registerAsBonus)
			LevelInfo.obtainedBonuses++;
	}

	void PickAsSpecialItem(){
		LevelInfo.RegisterPickedItem(pickedItemName,1);
		Destroy (gameObject);
		if(registerAsBonus)
			LevelInfo.obtainedBonuses++;
	}


	void OnTriggerEnter(Collider c){
		if(c.name == PlayerInfo.curPlayerName || c.name == playerName){
			playerT = c.transform;
			if(autoPickup){
				if(pickupType == 0 && PlayerInfo.playerInventory){
					if(PlayerInfo.playerInventory.GetFreeSlot()>-1)
					PickupThisItem();
					else Debug.Log ("Can't pickup item: inventory hasn't free slot!");
				}else if(pickupType == 1){
					PickAsSpecialItem();
					Debug.Log ("special item '"+pickedItemName+"' has been picked!");
				}
			}
		}
	}

	void OnTriggerExit(Collider c){
		if(c.name == PlayerInfo.curPlayerName || c.name == playerName){
			if(playerT)
				playerT = null;
		}
	}

	void Click(ScreenClicker.ClickInfo clickInfo){
		if(!showDescription)
			return;
		if(ScreenClicker.DoubleClick(lastClickTime,Time.time,clickInfo.doubleClickInterval)){
			if(clickInfo.sender){
					clickInfo.sender.SendMessageUpwards("SetTargetPosition",transform.position,SendMessageOptions.DontRequireReceiver);
				LevelInfo.StopShowNotification();
				Debug.Log (transform.name+": i'm was clicked by double click!");
			}
		}else{
			if(clickInfo.sender)
				clickInfo.sender.SendMessageUpwards("SetTargetPosition",Vector3.zero,SendMessageOptions.DontRequireReceiver);
			LevelInfo.ShowNotification(ItemsData.GetItemDescription(pickedItemName),5f);
			Debug.Log (transform.name+": i'm was clicked by single click!");
			
		}
		lastClickTime = Time.time;
	}
}
