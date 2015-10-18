using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    void OnTriggerEnter(Collider player) {
        Application.LoadLevel("CrappyTown");
    }
}
