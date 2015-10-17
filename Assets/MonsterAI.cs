using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

	public float teleportRange;
	public float acceleration;
	public float deceleration;
	public float startSpeed;
	public float maxSpeed;
	public GameObject prey;
	//this should always be a valid heading between waypoints.
	//hitting a waypoint will update this
	public Vector3 heading;

	private RaycastHit sightLineHit;
	private Ray sightLine;
	private float actualSpeed;
	public bool canSeePrey;
	private Vector3 preyHeading;




	// Use this for initialization
	void Start () {
		
	}

	public void changeHeadingDirectional(GameObject waypointLeft, GameObject waypointRight) 
	{
		Vector3 headingLeft = (waypointLeft.transform.position - transform.position).normalized;
		Vector3 headingRight = (waypointRight.transform.position - transform.position).normalized;

		if (canSeePrey)
		{
			float differenceLeft = (preyHeading - headingLeft).magnitude;
			float differenceRight = (preyHeading - headingRight).magnitude;
			if (differenceLeft < differenceRight)
			{
				heading = headingLeft;
			}
			else 
			{
				heading = headingRight;
			}
		}
		else 
		{
			int takeLeft = Random.Range (0, 2);
			if (takeLeft == 1) heading = headingLeft;
			else heading = headingRight;
		}
	
	}

	public void changeHeadingLinear(GameObject waypoint)
	{
		heading = (waypoint.transform.position - transform.position).normalized;
	}

	// Update is called once per frame
	void Update () 
	{
		//Within range of the prey, don't teleport
		if (Vector3.Distance (transform.position, prey.transform.position) <= teleportRange) 
		{
			Debug.Log("in range");
			Vector3 preyDirection = prey.transform.position - transform.position;
			float preyDistance = preyDirection.magnitude;
			//try to get a line of sight
			if (Physics.Raycast(transform.position, preyDirection, out sightLineHit, teleportRange)) 
			{
				//Debug.Log("has line of sight");
				// Line of sight to prey established
				if (sightLineHit.transform == prey.transform) 
				{
					//Debug.Log("has line of sight to prey");
					preyHeading = preyDirection.normalized;
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
		//outside range of the prey, teleport to prey
		else 
		{

		}
	}
}
