using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	public GameObject optionsMenuObject,gameMenuObject,gameMenuButton;
	public Text gameTimeText,difficultyNameText,lockDifficultyNameText,objectivesStatusText;
	bool gameMenuDisabled = true,optionsMenuDisabled = true,gameMenuButtonDisabled = false,gameMenuRootObjectDisabled = false;
	int lastTimeInfo = 0,curTimeInfo = 0,temp;
	public float timeInfoUpdateInterval = 1f;
	float nextTimeInfoUpdateTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(LevelInfo.missionComplete || LevelInfo.gameOver){
			if(gameMenuRootObjectDisabled == false){
				gameObject.SetActive(false);
				gameMenuRootObjectDisabled = true;
			}
		}

		if(!gameMenuDisabled)
			UpdateTimeInfo();
	}


	public void UpdateTimeInfo(){
		if(gameTimeText == null)
			return;
		if(GameInfo.gameTime<nextTimeInfoUpdateTime)
			return;
		curTimeInfo = Mathf.RoundToInt(GameInfo.gameTime);
		if(curTimeInfo!= lastTimeInfo){
			temp = Mathf.RoundToInt(curTimeInfo/60);
			gameTimeText.text = temp.ToString()+":"+Mathf.RoundToInt(curTimeInfo-temp*60).ToString();
			lastTimeInfo = curTimeInfo;
		}
		nextTimeInfoUpdateTime = GameInfo.gameTime+timeInfoUpdateInterval;
	}


	public void UpdateGameStatistic(){
		if(difficultyNameText){
			difficultyNameText.text = GameInfo.difficultyParameters[GameInfo.gameDifficulty].difficultyName;
		}
		if(lockDifficultyNameText){
			lockDifficultyNameText.text = GameInfo.staticLockDifficultyDescription[GameInfo.safeLockDifficulty].lockDifficultyName;
		}
		if(objectivesStatusText){
			objectivesStatusText.text = LevelInfo.executedObjectives.ToString ()+" ИЗ "+LevelInfo.objectivesCount.ToString ();
		}
	}


	public void ChangeVolume(float newVolume){
		AudioListener.volume = newVolume;
	}


	public void OpenGameMenu(){
		if(gameMenuButton)
			gameMenuButton.SetActive(false);
		LevelInfo.hackingEnabled = true;
		Time.timeScale = 0f;
		UpdateGameStatistic();
		ActivateGameMenu(true);
	}


	public void CloseGameMenu(){
		if(gameMenuButton)
			gameMenuButton.SetActive(true);
		LevelInfo.hackingEnabled = false;
		ActivateGameMenu(false);
		Time.timeScale = 1f;
	}


	public void ActivateGameMenu(bool activity){
		if(gameMenuObject == null){
			Debug.Log ("Can't set activity for Game Menu: gameMenuObject is not assigned!");
			return;
		}
		if(activity)
			ActivateOptionsMenu(false);
		gameMenuObject.SetActive(activity);
		gameMenuDisabled = !activity;
	}


	public void ActivateOptionsMenu(bool activity){
		if(optionsMenuObject == null){
			Debug.Log ("Can't set activity for Options Menu: optionsMenuObject is not assigned!");
			return;
		}
		if(activity)
			ActivateGameMenu(false);
		optionsMenuObject.SetActive(activity);
	}


	public void OpenURL(string newURL){
		Application.OpenURL(newURL);
	}


	public void GoToStartMenu(){
		Time.timeScale = 1f;
		GameInfo.GoToStartMenu();
	}
}
