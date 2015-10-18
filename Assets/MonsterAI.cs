﻿using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

	// if the monster is this amount of distance away, have it delay teleport to the player's location
	public float maxTeleportDistance;

	// need to check that the player is at least this far away before teleporting in.
	public float minTeleportDistance;

	// how far the monster can see towards the prey
	public float sightLineRange;

	// how fast the monster will accelerate towards the player when the monster sees the player
	public float acceleration;

	// how fast the monster slows down when it can't see the player
	public float deceleration;

	// the slowest the monster can go
	public float startSpeed;

	// the fastest the monster can go
	public float maxSpeed;

	// how long to wait before teleporting in
	public float minWaitDuration;
	public float maxWaitDuration;

	// whether the monster is actively looking for the player or waiting to teleport in
	private bool monsterActive;

	//AKA the player
	public GameObject prey;
	//this should always be a valid heading between waypoints.
	//hitting a waypoint will update this
	public Vector3 heading;

	private RaycastHit sightLineHit;
	private Ray sightLine;
	private float actualSpeed;
	private bool canSeePrey;
	private Vector3 preyHeading;

	private float waitTime;
	private Vector3 teleportDest; 

	// Use this for initialization
	void Start () {
		deactivateMonster ();
		canSeePrey = false;
	}

	public void changeHeadingDirectional(GameObject[] waypoints) 
	{
		if (canSeePrey)
		{
			//set the heading to backwards in case there's no non-null waypoints
			Vector3 curHeading = heading*-1;
			float curAngle = Vector3.Angle(curHeading, preyHeading);
			for (int i=0; i<4; i++)
			{
				if (waypoints[i]!=null)
				{
					Vector3 waypointHeading = (waypoints[i].GetComponent<WaypointTriggerDirectional>().transform.position - transform.position).normalized;
					float angle = Vector3.Angle(waypointHeading, preyHeading);
					if (angle < curAngle)
					{
						curHeading = waypointHeading;
					}
				}
			}
			heading = curHeading;
		}
		else 
		{
			int directionIndex = Random.Range (0, 4);
			for (int i=0; i<4; i=(i+1)%4)
			{
				if (waypoints[i]!=null)
				{
					Vector3 waypointHeading = (waypoints[i].GetComponent<WaypointTriggerDirectional>().transform.position - transform.position).normalized;
					heading = waypointHeading;
					break;
				}
			}
		}
	
	}

	public void changeHeadingLinear(GameObject waypoint)
	{
		heading = (waypoint.transform.position - transform.position).normalized;
	}

	// Update is called once per frame
	void Update () 
	{
		if (monsterActive) 
		{
			//Within range of the prey, don't teleport
			if (Vector3.Distance (transform.position, prey.transform.position) <= maxTeleportDistance) 
			{
				Vector3 preyDirection = prey.transform.position - transform.position;
				preyHeading = preyDirection.normalized;
				//try to get a line of sight
				if (Physics.Raycast(transform.position, preyDirection, out sightLineHit, sightLineRange)) 
				{
					//Debug.Log("has line of sight");
					// Line of sight to prey established
					if (sightLineHit.transform == prey.transform) 
					{
						//Debug.Log("has line of sight to prey");
						canSeePrey = true;
						//speed up to max speed
						actualSpeed = Mathf.Min(maxSpeed, actualSpeed + acceleration * Time.deltaTime);
						transform.Translate(heading * actualSpeed * Time.deltaTime);
					}
					
					// no line of sight to prey
					else {
						//slow down until it reaches default speed
						canSeePrey = false;
						actualSpeed = Mathf.Max(startSpeed, actualSpeed - deceleration * Time.deltaTime);
						transform.Translate(heading * actualSpeed * Time.deltaTime);
					}
				}
			} 
			//outside range of the prey, teleport out
			else 
			{
				deactivateMonster();
			}
		} 
		// the monster is inactive and wants to teleport in
		else 
		{
			if (Time.time > waitTime && Vector3.Distance(prey.transform.position, teleportDest) > minTeleportDistance) 
			{
				activateMonster();
			}
		}
	}

	void deactivateMonster()
	{
		//teleport below the map
		transform.position = transform.position + Vector3.down * 2*sightLineRange;
		monsterActive = false;
		waitTime = Time.time + Random.Range(minWaitDuration,maxWaitDuration);
		teleportDest = prey.transform.position;
	}

	void activateMonster()
	{
		transform.position = teleportDest;
		heading = (prey.GetComponent<Player>().nextWaypoint.transform.position - transform.position).normalized;
		monsterActive = true;
	}
}
