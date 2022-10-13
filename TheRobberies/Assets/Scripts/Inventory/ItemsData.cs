using UnityEngine;
using System.Collections;

public class ItemsData : MonoBehaviour {
	public ItemData[] refItems = new ItemData[0];
	public static ItemData[] items = new ItemData[0];

	// Use this for initialization
	void Awake () {
		if(refItems.Length>0)
			items = refItems;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public static int GetItemDataID(string itemName){
		int result = -1;
		if(string.IsNullOrEmpty(itemName)){
			Debug.Log ("Can't get item data ID: nvalid item's name( null or empty)!");
			return result;
		}
		for(int i=0;i<items.Length;i++){
			if(items[i].name == itemName){
				result = i;
				break;
			}
		}
		return result;
	}


	public static string GetItemDescription(string itemName){
		string result = "";
		int itemID = GetItemDataID(itemName);
		if(itemID>-1){
			result = items[itemID].itemDescription;
		}
		return result;
	}


	[System.Serializable]
	public class ItemData
	{
		public string name = "";
		public Texture2D icon;
		public string itemDescription = "";
		public Transform itemTransform;
	}
}
