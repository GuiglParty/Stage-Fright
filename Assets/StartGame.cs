using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class StartGame : MonoBehaviour
{
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

        List<Note> newNotes = new List<Note>();

        // sting notes
        for (int i = bottomOctaveStart; i <= topOctaveEnd; i++)
        {
            if (MidiMaster.GetKey(i) > 0)
            {
                newNotes.Add(new Note
                {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }

        // process notes
        print("notes count: " + newNotes.Count);
        if (newNotes.Count > 3) {
            Application.LoadLevel("CrappyTown");
        }
    }
}
