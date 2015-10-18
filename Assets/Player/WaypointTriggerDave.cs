using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaypointTriggerDave : MonoBehaviour {

    public List<GameObject> neighbours;
    public GameObject mapShadow;
    public bool changeMonsterHeading;

    bool _setMiddleWaypoint = false;

    // Use this for initialization
    void Start ()
    {
        this.GetComponent<Collider>().enabled = true;
        changeMonsterHeading = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _setMiddleWaypoint = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            Vector3 playerHeading = (player.nextWaypoint.transform.position - player.lastWaypoint.transform.position).normalized;
            Vector3 playerTargetHeading = Vector3.zero;
            switch (player.getWalkState())
            {
                case PlayerWalkState.left:
                    playerTargetHeading = (Quaternion.AngleAxis(-90, Vector3.up) * playerHeading).normalized;
                    break;
                case PlayerWalkState.right:
                    playerTargetHeading = (Quaternion.AngleAxis(90, Vector3.up) * playerHeading).normalized;
                    break;
                case PlayerWalkState.forward:
                    playerTargetHeading = (Quaternion.AngleAxis(0, Vector3.up) * playerHeading).normalized;
                    break;
                default:
                    break;
            }
            if (neighbours.Count > 0)
            {
                GameObject bestWaypointMatch = neighbours
                    .Where(waypoint => Mathf.Abs(Vector3.Angle(playerTargetHeading, WaypointHeadingFromPlayer(player.nextWaypoint, waypoint))) < 90)
                    .OrderBy(waypoint => Mathf.Abs(Vector3.Angle(playerTargetHeading, WaypointHeadingFromPlayer(player.nextWaypoint, waypoint))))
                    .FirstOrDefault();
                if (bestWaypointMatch != null)
                {
                    if (!_setMiddleWaypoint)
                    {
                        if (player.getWalkState() == PlayerWalkState.forward)
                        {
                            player.setWaypoint(bestWaypointMatch);
                            _setMiddleWaypoint = true;
                        }
                        else
                        {
                            player.setWaypointFront(bestWaypointMatch);
                            this.transform.parent.GetComponent<Waypoint>().disableTriggers();
                        }
                    }
                    else
                    {
                        player.setWaypoint(bestWaypointMatch);
                        this.transform.parent.GetComponent<Waypoint>().disableTriggers();
                    }
                    mapShadow.GetComponent<Renderer>().enabled = false; // Hide the shadow on the map view
                }
            }
        }
        else if (other.tag == "Monster" && changeMonsterHeading)
        {
            other.GetComponent<MonsterAI>().changeHeadingDirectional(neighbours.ToArray());
            changeMonsterHeading = false;
        }
    }

    Vector3 WaypointHeadingFromPlayer(GameObject playerNextWaypoint, GameObject candidateWaypoint)
    {
        return (candidateWaypoint.transform.position - playerNextWaypoint.transform.position).normalized;
    }
}
