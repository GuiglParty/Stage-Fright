using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour {
    bool occupied;
    Vector3 returnPosition;
    Quaternion returnRotation;

    // Use this for initialization
    void Start()
    {
        occupied = false;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void SetReturnPosition(Vector3 pos)
    {
        returnPosition = pos;
    }

    public Vector3 GetReturnPosition()
    {
        return returnPosition;
    }

    public void SetReturnRotation(Quaternion rot)
    {
        returnRotation = rot;
    }

    public Quaternion GetReturnRotation()
    {
        return returnRotation;
    }


    public void SetOccupied(bool val)
    {
        occupied = val;
    }

    public bool GetOccupied()
    {
        return occupied;
    }
}
