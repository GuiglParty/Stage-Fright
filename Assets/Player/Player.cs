using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public float walkSpeed = 5f; // Units moved per second, when running at full speed
	public float turnSpeed = 0.5f; // How quickly the view will spin when walking normally
	public GameObject nextWaypoint; // the next position that the player will move towards when walking

    public Camera firstPerson; // Used to navigate game
    public Camera birdsEye; // Used when viewing map

    GameObject _lastWaypoint; // the previous position that the player moved towards
	GameObject _intermediateWaypoint; // used to make movement more smooth and consistant
	PlayerState _ps;
	PlayerWalkState _pws;
	
	float _stepForce;
	
	// initialization
	void Start () {
	    _ps = PlayerState.walking;
		_pws = PlayerWalkState.forward;
		_stepForce = 1.0f;
		
		_lastWaypoint = nextWaypoint;
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

        
	
		// Check what type of action the player object is currently undertaking
		if( _ps == PlayerState.walking && nextWaypoint != null) {
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

    }

    // 180 turn
    public void turnAround()
    {
		GameObject temp = _lastWaypoint;
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
		_lastWaypoint = nextWaypoint;
		_intermediateWaypoint = nextWaypoint;
		nextWaypoint = Waypoint;
	}
}

// Used for determining the type of action the player object is taking
public enum PlayerState {
	walking, // Default state for when the player has control
	cutScene,
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