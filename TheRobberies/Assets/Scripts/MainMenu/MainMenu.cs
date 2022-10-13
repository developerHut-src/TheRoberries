using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	//difficulty ID: 0 - easy, 1- normal, 2 - hard, 3 - mission impossible
	public int maxLevelNumber = 0,selectedLevel = 0,selectedDifficulty = 0,selectedQuestDifficulty = 0;
	public GameObject startMenuObject,mainMenuObject,levelSelectionMenuObject,difficultySelectionMenuObject,questDifficultySelectionMenuObject;
	public Text levelTitle,difficultyTitle,difficultyDescription,lockDifficultyTitle,lockDifficultyDescription;
	GameInfo gameInfo;

	// Use this for initialization
	void Awake(){
		gameInfo = GetComponent<GameInfo>();
	}

	void Start(){
		UpdateDifficultyInfo();
		UpdateLevelInfo();
		UpdateLockDifficultyInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartSelectedLevel(){
		Application.LoadLevel(selectedLevel+1);
	}


	public void ChangeLevelID(int step){
		selectedLevel+=step;
		selectedLevel = Mathf.Clamp (selectedLevel,0,maxLevelNumber);
		UpdateLevelInfo();
	}

	

	public void ChangeDifficultyID(int step){
		selectedDifficulty+=step;
		selectedDifficulty = Mathf.Clamp (selectedDifficulty,0,2);
		GameInfo.gameDifficulty = selectedDifficulty;
		UpdateDifficultyInfo();
	}
	
	

	public void ChangeQuestDifficultyID(int step){
		selectedQuestDifficulty+=step;
		selectedQuestDifficulty = Mathf.Clamp (selectedQuestDifficulty,0,2);
		GameInfo.safeLockDifficulty = selectedQuestDifficulty;
		UpdateLockDifficultyInfo();
	}



	public void ExitFromGame(){
		Application.Quit();
	}


	public void SetStartMenu(bool activate){
		if(startMenuObject){
			startMenuObject.SetActive(activate);
		}else Debug.Log ("Can't manage Start Menu: startMenuObject not assigned!");
	}


	public void SetMainMenu(bool activate){
		if(mainMenuObject){
			mainMenuObject.SetActive(activate);
		}else Debug.Log ("Can't manage Main Menu: mainMenuObject not assigned!");
	}


	public void SetLevelSelectionMenu(bool activate){
		if(levelSelectionMenuObject){
			levelSelectionMenuObject.SetActive(activate);
		}else Debug.Log ("Can't manage Level Selection Menu: levelSelectionMenuObject not assigned!");
	}


	public void SetDifficultySelectionMenu(bool activate){
		if(difficultySelectionMenuObject){
			difficultySelectionMenuObject.SetActive(activate);
		}else Debug.Log ("Can't manage Difficulty Selection Menu: difficultySelectionMenuObject not assigned!");
	}


	public void SetQuestDifficultySelectionMenu(bool activate){
		if(questDifficultySelectionMenuObject){
			questDifficultySelectionMenuObject.SetActive(activate);
		}else Debug.Log ("Can't manage Qust Difficulty Selection Menu: questDifficultySelectionMenuObject not assigned!");
	}


	/*
	public void ActivateMainMenu(){
		SetMainMenu(true);
		SetStartMenu(false);
		SetLevelSelectionMenu(false);
		SetDifficultySelectionMenu(false);
		SetQuestDifficultySelectionMenu(false);
	}


	public void ActivateStartMenu(){
		SetMainMenu(false);
		SetStartMenu(true);
		SetLevelSelectionMenu(false);
		SetDifficultySelectionMenu(false);
		SetQuestDifficultySelectionMenu(false);
	}


	public void ActivateLevelSelectionMenu(){
		SetMainMenu(false);
		SetStartMenu(false);
		SetLevelSelectionMenu(true);
		SetDifficultySelectionMenu(false);
		SetQuestDifficultySelectionMenu(false);
	}


	public void ActivateDifficultySelectionMenu(){
		SetMainMenu(false);
		SetStartMenu(false);
		SetLevelSelectionMenu(false);
		SetDifficultySelectionMenu(true);
		SetQuestDifficultySelectionMenu(false);
	}


	public void ActivateQuestDifficultySelectionMenu(){
		SetMainMenu(false);
		SetStartMenu(false);
		SetLevelSelectionMenu(false);
		SetDifficultySelectionMenu(false);
		SetQuestDifficultySelectionMenu(true);
	}

*/

	public void ActivateMenu(int menuID){
		if(menuID == 0)
			SetStartMenu(true);
		else
			SetStartMenu(false);

		if(menuID == 1)
			SetMainMenu(true);
		else
			SetMainMenu(false);

		if(menuID == 2)
			SetLevelSelectionMenu(true);
		else
			SetLevelSelectionMenu(false);
	
		if(menuID == 3)
			SetDifficultySelectionMenu(true);
		else
			SetDifficultySelectionMenu(false);

		if(menuID == 4)
			SetQuestDifficultySelectionMenu(true);
		else
			SetQuestDifficultySelectionMenu(false);

	}

	public int GetMenuID(string menuName){
		int result = -1;
		if(menuName == "StartMenu")
			result = 0;
		else if(menuName == "MainMenu")
			result = 1;
		else if(menuName == "LevelSelectionMenu")
			result = 2;
		else if(menuName == "DifficultySelectionMenu")
			result = 3;
		else if(menuName == "QuestDifficultySelectionMenu")
			result = 4;

		return result;
	}


	public void UpdateLevelInfo(){
		if(gameInfo == null){
			Debug.Log ("current GameInfo component is empty, can't update interface by its data!");
			return;
		}
		if(levelTitle){
			levelTitle.text = gameInfo.levelsDescriptions[selectedLevel].levelName;
		}else Debug.Log ("Can't update level title: levelTitle not assigned!");

	}


	public void UpdateDifficultyInfo(){
		if(gameInfo == null){
			Debug.Log ("current GameInfo component is empty, can't update interface by its data!");
			return;
		}
		if(difficultyTitle){
			difficultyTitle.text = gameInfo.refDifficultyParameters[selectedDifficulty].difficultyName;
		}else Debug.Log ("Can't update difficulty title: difficultyTitle not assigned!");
		if(difficultyDescription){
			difficultyDescription.text = gameInfo.refDifficultyParameters[selectedDifficulty].difficultyDescription;
		}else Debug.Log ("Can't update difficulty description: difficultyDescription not assigned!");
	}


	public void UpdateLockDifficultyInfo(){
		if(gameInfo == null){
			Debug.Log ("current GameInfo component is empty, can't update interface by its data!");
			return;
		}
		if(lockDifficultyTitle){
			lockDifficultyTitle.text = gameInfo.lockDifficultyDescription[selectedQuestDifficulty].lockDifficultyName;
		}else Debug.Log ("Can't update lock difficulty title: lockDifficultyTitle not assigned!");
		if(lockDifficultyDescription){
			lockDifficultyDescription.text = gameInfo.lockDifficultyDescription[selectedQuestDifficulty].lockDifficultyDesciption;
		}else Debug.Log ("Can't update lock difficulty description: lockDifficultyDescription not assigned!");
	}
}
