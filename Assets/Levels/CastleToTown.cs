using UnityEngine;
using System.Collections;

public class CastleToTown : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			Application.LoadLevel("CrappyTown");
		}
	}
}
