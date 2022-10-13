using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SymbolLock : MonoBehaviour {
	public Transform interactiveSafeObject,normalLockTransform;
	public LockDataLine[] lockLines = new LockDataLine[0];
	public int lineID = 0;
	public float maxBlockHeight = 90f,minBlockHeight = 0f,blockHeightStep = 30f;
	public LockDifficulty lockNormalDifficulty = new LockDifficulty();
	public LockLineCellsRects[] lockLinesUIData = new LockLineCellsRects[0];
	public LockLineCellsRects controlLine = new LockLineCellsRects();
	public int[] unlockedCombination = new int[0];
	Transform thisTransform;
	public bool unlocked = false;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;

	}
	
	void OnEnable(){
		RandomizeLock ();
		UpdateControlLineUI();
		RandomizeLinesStates();
		UpdateLinesUIData();
	}

	void Update(){
		if(LevelInfo.gameOver || LevelInfo.missionComplete)
			if(interactiveSafeObject)
				interactiveSafeObject.SendMessage("HackingDisabled",unlocked,SendMessageOptions.DontRequireReceiver);
	}
	
	void SetSafeTransform(Transform safeObjectTransform){
		interactiveSafeObject = safeObjectTransform;
	}

	public void UpdateLinesUIData(){
		for(int i=0;i<lockLines.Length;i++){
			if(lockLines[i].cells.Length>0){
				for(int j=0;j<lockLines[i].cells.Length;j++){
					lockLinesUIData[i].cellsRects[j].sizeDelta = new Vector2(40, lockLines[i].cells[j].size);
				}
			}
		}
	}


	public void UpdateControlLineUI(){
		Vector2 tempSize = new Vector2(40,0);
		for(int i=0;i<lockLines[0].cells.Length;i++){
			tempSize.y = 0f;
			for(int j=0;j<lockLines.Length;j++){
				tempSize.y += lockLines[j].cells[i].size;
			}
			controlLine.cellsRects[i].sizeDelta = tempSize;
		}
	}

	public void RandomizeLock(){
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		unlockedCombination = new int[lockLines.Length];
		for(int i=0;i<lockLines.Length;i++){
			lockLines[i].SetRandomState();
			lockLines[i].RandomizeCellsData(minBlockHeight,maxBlockHeight,blockHeightStep);
			unlockedCombination[i] = lockLines[i].state;
		}
	}

	public void RandomizeLinesStates(){
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		for(int i=0;i<lockLines.Length;i++){
			lockLines[i].SetRandomState();
		}
	}
	

	public void TryToUnlock(){
		if(CanBeUnlocked())
			unlocked = true;
		if(interactiveSafeObject)
			interactiveSafeObject.SendMessage("HackingDisabled",unlocked,SendMessageOptions.DontRequireReceiver);
		
	}


	public bool CanBeUnlocked(){
		bool result = true;
		float curCellsSumm = 0f;
		for(int i=0;i<lockLines[0].cells.Length;i++){
			curCellsSumm = 0f;
			for(int j=0;j<lockLines.Length;j++){
				curCellsSumm += lockLines[j].cells[i].size;
			}
			if(controlLine.cellsRects[i].sizeDelta.y != curCellsSumm){
				result = false;
				break;
			}
		}
		return result;
	}


	public void RollLineWithID(int newLineID){
		int lineID = (Mathf.Abs (newLineID)-1)*(int)Mathf.Sign (newLineID);
		//Debug.Log ("cur line ID:"+lineID);
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		if(lockLines.Length<=Mathf.Abs (lineID)){
			Debug.Log ("Invalid line ID: lineID>=lockLines.Length");
			return;
		}
		if(lockLines[Mathf.Abs (lineID)] == null){
			Debug.Log ("Line with ID "+Mathf.Abs (lineID)+" is empty!");
			return;
		}
		lockLines[Mathf.Abs (lineID)].RollLine(1*(int)Mathf.Sign (newLineID));
		UpdateLinesUIData();
	}
	
	public void RollLine(int rollStep){
		if(lineID<0){
			Debug.Log ("Invalid line ID: lineID<0!");
			return;
		}
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		if(lockLines.Length<=lineID){
			Debug.Log ("Invalid line ID: lineID>=lockLines.Length");
			return;
		}
		if(lockLines[lineID] == null){
			Debug.Log ("Line with ID "+lineID+" is empty!");
			return;
		}
		lockLines[lineID].RollLine(rollStep);
		UpdateLinesUIData();
	}


	public void RandomizeLineState(){
		if(lineID<0){
			Debug.Log ("Invalid line ID: lineID<0!");
			return;
		}
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		if(lockLines.Length<=lineID){
			Debug.Log ("Invalid line ID: lineID>=lockLines.Length");
			return;
		}
		if(lockLines[lineID] == null){
			Debug.Log ("Line with ID "+lineID+" is empty!");
			return;
		}
		lockLines[lineID].SetRandomState();
	}


	public void SetLineState(int lineState){
		if(lineID<0){
			Debug.Log ("Invalid line ID: lineID<0!");
			return;
		}
		if(lockLines.Length<1){
			Debug.Log ("Lock lines array length < 1!");
			return;
		}
		if(lockLines.Length<=lineID){
			Debug.Log ("Invalid line ID: lineID>=lockLines.Length");
			return;
		}
		if(lockLines[lineID] == null){
			Debug.Log ("Line with ID "+lineID+" is empty!");
			return;
		}
		lockLines[lineID].SetState(lineState);
	}
	
	
	[System.Serializable]
	public class LockDataCell
	{
		public float size = 0f;
		public int symbolID = 0;
		
		public LockDataCell(float newSize,int newSymbolID){
			size = newSize;
			symbolID = newSymbolID;
		}

		public void SetValues(float newSize,int newSymbolID){
			size = newSize;
			symbolID = newSymbolID;
		}
	}
	
	
	[System.Serializable]
	public class LockDataLine
	{
		public LockDataCell[] cells = new LockDataCell[0];
		public int state = 0;
		
		public void RollLine(int step){
			if(cells.Length<1){
				Debug.Log ("Can't do roll to "+step+": cells length if this line <1!");
				return;
			}
			if(step == 0)
				return;
			LockDataCell tempCell = new LockDataCell(0f,0);
			for(int i = 0;i<Mathf.Abs (step);i++){
				for(int j=0;j<cells.Length;j++){
					if(j==0 ){
						if(step<0)
							tempCell.SetValues(cells[j].size,cells[j].symbolID);
						else if(step>0)
							tempCell.SetValues(cells[cells.Length-1].size,cells[cells.Length-1].symbolID);
					}else{
						if(step<0){
							cells[j-1].SetValues(cells[j].size,cells[j].symbolID);
						}else{
							cells[cells.Length-j].SetValues (cells[cells.Length-j-1].size,cells[cells.Length-j-1].symbolID);
						}
					}
					
				}
				if(step<0){
					cells[cells.Length-1].SetValues(tempCell.size,tempCell.symbolID);
					UpdateState(-1);
				}else if(step>0){
					cells[0].SetValues(tempCell.size,tempCell.symbolID);
					UpdateState(1);
				}
			}
			//UpdateState(step);
			
		}

		public void UpdateState(int rollStep){
			state+=rollStep;
			if(Mathf.Abs (state)>=cells.Length || cells.Length<1)
				state = 0;
		}


		public void SetRandomState(){
			if(cells.Length<1){
				Debug.Log ("Can't set any state: cells length<1!");
				return;
			}
			int randomRollCount = Random.Range (0,cells.Length);
			if(Random.value>0.5f)
				randomRollCount*=-1;
			RollLine(randomRollCount);
		}


		public void SetState(int desiredState){
			if(cells.Length<1){
				Debug.Log ("Can't set line to state "+desiredState+": cells length<1!");
				return;
			}
			if(Mathf.Abs (desiredState)>cells.Length-1){
				Debug.Log ("Can't set line to state "+desiredState+": invalid state id (desiredState>cells length-1!)");
				return;
			}
			int rollStep = 1*(int)Mathf.Sign (desiredState);
			for(int i=0;i<cells.Length;i++){
				RollLine(rollStep);
				if(state == desiredState)
					break;
			}
		}

		public void RandomizeCellsData(float minHeight,float maxHeight,float heightStep){
			if(cells.Length<1){
				Debug.Log ("Can't randomize cells data: cells length<1!");
				return;
			}
			int availableHeightSteps;
			//Debug.Log ("available steps:"+availableHeightSteps);
			for(int i=0;i<cells.Length;i++){
				if(cells[i]!=null){
					cells[i].size = 0f;
					availableHeightSteps = Mathf.RoundToInt(maxHeight/heightStep);
					availableHeightSteps = Random.Range (1,availableHeightSteps);
					if(Random.value<0.2f)
						availableHeightSteps = 0;
					for(int j=0;j<availableHeightSteps;j++){
						cells[i].size+=heightStep;
					}
					cells[i].size = Mathf.Clamp (cells[i].size,minHeight,maxHeight);

				}else Debug.Log ("Can't randomize cell data: cell is empty!");
			}
		}
	}


	[System.Serializable]
	public class LockDifficulty
	{
		public string difficultyName = "Normal";
		public int linesCount = 3;
		public int cellsCount = 5;
	}


	[System.Serializable]
	public class LockLineCellsRects
	{
		public RectTransform[] cellsRects = new RectTransform[0];
	}
}
