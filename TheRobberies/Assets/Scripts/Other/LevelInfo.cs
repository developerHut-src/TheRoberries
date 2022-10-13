using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour {
	public static int alertsCount = 0,detectionsCount = 0,skippedDetectionsCount = 0,bonusesOnLevel = 0,obtainedBonuses = 0,objectivesCount = 0,executedObjectives = 0;
	public static int startPlayerHealth = 0,endPlayerHealth = 0;
	public static bool gameOver = false,missionComplete = false,hackingEnabled = false;
	public GameObject gameOverTextObject,missionCompleteTextObject,notificationTextObjectRef;
	public Text notificationTextRef;
	public float restartDelay = 2f;
	public static float levelEndTime,notificationEndTime = -1f,timeToRestart = -1f;
	public static UsedItem[] usedItems = new UsedItem[0];
	public static GameObject notificationTextObject;
	public static Text notificationText;
	public static Transform worldCameraTransform;
	public static Camera worldCamera;
	public static UsedItem[] pickedItems = new UsedItem[0];


	public class UsedItem
	{
		public int usedTimes = 0;
		public string name = "";

		public UsedItem(){
			usedTimes = 0;
			name = "";
		}

		public UsedItem(string newName,int newUsedTimes){
			usedTimes = newUsedTimes;
			name = newName;
		}
	}

	// Use this for initialization
	void Start () {
		notificationTextObject = notificationTextObjectRef;
		notificationText = notificationTextRef;
		worldCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		if(worldCameraTransform)
			worldCamera = worldCameraTransform.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver && gameOverTextObject){
			hackingEnabled = false;
			if(timeToRestart<0){
				if(gameOverTextObject.activeSelf == false)
					gameOverTextObject.SetActive(true);
				timeToRestart = Time.time+restartDelay;
			}else if(Time.time>timeToRestart && !GameInfo.ratingTableEnabled){
				gameOverTextObject.SetActive(false);
				GameInfo.ShowRatingTable();
			}
			return;
		}
		if(missionComplete && missionCompleteTextObject){
			if(timeToRestart<0){
				if(missionCompleteTextObject.activeSelf == false)
				missionCompleteTextObject.SetActive(true);
				timeToRestart = Time.time+restartDelay;
			}else if(Time.time>timeToRestart && !GameInfo.ratingTableEnabled){
				missionCompleteTextObject.SetActive(false);
				GameInfo.ShowRatingTable();
			}
			return;
		}

		if(Time.time>notificationEndTime && notificationEndTime>=0){
			StopShowNotification();
		}
	}


	public static void ResetLevel(){
		timeToRestart = -1f;
		gameOver = false;
		missionComplete = false;
		hackingEnabled = false;
		ResetLevelInfo();
	}


	public static void ShowNotification(string infoText,float duration){
		if(notificationTextObject==null){
			Debug.Log ("Can't show notification because notificationTextObject is not assigned!");
			return;
		}
		if(notificationText==null){
			Debug.Log ("Can't show notification because notificationText is not assigned!");
			return;
		}
		notificationText.text = infoText;
		notificationTextObject.SetActive(true);
		notificationEndTime = Time.time+duration;
	}

	public static void StopShowNotification(){
		if(notificationTextObject==null){
			Debug.Log ("Can't stop shows notification because notificationTextObject is not assigned!");
			return;
		}
		if(notificationText==null){
			Debug.Log ("Can't stop shows notification because notificationText is not assigned!");
			return;
		}
		notificationText.text = "";
		notificationTextObject.SetActive(false);
		notificationEndTime = -1f;
	}


	public static void ResetLevelInfo(){
		alertsCount = detectionsCount =skippedDetectionsCount = bonusesOnLevel = obtainedBonuses = objectivesCount = executedObjectives = 0;
		 startPlayerHealth = endPlayerHealth = GameInfo.gameDifficulty = GameInfo.safeLockDifficulty = 0;
		gameOver = missionComplete = hackingEnabled = false;
		levelEndTime = 0f;
		notificationEndTime = timeToRestart = -1f;
		 usedItems = new UsedItem[0];
		notificationTextObject = null;
		notificationText = null;
		worldCameraTransform = null;
		worldCamera = null;
	}


	public static void RegisterDetection(){
		detectionsCount++;
	}

	public static void SkipDetection(){
		skippedDetectionsCount++;
	}

	public static void RegisterAlert(){
		alertsCount++;
	}
	
	

	public static void SetGameOver(bool gO){
		gameOver = gO;
	}


	public static string GetUsedItemsList(){
		string result = "";
		if(usedItems.Length>0){
			for(int i=0;i<usedItems.Length;i++){
				if(usedItems[i]!=null){
					if(i>0)
						result+=", ";
					result+=usedItems[i].name+"("+usedItems[i].usedTimes.ToString ()+")";
				}
			}
		}
		Debug.Log ("Used items array size : "+usedItems.Length);
		return result;
	}


	public static void RegisterItemUsing(string itemName,int usedTimes){
		bool itemRegistered = false;
		ArrayList temp = new ArrayList();
		//Debug.Log ("Registering item :"+itemName);
		if(usedItems.Length>0){
			for(int i=0;i<usedItems.Length;i++){
				temp.Add(new UsedItem(usedItems[i].name,usedItems[i].usedTimes));
				//Debug.Log ("Item added to temporary array:"+usedItems[i].name);
				if(usedItems[i].name == itemName){
					usedItems[i].usedTimes+=usedTimes;
					itemRegistered = true;
					//Debug.Log ("Using of the item '"+itemName+"' already registered, number of using:"+usedItems[i].usedTimes);
				}

			}
			if(itemRegistered == false && temp.Count>0){
				temp.Add (new UsedItem(itemName,usedTimes));
				usedItems = (UsedItem[]) temp.ToArray(typeof(UsedItem));
				//Debug.Log ("Used items list was updated, length:"+usedItems.Length);
			}
		}else{
			usedItems = new UsedItem[]{new UsedItem(itemName,usedTimes)};
			//Debug.Log ("Used items list was updated(length<1), length:"+usedItems.Length);
		}
	}


	public static void RegisterPickedItem(string itemName,int pickupTimes){
		bool itemRegistered = false;
		ArrayList temp = new ArrayList();
		if(pickedItems.Length>1){
			for(int i=0;i<pickedItems.Length;i++){
				if(pickedItems[i].name == itemName){
					pickedItems[i].usedTimes++;
					itemRegistered = true;
					break;
				}else
					temp.Add(pickedItems[i]);
			}
			if(itemRegistered == false && temp.Count>0){
				pickedItems = (UsedItem[]) temp.ToArray(typeof(UsedItem));
			}
		}else{
			pickedItems = new UsedItem[]{new UsedItem(itemName,pickupTimes)};
		}
	}


	public static bool ItemPicked(string itemName){
		bool result = false;
		if(pickedItems.Length<1){
			return result;
		}

		for(int i=0;i<pickedItems.Length;i++){
			if(itemName == pickedItems[i].name){
				result = true;
				break;
			}
		}
		return result;
	}


}
