using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	public InventoryItem[] itemsSlots = new InventoryItem[]{new InventoryItem(),new InventoryItem(),new InventoryItem()};
	public Transform UITransform;
	public Texture2D emptySlotImage;
	public RawImage[] slotsImages = new RawImage[0];

	void Awake(){
		PlayerInfo.playerInventory = this;
	}

	// Use this for initialization
	void Start () {
		UpdateInventoryInterface();
		//AddItem(new InventoryItem("FlapStick",1));
		//AddItem(new InventoryItem("SpeedUp",1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void UpdateInventoryInterface(){
		if(UITransform == null)
			return;
		if(UITransform.childCount<1)
			return;
		slotsImages = new RawImage[UITransform.childCount];
		for(int i=0;i<slotsImages.Length;i++){
			slotsImages[i] = UITransform.GetChild (i).GetComponent<RawImage>();
		}
		
	}


	public void ClearItem(int itemID){
		if(itemID<0){
			Debug.Log ("Can't clear item because of invalid ID "+itemID);
			return;
		}
		if(itemsSlots.Length<1){
			Debug.Log ("Can't clear item because items slots length <1 !");
			return;
		}
		if(itemID>(itemsSlots.Length-1)){
			Debug.Log ("Can't clear item because itemID > items slots length !");
			return;
		}
		itemsSlots[itemID].ClearItem();
	}


	public void AddItem(InventoryItem item){
		if(itemsSlots.Length<1){
			Debug.Log ("Can't add item "+item.name+": items slots length <1 !");
			return;
		}
		int clearSlotID = -1,itemDataID = -1;
		clearSlotID = GetFreeSlot();
		if(clearSlotID>-1){
			itemsSlots[clearSlotID] = new InventoryItem(item.name,item.quantity);
			itemDataID = ItemsData.GetItemDataID(item.name);
			if(itemDataID>-1){
				if(ItemsData.items[itemDataID].icon!=null){
					itemsSlots[clearSlotID].itemIcon = ItemsData.items[itemDataID].icon;
					if(slotsImages.Length>0){
						slotsImages[clearSlotID].texture = (Texture)itemsSlots[clearSlotID].itemIcon;
					}
				}
			}
		}else Debug.Log ("Can't add item "+item.name+" to the inventory: all slots are occupied!");

	}


	public int GetFreeSlot(){
		int result = -1;
		if(itemsSlots.Length<1){
			Debug.Log ("Inventory slots length <1! Can't get free slot!");
			return result;
		}
		for(int i=0;i<itemsSlots.Length;i++){
			if(itemsSlots[i].HasItem() == false){
				result = i;
				break;
			}
		}
		return result;
	}


	public void UseItem(int itemID){
		if(itemID<0){
			Debug.Log ("Can't use item because of invalid ID "+itemID);
			return;
		}
		if(itemsSlots.Length<1){
			Debug.Log ("Can't use item because items slots length <1 !");
			return;
		}
		if(itemID>(itemsSlots.Length-1)){
			Debug.Log ("Can't use item because itemID > items slots length !");
			return;
		}
		if(!itemsSlots[itemID].HasItem()){
			Debug.Log ("Can't use item with id "+itemID+" because its slot is empty!");
			return;
		}
		int itemDataID = ItemsData.GetItemDataID(itemsSlots[itemID].name);
		if(itemDataID>-1){
			if(ItemsData.items[itemDataID].itemTransform !=null){
				Transform newItem = Instantiate (ItemsData.items[itemDataID].itemTransform,Vector3.zero,Quaternion.identity) as Transform;
				newItem.parent = PlayerInfo.playerTransform;
				newItem.localPosition = Vector3.zero;
				itemsSlots[itemID].quantity--;
				LevelInfo.RegisterItemUsing(itemsSlots[itemID].name,1);
				//Debug.Log ("Try to register item using:"+itemsSlots[itemID].name);
				if(itemsSlots[itemID].quantity<1){
					itemsSlots[itemID].ClearItem();
					if(slotsImages.Length>0 && emptySlotImage)
						slotsImages[itemID].texture = (Texture) emptySlotImage;
				}
			}
		}
	}


	[System.Serializable]
	public class InventoryItem
	{
		public string name = "";
		public int quantity = -1;
		public Texture2D itemIcon;

		public InventoryItem(){
			name = "";
			quantity = -1;
		}

		public InventoryItem(string newName,int newQuantity){
			name = newName;
			quantity = newQuantity;
		}

		public void ClearItem(){
			name = "";
			quantity = -1;
			itemIcon = null;
		}

		public bool HasItem(){
			if(quantity>-1)
				return true;
			else return false;
		}
	}
}
