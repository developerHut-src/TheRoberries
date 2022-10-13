using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldInfo : MonoBehaviour {
	public float checkInterval = 0.5f;
	public Material[] skinMaterials = new Material[0];
	public static Material[] sMaterials = new Material[0];
	float checkTime = 0f,scoreTableUpdateTime = 0f;
	public static float curTime = 0f;
	public static GameEvent[] events = new GameEvent[0];
	public static GlobalPlayerInfo[] players = new GlobalPlayerInfo[0];
	public static Transform objectTransform;
	public static SpectatorInfo spectatorInfo = new SpectatorInfo("No one subject to spectate",0f,0,0f,0f,0f,0,0f,"none",new string[]{"undefined","undefined"},0f);
	public Dictionary<string,LearningData> learningData = new Dictionary<string,LearningData>();
	public static bool enableSpectation = true;
	bool showScoreTable = false;
	public bool spectate = false;
	Rect specInfoPos,scoreTablePos;
	string scoreTableInfo = "";
	int i;
	/*
	//Sound replics properties====
	public NbotSounds _nbotSounds = new NbotSounds();
	public static NbotSounds nbotSounds = new NbotSounds();
	//============================
	*/

	void Awake(){
		ResetWorldInfo();
		objectTransform = transform;
		//sMaterials = skinMaterials;
		//nbotSounds = _nbotSounds;
	}

	public static void ResetWorldInfo(){
		sMaterials = new Material[0];
		curTime = 0f;
		events = new GameEvent[0];
		players = new GlobalPlayerInfo[0];
		objectTransform = null;
		spectatorInfo = new SpectatorInfo("No one subject to spectate",0f,0,0f,0f,0f,0,0f,"none",new string[]{"undefined","undefined"},0f);
		enableSpectation = true;
	}

	// Use this for initialization
	void Start () {
		specInfoPos = new Rect(Screen.width-240,10,240,220);
		scoreTablePos = new Rect(Screen.width*0.5f-150,Screen.height*0.5f-150,300,300);

		if(spectate)
			enableSpectation = true;
		else enableSpectation = false;

		if(!enableSpectation)
			DisableSpectation();
	}
	
	// Update is called once per frame
	void Update () {
		curTime = Time.time;
	if(checkTime<=curTime){
			CheckEvents ();
			checkTime = curTime+checkInterval;
		//	Debug.Log ("total events:"+events.Length);
		}
		if(Input.GetKey (KeyCode.Tab)){
			UpdateScoreTableInfo();
			showScoreTable = true;
		}
		else
			showScoreTable = false;

	}


	void OnGUI(){
		if(enableSpectation)
		specInfoPos = GUI.Window(1,specInfoPos,SpectatorInfoWindow,"Spectator");
		if(showScoreTable)
			scoreTablePos = GUI.Window (2,scoreTablePos,ScoreTableWindow,"Score table");
	}

	void OnDrawGizmos(){
		if(events.Length<1)
			return;
		Gizmos.color = Color.green;
		for(i=0;i<events.Length;i++){
			Gizmos.DrawSphere (events[i].position,0.5f);
		}
	}


	void SpectatorInfoWindow(int id){
		GUILayout.Label (spectatorInfo.name+"\n"+
		"leadership:"+spectatorInfo.leadership.ToString()+"\n"+
		"hit points:"+spectatorInfo.hitPoints.ToString()+"\n"+
		"base accuracy:"+spectatorInfo.primAccuracy.ToString()+"\n"+
		"accuracy radius:"+spectatorInfo.secAccuracy.ToString ()+"\n"+
		"reaction speed:"+spectatorInfo.reactionSpeed.ToString ()+"\n"+
		"team ID:"+spectatorInfo.teamId.ToString ()+"\n"+
		"hearing radius:"+spectatorInfo.curHearingRadius.ToString ()+"\n"+
		"current FOV:"+spectatorInfo.fov.ToString ()+"\n"+
		"attack style:"+spectatorInfo.attackType+"\n"+
		"action layer 1:"+spectatorInfo.curActions[0]+"\n"+
		"action layer 2:"+spectatorInfo.curActions[1]);
		GUI.DragWindow();
	}


	void ScoreTableWindow(int id){
		GUILayout.Label ("Player name          kills deaths");
		GUILayout.Label (scoreTableInfo);
	}

	void UpdateScoreTableInfo(){
		scoreTableInfo = "";
		string plrName = "";
		for(int i=0;i<players.Length;i++){
			plrName = players[i].pTransform.name;
			if(plrName.Length>21)
				plrName.Remove(21);
			else plrName = plrName.PadRight(23);
			plrName +=("  "+players[i].kills.ToString()+"     "+players[i].deaths.ToString ()).PadRight(50);
			scoreTableInfo +=plrName+"\n";
	
		}
	}

	public static void AddEvent(GameEvent e,bool continuous){
		ArrayList tempEvents = new ArrayList();
		bool elementAdded = false;
		if(events.Length<1)
			tempEvents.Add (e);
		else{
			for(int i =0;i<events.Length;i++){
				if(events[i].sender == e.sender && continuous){
					events[i]=e;
					elementAdded = true;
				}
				tempEvents.Add (events[i]);
				if(elementAdded)
					break;
			}
			if(!elementAdded)
				tempEvents.Add (e);
		}
		events = (GameEvent[]) tempEvents.ToArray (typeof(GameEvent));
		//Debug.Log ("New event added: " +e.type+" from sender: "+e.sender);
	}

	void CheckEvents(){
		if(events.Length<1)
			return;
		ArrayList tempEvents = new ArrayList();
		for(int i =0;i<events.Length;i++){
			if(events[i].destrTime>curTime)
				tempEvents.Add (events[i]);
		}
		if(tempEvents.Count<1)
			events = new GameEvent[0];
		else
			events = (GameEvent[]) tempEvents.ToArray (typeof(GameEvent));
	}



	public static void RegisterPlayer(GlobalPlayerInfo playerInfo){
		if (players.Length < 1) {
			players = new GlobalPlayerInfo[1];
			players[0].pTransform = playerInfo.pTransform;
			players[0].teamId = playerInfo.teamId;
			players[0].alive = playerInfo.alive;
			//ApplySkinMaterial(playerInfo);
		Debug.Log ("Register player:"+playerInfo.pTransform.name+" team ID: "+playerInfo.teamId+" ,number of players:" + players.Length+", alive:"+playerInfo.alive);
			return;
				}
		ArrayList tempPlayers = new ArrayList();
		int i;
		for (i=0; i<players.Length; i++) {
			tempPlayers.Add (players[i]);
				}
		tempPlayers.Add (playerInfo);
		players = (GlobalPlayerInfo[])tempPlayers.ToArray (typeof(GlobalPlayerInfo));
		Debug.Log ("Register player:"+playerInfo.pTransform.name+" team ID: "+playerInfo.teamId+" ,number of players:" + players.Length+", alive:"+playerInfo.alive);
		//disable spectation for this player if spectation is prohibited
		playerInfo.pTransform.SendMessage("ChangeSpectatorView",false,SendMessageOptions.DontRequireReceiver);
		playerInfo.pTransform.SendMessage("SetSpectation",false,SendMessageOptions.DontRequireReceiver);
		//change skin material(optional)
		//ApplySkinMaterial(playerInfo);
	}


	public static void UnregisterPlayer(GlobalPlayerInfo playerInfo){
		if (players.Length ==1) {
			if(players[0].pTransform == playerInfo.pTransform)
			players = new GlobalPlayerInfo[0];
			Debug.Log ("Unregister player:"+playerInfo.pTransform.name+" team ID: "+playerInfo.teamId+"number of players:" + players.Length);
			return;
		}
		ArrayList tempPlayers = new ArrayList ();
		int i;
		for (i=0; i<players.Length; i++) {
			if(players[i].pTransform!=playerInfo.pTransform)
			tempPlayers.Add (players[i]);
		}
		players = (GlobalPlayerInfo[])tempPlayers.ToArray (typeof(GlobalPlayerInfo));
		Debug.Log ("Unregister player:"+playerInfo.pTransform.name+" team ID: "+playerInfo.teamId+" ,number of players:" + players.Length);
		ResetPlayersIDsForAll ();
	}

	public static void ResetPlayersIDsForAll(){
		if(players.Length<1)
			return;
		for(int i=0;i<players.Length;i++){
			players[i].pTransform.SendMessage("ResetPlayersIds",SendMessageOptions.DontRequireReceiver);
		}
	}

	public static void ChangeStatus(GlobalPlayerInfo playerInfo,bool autoRegistration){
		int index = -1;
		index = GetPlayerID (playerInfo.pTransform);
		if(index ==-1 && playerInfo.alive == false && !autoRegistration){
			Debug.Log ("Can't change status of player "+playerInfo.pTransform.name+" as 'DEAD': player was not registered");
			return;
		}
		if(index>-1){
			players[index].alive = playerInfo.alive;
		}else if(autoRegistration){
			RegisterPlayer(playerInfo);
		}
	}

	//this method returns ID(index in players array) of specified player by its transform
	public static int GetPlayerID(Transform playerT){
		int id = -1;
		if(players.Length<1){
			Debug.Log ("Can't get players ID: no one player registered!");
				return id;
		}
		for(int i=0;i<players.Length;i++){
			if(players[i].pTransform == playerT){
				id = i;
				break;
			}
		}
		return id;
	}

	public static void AddKillScore(string name,short s){
		if(players.Length<1){
			Debug.Log ("Can't add kill score: no one player registered!");
			return;
		}
		bool killPointAdded = false;
		for(int i = 0;i<players.Length;i++){
			if(players[i].pTransform.name == name){
				players[i].kills+=s;
				killPointAdded = true;
				break;
			}
		}
		if(!killPointAdded)
			Debug.Log ("Can't add kill point for name '"+name+"'!");
	}

	public static void AddDeathsScore(string name,short s){
		if(players.Length<1){
			Debug.Log ("Can't add kill score: no one player registered!");
			return;
		}
		for(int i = 0;i<players.Length;i++){
			if(players[i].pTransform.name == name){
				players[i].deaths+=s;
			}
		}
	}
	

	public static void DisableSpectationForOthers(int curT){
		for (int i =0; i<WorldInfo.players.Length; i++) {
			if(i!=curT){
				WorldInfo.players[i].pTransform.SendMessage("ChangeSpectatorView",false,SendMessageOptions.DontRequireReceiver);
				WorldInfo.players[i].pTransform.SendMessage("SetSpectation",false,SendMessageOptions.DontRequireReceiver);
			}
		}
	}


	public static void DisableSpectation(){
		enableSpectation = false;
		//disable spectation for all (-1 mean nothing to ignore)
		DisableSpectationForOthers(-1);

		if(objectTransform){
		Spectator spectator = objectTransform.GetComponent<Spectator>();
			if(spectator){
				spectator.spectatorCamera.gameObject.SetActive(false);
				AudioListener aL = spectator.spectatorCamera.GetComponent<AudioListener>();
				aL.enabled = false;
			}
		} else Debug.Log ("Can't disable third person camera, cos objectTransform not found!");
	}


	public static void EnableSpectation(){
		enableSpectation = true;
		if(!objectTransform){
			Debug.Log ("Object transform not found, spectation can't be enabled!");
			return;
		}
		Spectator spectator = objectTransform.GetComponent<Spectator>();
		if(spectator){
		spectator.spectatorCamera.gameObject.SetActive(true);
			AudioListener aL = spectator.spectatorCamera.GetComponent<AudioListener>();
			aL.enabled = true;
		}
		objectTransform.SendMessage ("RenewSpectation",SendMessageOptions.DontRequireReceiver);

	}


	public struct GameEvent{
		public string type;
		public float radius;
		public float destrTime;
		public Vector3 position;
		public string sender;

		public GameEvent(string newEventType, float effectRadius,float destructionTime,Vector3 eventPosition, string senderName){
			type = newEventType;
			radius = effectRadius;
			destrTime = destructionTime;
			position = eventPosition;
			sender = senderName;
		}

		public bool IsEmpty(){
			bool result = false;
			if(string.IsNullOrEmpty(type))
				result = true;
			if(radius<0)
				result = true;
			if(destrTime<0)
				result = true;
			if(string.IsNullOrEmpty(sender))
				result = true;
			return result;
		}
	}


	public struct GunShotParameters
	{
		public int damage;
		public Vector3 shotForce;
		public Vector3 hitPosition;
		public string hitObjectName;
		public string sender;

		public GunShotParameters(int d,Vector3 sF,Vector3 hP,string hON,string s){
			damage = d;
			shotForce = sF;
			hitPosition = hP;
			hitObjectName = hON;
			sender = s;
		}
	}

	public struct SpectatorInfo
	{
		public string name;
		public float leadership;
		public int hitPoints;
		public float primAccuracy;
		public float secAccuracy;
		public float reactionSpeed;
		public string[] curActions;
		public int teamId;
		public float fov;
		public string attackType;
		public float curHearingRadius;

		public SpectatorInfo(string n,float l,int hP,float pA,float sA,float rS,int tI,float FOV,string aT,string[] cA,float cHR){
			name = n;
			leadership = l;
			hitPoints = hP;
			primAccuracy = pA;
			secAccuracy = sA;
			reactionSpeed = rS;
			curActions = cA;
			teamId = tI;
			fov = FOV;
			attackType = aT;
			curHearingRadius = cHR;
		}
	}

	public struct LearningData
	{
		public Vector3[] warningPoints;
		public float minReaction,maxReaction;
	}

	public struct GlobalPlayerInfo
	{
		public Transform pTransform;
		public int teamId;
		public short kills,deaths;
		public bool alive;

		public GlobalPlayerInfo(Transform playerTransform,int teamID,bool Alive){
			pTransform = playerTransform;
			teamId = teamID;
			kills = 0;
			deaths = 0;
			alive = Alive;
		}
	}


	/*
	[System.Serializable]
	public class NbotSounds
	{
		public AudioClip[] heardSomething = new AudioClip[0];
		public AudioClip[] enemyDetected = new AudioClip[0];
		public AudioClip[] dead = new AudioClip[0];
		public AudioClip[] needHelp = new AudioClip[0];

	}
	*/
}
