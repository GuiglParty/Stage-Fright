using UnityEngine;
using System.Collections;

public class HidingSpotTrigger : MonoBehaviour {
    public GameObject hidingSpot;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("hiding spot on trigger stay");
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            HidingSpot spot = hidingSpot.GetComponent<HidingSpot>();

            if (player.getPlayerState() == PlayerState.lookingAround)
            {
                if (!spot.GetOccupied())
                {
                    spot.SetReturnPosition(player.transform.position);
                    spot.SetReturnRotation(player.transform.rotation);
                    spot.SetOccupied(true);
                    player.transform.position = spot.transform.position;
                    player.transform.rotation = spot.transform.rotation;
                    player.hiding = true;
                }
            }
            else
            {
                if (spot.GetOccupied())
                {
                    player.transform.position = spot.GetReturnPosition();
                    player.transform.rotation = spot.GetReturnRotation();
                    spot.SetOccupied(false);
                    player.hiding = false;
                }
            }
        }
    }
}
