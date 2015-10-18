using UnityEngine;
using System.Collections;

public class WaypointTriggerDirectional : MonoBehaviour {
	public GameObject nextWaypointLeft;
	public GameObject nextWaypointRight;
	public GameObject nextWaypointMiddle;
    public GameObject mapShadow;
	public bool changeMonsterHeading;

	bool _setMiddleWaypoint = false;
	
    // Use this for initialization
    void Start () {
		this.GetComponent<Collider>().enabled = true;
		changeMonsterHeading = true;
	}
	
	void onEnable() {
		_setMiddleWaypoint = false;
	}
	
	void OnTriggerStay(Collider other) {

        //check if the colliding object is the player
		if (other.tag == "Player" /*&& other.GetComponent<Player>().nextWaypoint == this.transform.parent.gameObject*/) {

			if (other.GetComponent<Player> ().getWalkState () == PlayerWalkState.left && nextWaypointLeft != null) {
				if(_setMiddleWaypoint == false) {
					other.GetComponent<Player> ().setWaypoint (nextWaypointLeft);
				} else {
					other.GetComponent<Player> ().setWaypointFront (nextWaypointLeft);
				}
				this.transform.parent.GetComponent<Waypoint>().disableTriggers();
				mapShadow.GetComponent<Renderer> ().enabled = false; // Hide the shadow on the map view
			} else 
			if (other.GetComponent<Player> ().getWalkState () == PlayerWalkState.right && nextWaypointRight != null) {
				if(_setMiddleWaypoint == false) {
					other.GetComponent<Player> ().setWaypoint (nextWaypointRight);
				} else {
					other.GetComponent<Player> ().setWaypointFront (nextWaypointRight);
				}
				this.transform.parent.GetComponent<Waypoint>().disableTriggers();
				mapShadow.GetComponent<Renderer> ().enabled = false; // Hide the shadow on the map view
			} else 
			if (_setMiddleWaypoint == false && other.GetComponent<Player> ().getWalkState () == PlayerWalkState.forward && nextWaypointMiddle != null){
				other.GetComponent<Player> ().setWaypoint (nextWaypointMiddle);
				_setMiddleWaypoint = true;
				mapShadow.GetComponent<Renderer> ().enabled = false; // Hide the shadow on the map view
			}
			
		} else if (other.tag == "Monster" && changeMonsterHeading) {
			other.GetComponent<MonsterAI> ().changeHeadingDirectional (new GameObject[] { nextWaypointLeft, nextWaypointRight });
			changeMonsterHeading = false;
		}
    }

	void OnTriggerExit(Collider other) {
		if (other.tag == "Monster") {
			changeMonsterHeading = true;
		} else
		if (other.tag == "Player") {
			this.transform.parent.GetComponent<Waypoint>().disableTriggers();
		}
	}
}
