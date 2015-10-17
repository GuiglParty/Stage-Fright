﻿using UnityEngine;
using System.Collections;

public class WaypointTriggerLinear : MonoBehaviour {
	public GameObject nextWaypoint;
	
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Collider>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
        //check if the colliding object is the player
		if(other.tag == "Player" /*&& other.GetComponent<Player>().nextWaypoint == this.transform.parent.gameObject*/ ) {
			Debug.Log("moving to new place");
			other.GetComponent<Player>().setWaypoint(nextWaypoint);
		}
    }
}