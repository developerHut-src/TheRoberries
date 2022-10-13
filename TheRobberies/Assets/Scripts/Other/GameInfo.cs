using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour {
	public static int gameDifficulty =0,safeLockDifficulty = 0;
	public DifficultyParameters[] refDifficultyParameters = new DifficultyParameters[3];
	public LevelDescription[] levelsDescriptions = new LevelDescription[0];
	public LockDifficultyDescription[] lockDifficultyDescription = new LockDifficultyDescription[3];
	public string rating = "";
	public string[] refRatingWords = new string[0];
	public GameObject refRatingTableObject;
	public float frameRateUpdateInterval = 0.3f;
	float nextFrameRateUpdateTime = 0f;
	public int frameRate = 30;
	public Text gameInfoText;
	public static GameObject ratingTableObject;
	public static string[] ratingWords = new string[0];
	public static DifficultyParameters[] difficultyParameters = new DifficultyParameters[0];
	public static LockDifficultyDescription[] staticLockDifficultyDescription = new LockDifficultyDescription[0];
	public static bool ratingTableEnabled = false;
	public static float gameTime = 0f;

	void Awake(){
		ratingTableEnabled = false;
		gameTime = 0f;
		if(difficultyParameters.Length<1)
			difficultyParameters = refDifficultyParameters;
		if(staticLockDifficultyDescription.Length<1)
			staticLockDifficultyDescription = lockDifficultyDescription;
		//Debug.Log ("static lock difficulty description length:"+staticLockDifficultyDescription.Length);
		ratingWords = refRatingWords;
		ratingTableObject = refRatingTableObject;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameTime+=Time.unscaledDeltaTime;
		if(LevelInfo.gameOver || LevelInfo.missionComplete && !ratingTableEnabled){
				rating = GetRating();
		}
		if(Time.time>nextFrameRateUpdateTime){
			nextFrameRateUpdateTime = Time.time+frameRateUpdateInterval;
			frameRate = Mathf.RoundToInt((Mathf.RoundToInt(1f/Time.deltaTime)+frameRate)*0.5f);
			
		}
		if(gameInfoText)
			SendLevelInfoText();
	}

	public void SendLevelInfoText(){
		if(gameInfoText == null)
			return;
		gameInfoText.text ="FPS:"+frameRate.ToString ();
	}

	public static void SetDifficulty(){
		if(WorldInfo.players.Length<1){
			Debug.Log("Can't set diffuclty for enemies: WorldInfo.players.Length<1!");
			return;
		}
		Memory memory;
		Eyes eyes;
		SecurityGuard securityGuard;

		for(int i=0;i<WorldInfo.players.Length;i++){
				if(WorldInfo.players[i].pTransform!=null){
					memory = WorldInfo.players[i].pTransform.GetComponent<Memory>();
					if(memory!=null){
						memory.baseLastTargetMemoryTime = difficultyParameters[gameDifficulty].enemiesTargetMemoryTime;
					}
					eyes = WorldInfo.players[i].pTransform.GetComponent<Eyes>();
					if(eyes!=null){
						eyes.fieldOfView = difficultyParameters[gameDifficulty].enemiesFOV;
						eyes.distance = difficultyParameters[gameDifficulty].enemiesDistanceOfView;
					}
					securityGuard =  WorldInfo.players[i].pTransform.GetComponent<SecurityGuard>();
					if(securityGuard!=null){
						securityGuard.chaseSpeed = difficultyParameters[gameDifficulty].enemiesChaseSpeed;
						if(securityGuard.weaponHolder!=null){
							if(securityGuard.weaponHolder.childCount>0){
							HandGun handGun = securityGuard.weaponHolder.GetChild (0).GetComponent<HandGun>();
								if(handGun){
								handGun.SetGunParameters(difficultyParameters[gameDifficulty].enemiesGunFireRate,difficultyParameters[gameDifficulty].enemiesGunFireRate,
								                         difficultyParameters[gameDifficulty].enemiesGunBulletSpeed);
								}
							}
						}
					}
				}
			
		}
	}

	public static void SetDifficultyForNPC(Transform npcTransform){
		if(WorldInfo.players.Length<1){
			Debug.Log("Can't set diffuclty for enemies: WorldInfo.players.Length<1!");
			return;
		}
		Memory memory;
		Eyes eyes;
		SecurityGuard securityGuard;
		
		for(int i=0;i<WorldInfo.players.Length;i++){
			if(WorldInfo.players[i].pTransform == npcTransform){
				memory = WorldInfo.players[i].pTransform.GetComponent<Memory>();
				if(memory!=null){
					memory.baseLastTargetMemoryTime = difficultyParameters[gameDifficulty].enemiesTargetMemoryTime;
				}
				eyes = WorldInfo.players[i].pTransform.GetComponent<Eyes>();
				if(eyes!=null){
					eyes.fieldOfView = difficultyParameters[gameDifficulty].enemiesFOV;
					eyes.distance = difficultyParameters[gameDifficulty].enemiesDistanceOfView;
				}
				securityGuard =  WorldInfo.players[i].pTransform.GetComponent<SecurityGuard>();
				if(securityGuard!=null){
					securityGuard.chaseSpeed = difficultyParameters[gameDifficulty].enemiesChaseSpeed;
					if(securityGuard.weaponHolder!=null){
						if(securityGuard.weaponHolder.childCount>0){
							HandGun handGun = securityGuard.weaponHolder.GetChild (0).GetComponent<HandGun>();
							if(handGun){
								handGun.SetGunParameters(difficultyParameters[gameDifficulty].enemiesGunFireRate,difficultyParameters[gameDifficulty].enemiesGunFireRate,
								                         difficultyParameters[gameDifficulty].enemiesGunBulletSpeed);
							}
						}
					}
				}
				break;
			}
			
		}
	}


	public static void ShowRatingTable(){
		if(ratingTableObject == null){
			Debug.Log ("Can't show rating table, because ratingTableObject not assigned!");
			return;
		}
		ratingTableEnabled = true;
		ratingTableObject.SetActive(true);
		Text ratingText = null;
		if(ratingTableObject.transform.childCount>0){
			ratingText = ratingTableObject.transform.GetChild (0).GetComponent<Text>();
		}
		if(ratingText){
			ratingText.text = "";
			ratingText.text+="СТАТУС ЗАДАНИЯ:";
			if(LevelInfo.missionComplete && LevelInfo.gameOver == false)
				ratingText.text+=" ВЫПОЛНЕНО";
			else 
				ratingText.text+=" ПРОВАЛЕНО";
			ratingText.text+="\n";
			ratingText.text+="Поднято тревог: "+LevelInfo.alertsCount.ToString ()+"\n";
			ratingText.text += "Вас обнаружили: "+LevelInfo.detectionsCount.ToString ()+"\nВы ушли от погони: "+LevelInfo.skippedDetectionsCount.ToString ();
			ratingText.text+="\nПотеряно здоровья:"+(LevelInfo.startPlayerHealth - Mathf.Clamp (LevelInfo.endPlayerHealth,0,LevelInfo.startPlayerHealth)).ToString ()
			+"\nБонусов собрано: "+LevelInfo.obtainedBonuses.ToString()+
		" из "+LevelInfo.bonusesOnLevel.ToString ()+"\nВыполнено заданий: "+LevelInfo.executedObjectives.ToString ()+" из "+LevelInfo.objectivesCount.ToString ();
			ratingText.text+="\nИспользованные предметы: "+LevelInfo.GetUsedItemsList()+"\n\nВаш рейтинг: "+GetRating();
		}else Debug.Log ("rating table Text component not found!");
		Debug.Log ("Rating table void call");
	}

	public static void GoToStartMenu(){
		//difficultyParameters = new DifficultyParameters[3];
		//staticLockDifficultyDescription = new LockDifficultyDescription[3];
		LevelInfo.ResetLevel();
		Application.LoadLevel(0);
	}

	public void GoToStartMenu2(){
		GoToStartMenu();
	}


	public static string GetRating(){
		string rating = "";
		if(ratingWords.Length<1){
			Debug.Log("Can't get rating name: ratingWords.Length<1!");
			return rating;
		}
		if(LevelInfo.gameOver == false){
		if(LevelInfo.alertsCount == 0 && LevelInfo.detectionsCount<2 && LevelInfo.detectionsCount == LevelInfo.skippedDetectionsCount 
			  && LevelInfo.usedItems.Length<1){
			rating+=ratingWords[0];
		}else if(LevelInfo.alertsCount == 0 && LevelInfo.detectionsCount<2 && LevelInfo.detectionsCount == LevelInfo.skippedDetectionsCount){
			rating+=ratingWords[1];
		}else if(LevelInfo.alertsCount <3 && LevelInfo.detectionsCount<2 && LevelInfo.detectionsCount == LevelInfo.skippedDetectionsCount){
			rating+=ratingWords[2];
		}else if(LevelInfo.alertsCount ==0 && LevelInfo.detectionsCount>1 && LevelInfo.detectionsCount == LevelInfo.skippedDetectionsCount){
			rating+=ratingWords[3];
		}else if(LevelInfo.alertsCount ==0 && LevelInfo.detectionsCount>1 && LevelInfo.detectionsCount != LevelInfo.skippedDetectionsCount){
			rating+=ratingWords[4];
		}else if(LevelInfo.alertsCount >0 && LevelInfo.detectionsCount>1 && LevelInfo.detectionsCount != LevelInfo.skippedDetectionsCount){
			rating+=ratingWords[5];
		}else if(LevelInfo.alertsCount >0 && LevelInfo.detectionsCount>1 && LevelInfo.detectionsCount != LevelInfo.skippedDetectionsCount &&
		         LevelInfo.endPlayerHealth<11){
			rating+=ratingWords[6];
		}
		}else rating = "МОКРИЦА";
		if(rating == "")
			rating = "ЖУЖЖЕЛИЦА";
		return rating;
	}
	

	[System.Serializable]
	public class DifficultyParameters
	{
		public string difficultyName = "Easy";
		public string difficultyDescription = "===";
		public float enemiesChaseSpeed = 3f;
		public float enemiesFOV = 60f;
		public float enemiesDistanceOfView = 100f;
		public float enemiesGunSpread = 3f;
		public float enemiesGunBulletSpeed = 10f;
		public float enemiesGunFireRate = 0.5f;
		public float enemiesTargetMemoryTime = 5f;
	}

	[System.Serializable]
	public class LevelDescription
	{
		public string levelName = "";
		public string levelDescription = "";
		public UnityEngine.UI.Image levelImage;
	}


	[System.Serializable]
	public class LockDifficultyDescription
	{
		public string lockDifficultyName = "Easy";
		public string lockDifficultyDesciption = "This is easy lock";
	}
}
