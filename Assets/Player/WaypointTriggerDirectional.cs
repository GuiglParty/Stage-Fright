using UnityEngine;
using System.Collections;

public class WaypointTriggerDirectional : MonoBehaviour {
	public GameObject nextWaypointLeft;
	public GameObject nextWaypointRight;
	
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Collider>().enabled = true;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
        //check if the colliding object is the player
		if(other.tag == "Player" /*&& other.GetComponent<Player>().nextWaypoint == this.transform.parent.gameObject*/ ) {
			
			if(other.GetComponent<Player>().getWalkState() == PlayerWalkState.left) {
				other.GetComponent<Player>().setWaypoint(nextWaypointLeft);
				this.GetComponent<Collider>().enabled = false;
			}
			
			if(other.GetComponent<Player>().getWalkState() == PlayerWalkState.right) {
				other.GetComponent<Player>().setWaypoint(nextWaypointRight);
				this.GetComponent<Collider>().enabled = false;
			}
		}
    }
}
