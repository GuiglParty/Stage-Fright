using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

	public float teleportRange;
	public float acceleration;
	public float startSpeed;
	public GameObject prey;

	private RaycastHit sightLineHit;
	private Ray sightLine;
	private float actualSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.Distance (transform.position, prey.transform.position) <= teleportRange) 
		{
			Debug.Log("in range");
			Vector3 preyDirection = prey.transform.position - transform.position;
			float preyDistance = preyDirection.magnitude;
			//try to get a line of sight
			if (Physics.Raycast(transform.position, preyDirection, out sightLineHit, teleportRange)) 
			{
				//Debug.Log("has line of sight");

				if (sightLineHit.transform == prey.transform) 
				{
					//Debug.Log("has line of sight to prey");
					Vector3 heading = preyDirection / preyDistance;
					transform.Translate(heading * actualSpeed * Time.deltaTime);
				}
			}
		} 
		else 
		{

		}
	}
}
