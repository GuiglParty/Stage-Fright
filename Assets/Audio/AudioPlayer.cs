using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void PlayNote(int note, float volume)
    {
        if (note >= 0 && note < 128)
        {
            print("playing " + note);
            audioSource.pitch = Mathf.Pow(2, (float)((note) / 12.0));
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public void StopNote(int note)
    {
        audioSource.Stop();
    }
}
