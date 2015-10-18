using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class InputParser : MonoBehaviour
{
    public GameObject playerObject;
    private Player playerScript;
    public GameObject audioObject;
    private UnitySynthTest audioScript;

    private int _previousStepRoot;
    private Note _newStepRoot;
    private List<int> _chordToComplete;

    float _previousStepTime;

    private bool _lastFrameWasRoot = false;
    private bool _lastInputWasChordComplete = false;
    private bool _justTurnedAround = false;

    // midi min = 0
    // midi max = 127

    int bottomOctaveStart = 36;
    int bottomOctaveEnd = 48;

    int topOctaveStart = 84; // TODO: Determine actual value
    int topOctaveEnd = 96; // TODO: Determine actual value

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start()
    {
        playerScript = playerObject.GetComponent<Player>();
        audioScript = audioObject.GetComponent<UnitySynthTest>();
        _chordToComplete = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        // play sounds
        for (int i = 0; i < 128; i++)
        {
            if (MidiMaster.GetKeyDown(i))
            {
                audioScript.PlayNote(i, MidiMaster.GetKey(i));
            }
            else if (MidiMaster.GetKeyUp(i))
            {
                audioScript.StopNote(i);
            }
        }

        List<Note> newStingNotes = new List<Note>();

        // sting notes
        for (int i = topOctaveStart; i <= topOctaveEnd; i++)
        {
            if (MidiMaster.GetKey(i) > 0)
            {
                newStingNotes.Add(new Note
                {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }

        List<Note> newLookNotes = new List<Note>();

        // look notes
        for (int i = bottomOctaveStart; i < bottomOctaveEnd; i++)
        {
            if (MidiMaster.GetKey(i) > 0) // should repeatedly trigger on hold
            {
                print("bottom note " + i + " entered");
                newLookNotes.Add(new Note
                {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }

        List<Note> newWalkingNotes = new List<Note>();

        // walking notes
        for (int i = bottomOctaveEnd; i < topOctaveStart; i++)
        {
            if (MidiMaster.GetKey(i) > 0)
            {
                newWalkingNotes.Add(new Note
                {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }

        // process notes
        print("sting notes count: " + newStingNotes.Count);
        print("look notes count: " + newLookNotes.Count);
        // 180 sting takes priority over looking
        if (newStingNotes.Count > 3)
        {
            if (!_justTurnedAround)
            {
                playerScript.turnAround();
                _justTurnedAround = true;
            }
        }
        else
        {
            _justTurnedAround = false;
        }

        if (newLookNotes.Count > 3)
        {
            print("look around notes pressed");
            playerScript.lookAround();
        }
        else
        {
            if (playerScript.getPlayerState() == PlayerState.lookingAround)
            {
                playerScript.stopLookingAround();
            }
        }

        // can keep running while looking/turning around
        if (newWalkingNotes.Count == 1)
        {
            if (_lastFrameWasRoot)
            {
                // don't reset root just yet, wait a frame to see if they press two 
                _lastFrameWasRoot = false;
            }
            else
            {
                _newStepRoot = newWalkingNotes[0];
                _chordToComplete = new List<int>
                {
                    // construct diminished triad
                    newWalkingNotes[0].Value + 3,
                    newWalkingNotes[0].Value + 6
                };
                _lastFrameWasRoot = true;
                _lastInputWasChordComplete = false;
            }
        }
        else if (newWalkingNotes.Count == 2)
        {
            if (!_lastInputWasChordComplete)
            {
                if (_chordToComplete.Find(note => note == newWalkingNotes[0].Value) != 0 &&
                    _chordToComplete.Find(note => note == newWalkingNotes[1].Value) != 0)
                {
                    // they've completed the chord, take a step

                    float avgVelocity = (_newStepRoot.Velocity + newWalkingNotes[0].Velocity + newWalkingNotes[1].Velocity) / 3;
                    float timeDelta = Time.time - _previousStepTime;

                    int rootDifference = _newStepRoot.Value - _previousStepRoot;

                    if (rootDifference < 0)
                    {
                        //print("turn left");
                        playerScript.turnLeft();
                    }
                    else if (rootDifference > 0)
                    {
                        //print("turn right");
                        playerScript.turnRight();
                    }
                    else
                    {
                        //print("move forward");
                        playerScript.moveForward(timeDelta, avgVelocity);
                    }

                    _previousStepTime = Time.time;
                    _previousStepRoot = _newStepRoot.Value;
                    _chordToComplete.Clear();
                    _lastInputWasChordComplete = true;
                }
            }
        }
    }
}
