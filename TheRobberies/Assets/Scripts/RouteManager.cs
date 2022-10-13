using UnityEngine;
using System.Collections;

public class RouteManager : MonoBehaviour {
	public bool drawRoutes = true;
	public float waypointsSize = 0.3f;
	public Route[] refRoutes = new Route[0];
	public static Route[] routes = new Route[0];
	int i,j;
	Vector3 gizmoSize = new Vector3(0.3f,0.3f,0.3f);

	// Use this for initialization
	void Start () {
		if(refRoutes.Length>0)
			routes = refRoutes;
	}
	
	// Update is called once per frame
	void OnDrawGizmos () {
		if(refRoutes.Length>0)
			routes = refRoutes;
		if(drawRoutes)
			DrawRoutes ();
	}


	void DrawRoutes(){
		gizmoSize = new Vector3(waypointsSize,waypointsSize,waypointsSize);
		for(i=0;i<routes.Length;i++){
			if(routes[i].showRoute == false)
				continue;
			if(routes[i].waypoints.Length>1){

				for(j=0;j<routes[i].waypoints.Length;j++){
					if(j<routes[i].waypoints.Length-1){
						Gizmos.color = routes[i].color;
						Gizmos.DrawLine(routes[i].waypoints[j].position,routes[i].waypoints[j+1].position);
						Gizmos.DrawCube(routes[i].waypoints[j].position,gizmoSize);
						Gizmos.DrawCube(routes[i].waypoints[j+1].position,gizmoSize);
						//Debug.Log ("Drawing routes: from "+routes[i].waypoints[j].name+" to "+routes[i].waypoints[j+1].name);
					}
				}
			}
		}
	}


	public static void GetRouteWaypoint(RouteData curRouteData){

		if(curRouteData.routeID<0){
			Debug.Log ("Invalid route id:"+curRouteData.routeID);
			curRouteData.curWaypointID = -1;
			curRouteData.waypointPos = Vector3.zero;
			return;
		}
		if(routes.Length<=curRouteData.routeID){
			Debug.Log ("Invalid route id:"+curRouteData.routeID+", routes length <= routeID");
			curRouteData.curWaypointID = -1;
			curRouteData.waypointPos = Vector3.zero;
			return;
		}
		if(routes[curRouteData.routeID].waypoints.Length<0){
			Debug.Log ("Route with ID "+curRouteData.routeID+" don't has any waypoints!");
			curRouteData.curWaypointID = -1;
			curRouteData.waypointPos = Vector3.zero;
			return;
		}
		if(curRouteData.curWaypointID<0){
			if(curRouteData.direction>=0)
				curRouteData.curWaypointID = 0;
			else
				curRouteData.curWaypointID = routes[curRouteData.routeID].waypoints.Length-1;
		}else{
			if(curRouteData.direction<0)
				curRouteData.curWaypointID --;
			else 
				curRouteData.curWaypointID++;
			if(curRouteData.autoReverse){
				if(curRouteData.curWaypointID<0){
					curRouteData.curWaypointID = 1;
					curRouteData.direction*=-1;
				}else if(curRouteData.curWaypointID >routes[curRouteData.routeID].waypoints.Length-1){
					curRouteData.curWaypointID = routes[curRouteData.routeID].waypoints.Length-2;
					curRouteData.direction*=-1;
				}
			}
			curRouteData.curWaypointID = Mathf.Clamp (curRouteData.curWaypointID,0,routes[curRouteData.routeID].waypoints.Length-1);
			curRouteData.waypointPos = routes[curRouteData.routeID].waypoints[curRouteData.curWaypointID].position;
		}

	}


	public static int GetRouteID(string routeName){
		int result = -1;
		if(routes.Length<1){
			Debug.Log ("Can't get route ID: routes length <1!");
			return result;
		}
		for(int i=0;i<routes.Length;i++){
			if(routes[i].name == routeName){
				result = i;
				break;
			}
		}
		return result;
	}

	


	[System.Serializable]
	public class Route
	{
		public Color color = Color.green;
		public Transform[] waypoints = new Transform[0];
		public string name = "";
		public bool showRoute = true;

		public Route(){
			color = Color.green;
			waypoints = new Transform[0];
			name = "";
			showRoute = true;
		}

		public Route(Color routeColor,Transform[] routeWaypoints,string routeName){
			color = routeColor;
			waypoints = routeWaypoints;
			name = routeName;
			showRoute = true;
		}
	}


	[System.Serializable]
	public class RouteData
	{
		public string routeName = "";
		public bool autoReverse = true;
		[HideInInspector]
		public int curWaypointID = -1;
		public int direction = 1;
		[HideInInspector]
		public int routeID = -1;
		public Vector3 waypointPos = Vector3.zero;

		public void ResetRoute(){
			routeID = -1;
			curWaypointID = -1;
			waypointPos = Vector3.zero;
		}
	}
}
