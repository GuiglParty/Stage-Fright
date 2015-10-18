using UnityEngine;
using System.Collections;

public class DeadEndScript : MonoBehaviour {
	public bool changeMonsterHeading;
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Collider>().enabled = true;
		changeMonsterHeading = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Monster" && changeMonsterHeading) 
		{
			MonsterAI monster = other.GetComponent<MonsterAI>();
			//reverse direction
			monster.changeHeading(monster.heading*-1);
			changeMonsterHeading = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Monster") 
		{
			changeMonsterHeading = true;
		}
	}
}
