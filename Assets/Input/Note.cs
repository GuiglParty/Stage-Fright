using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An occurrence of a key being pressed.
/// </summary>
public class Note
{
    /// <summary>
    /// midi value of note, 0-127.
    /// </summary>
    public int Value;

    /// <summary>
    /// force of note press, 0-1.
    /// </summary>
    public float Velocity;
}

