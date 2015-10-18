using UnityEngine;
using System.Collections;

public class MonsterTrigger : MonoBehaviour {

	private bool caughtPlayer;
	private float waitTime;
	public float deathDelay;

	// Use this for initialization
	void Start () {
		caughtPlayer = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			caughtPlayer = false;
			waitTime = Time.time + deathDelay;
		}
	}

	// Update is called once per frame
	void Update () {
		if (caughtPlayer && Time.time > waitTime) 
		{
			Application.LoadLevel(Application.loadedLevel);
		}

	}
}
