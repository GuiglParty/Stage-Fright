using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class InputParser : MonoBehaviour
{
    public GameObject playerObject;
    private Player playerScript;

    private int previousStepRoot;
    private Note newStepRoot;
    private List<int> chordToComplete;

    float previousStepTime;

    int midiMinKey = 0;
    int midiMaxKey = 127;

    int bottomOctaveStart = 36;
    int bottomOctaveEnd = 48;

    int topOctaveStart = 100; // TODO: Determine actual value
    int topOctaveEnd = 112; // TODO: Determine actual value

    // Use this for initialization
    void Start()
    {
        playerScript = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Note> newWalkingNotes = new List<Note>();

        // walking notes
        for (int i = bottomOctaveEnd; i < topOctaveStart; i++)
        {
            if (MidiMaster.GetKeyDown(i))
            {
                newWalkingNotes.Add(new Note
                {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }
        
        if (newWalkingNotes.Count == 1)
        {
            newStepRoot = newWalkingNotes[0];
            chordToComplete = new List<int>
            {
                // construct diminished triad
                newWalkingNotes[0].Value + 3,
                newWalkingNotes[0].Value + 6
            };
        }
        else if (newWalkingNotes.Count == 2)
        {
            if (chordToComplete.Find(note => note == newWalkingNotes[0].Value) != 0 &&
                chordToComplete.Find(note => note == newWalkingNotes[1].Value) != 0)
            {
                // they've completed the chord, take a step

                float avgVelocity = (newStepRoot.Velocity + newWalkingNotes[0].Velocity + newWalkingNotes[1].Velocity) / 3;
                float timeDelta = Time.time - previousStepTime;

                int rootDifference = newStepRoot.Value - previousStepRoot;

                if (rootDifference < 0)
                {
                    playerScript.TurnLeft(); // TODO(joseph)
                }
                else if (rootDifference > 0)
                {
                    playerScript.TurnRight(); // TODO(joseph)
                }
                else
                {
                    playerScript.MoveForward(timeDelta, avgVelocity); // TODO(joseph)
                }

                previousStepTime = Time.time;
                previousStepRoot = newStepRoot.Value;
                chordToComplete.Clear();
            }
        }
    }
}
