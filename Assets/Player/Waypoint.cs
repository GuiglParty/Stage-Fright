using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Waypoint : MonoBehaviour {
	GameObject player;
	
	public List<GameObject> triggers; // the movement triggers associated with this object
	public float resetDistance = 22; // how far the player needs to be from the waypoint to re-enable the triggers
	
	// Use this for initialization
	void Start () {
		// disable the renders used for editor visibility
		foreach( GameObject trigger in triggers ) {
			trigger.GetComponent<Renderer>().enabled = false;
		}
		
		// find player object
		player = GameObject.FindWithTag("Player");
		
	}
	
	// Update is called once per frame
	void Update () {
		var heading = player.transform.position - this.transform.position;
		var distance = heading.magnitude;
	
		// check if the player is far enough away to reset triggers
		if(distance > resetDistance) {
			foreach( GameObject trigger in triggers ) {
				trigger.GetComponent<Collider>().enabled = true;
			}
		}
	}
	
	public void disableTriggers () {
		foreach( GameObject trigger in triggers ) {
			trigger.GetComponent<Collider>().enabled = false;
		}
	}
}
