using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {
    GameObject player;

    public float finalRotation; // Within (-90, 90)
    int test = 0;

    // Use this for initialization
    void Start () {

        // find player object
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        float distance = (player.transform.position - this.transform.position).magnitude;
        float yRotate = this.transform.rotation.y;

        
        // if player is close enough, open the door. Never close it
        if (distance <= 5)
        {
            if (finalRotation > 0 && yRotate < finalRotation) // 1 to 90
            {
                this.transform.Rotate(0f, 1f, 0f, Space.World);
            } else if(finalRotation < 0 && yRotate > finalRotation) // -1 to -90
            {
                this.transform.Rotate(0f, -1f, 0f, Space.World);
            } else
            {
                if (test > 0)
                    UnityEditor.EditorUtility.DisplayDialog("TEST", "" + yRotate, "OK");
            }
        }
    }
    
}
