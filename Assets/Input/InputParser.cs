using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;

public class InputParser : MonoBehaviour
{
    public GameObject playerObject;
    public Player playerScript;

    int MidiMinKey = 0;
    int MidiMaxKey = 127;

    // Use this for initialization
    void Start()
    {
        playerScript = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Note> notes = new List<Note>();

        for (int i = MidiMinKey; i <= MidiMaxKey; i++)
        {
            if (MidiMaster.GetKeyDown(i))
            {
                notes.Add(new Note {
                    Value = i,
                    Velocity = MidiMaster.GetKey(i)
                });
            }
        }
    }
}
