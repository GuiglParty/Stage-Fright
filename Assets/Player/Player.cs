﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public bool hiding = false;
    public bool wasHiding = false;
    public float walkSpeed = 5f; // Units moved per second, when running at full speed
	public float turnSpeed = 0.5f; // How quickly the view will spin when walking normally
	public GameObject nextWaypoint; // the next position that the player will move towards when walking
    public GameObject hidingSpot;

    public Camera firstPerson; // Used to navigate game
    public Camera birdsEye; // Used when viewing map

    public GameObject lastWaypoint; // the previous position that the player moved towards
	GameObject _intermediateWaypoint; // used to make movement more smooth and consistant
	PlayerState _ps;
	PlayerWalkState _pws;
	
	float _stepForce;
	
	// initialization
	void Start () {
	    _ps = PlayerState.walking;
		_pws = PlayerWalkState.forward;
		_stepForce = 1.0f;
		
		//lastWaypoint = nextWaypoint;
		_intermediateWaypoint = null;

        firstPerson.enabled = true;
        birdsEye.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            firstPerson.enabled = !firstPerson.enabled;
            birdsEye.enabled = !birdsEye.enabled;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _ps = PlayerState.lookingAround;
        }

        if (Input.GetButtonUp("Jump"))
        {
            _ps = PlayerState.walking;
        }

        if (Input.GetButtonDown("Fire1") && firstPerson.enabled) { // Move only when not viewing map
			_stepForce = 1.0f;
			_pws = PlayerWalkState.forward;
		}
	
		if (Input.GetAxis("Horizontal") < 0.0f) {
			_pws = PlayerWalkState.left;
		}
		
		if (Input.GetAxis("Horizontal") > 0.0f) {
			_pws = PlayerWalkState.right;
		}

        if ( _ps == PlayerState.lookingAround && hiding )
        {
            float hideStepForce = 1.2f;
            float hideTurnSpeed = 0.6f;

            Vector3 heading = hidingSpot.transform.position - this.transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            if (distance > 1)
            {
                this.GetComponent<CharacterController>().Move(direction * walkSpeed * hideStepForce * Time.deltaTime);
            }
            else
            {
                this.transform.position = hidingSpot.transform.position;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, hidingSpot.transform.rotation, hideTurnSpeed);
            }
        }
        else if ( _ps == PlayerState.walking && wasHiding )
        {
            float unhideStepForce = 0.5f;
            float unhideTurnSpeed = 0.1f;
            HidingSpot spot = hidingSpot.GetComponent<HidingSpot>();

            Vector3 heading = spot.GetReturnPosition() - this.transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            if (distance > 0.1f)
            {
                this.GetComponent<CharacterController>().Move(direction * walkSpeed * unhideStepForce * Time.deltaTime);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, spot.GetReturnRotation(), unhideTurnSpeed);
            }
            else
            {
                this.transform.position = spot.GetReturnPosition();
                wasHiding = false;
                hidingSpot = null;
            }
        }
		// Check what type of action the player object is currently undertaking
		else if( _ps == PlayerState.walking && nextWaypoint != null) {
			var heading = nextWaypoint.transform.position - this.transform.position;
			var distance = heading.magnitude;
			var direction = heading / distance;
			if( _intermediateWaypoint != null ){
				heading = _intermediateWaypoint.transform.position - this.transform.position;
				distance = heading.magnitude;
				direction = heading / distance;
			}

			// Check if we have received any input to move
			if( _pws == PlayerWalkState.forward ) {
				// moving forward, move towards nextWaypoint
				if( distance > 1 ) {
					this.GetComponent<CharacterController>().Move(direction * walkSpeed * _stepForce * Time.deltaTime);
				} else if ( _intermediateWaypoint != null) {
					_intermediateWaypoint = null;
				}
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), turnSpeed);
				// reduce the _stepForce for the next frame
				_stepForce = Mathf.Lerp(0.0f, _stepForce, 0.95f);
			}
		}
	}
	
	public PlayerWalkState getWalkState() {
		return _pws;
	}
	
	public PlayerState getPlayerState() {
		return _ps;
	}
	
	// timeDelta = time since last input given
	// stepForce = float between 0.0f and 1.0f. How hard the keys were hit
	public void moveForward(float timeDelta, float stepForce) {
		// change to forward motion
		_pws = PlayerWalkState.forward;
		
		// if the new stepforce is greater than the current value, then move that much faster.
		if(_stepForce < stepForce ) {
			_stepForce = stepForce;
		}
	}

    public void lookAround()
    {
        _ps = PlayerState.lookingAround;
    }

    public void stopLookingAround()
    {
        if (_ps == PlayerState.lookingAround && !Input.GetButton("Jump"))
        {
            _ps = PlayerState.walking;
        }
    }

    // 180 turn
    public void turnAround()
    {
		GameObject temp = lastWaypoint;
        lastWaypoint = nextWaypoint;
		nextWaypoint = temp;
    }
	
	public void turnLeft() {
		_pws = PlayerWalkState.left;
	}
	
	public void turnRight() {
		_pws = PlayerWalkState.right;
		
	}
	
	// tell the player which position to move to next
	public void setWaypoint( GameObject Waypoint ) {
        if (Waypoint != nextWaypoint)
        {
            lastWaypoint = nextWaypoint;
            _intermediateWaypoint = nextWaypoint;
            nextWaypoint = Waypoint;
        }
	}
	
	// change the next waypoint without changing the intermediate information
	public void setWaypointFront( GameObject Waypoint ) {
		nextWaypoint = Waypoint;
	}
}

// Used for determining the type of action the player object is taking
public enum PlayerState {
	walking, // Default state for when the player has control
	cutScene,
    lookingAround,
	paused, // no input can be issued
	other
}

// Used to determine the direction the player is walking when approaching a new waypoint
public enum PlayerWalkState {
    standing,
    left,
    right,
    forward
}
