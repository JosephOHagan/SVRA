using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SVRA_TriggerAudio : MonoBehaviour
{
    AudioSource audio;
    public bool pauseToggle = true;

    private bool audioPlaying = false;
    private bool audioPaused = false;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void TriggerAudio()
    {
        if (audioPlaying && pauseToggle)
        {
            Debug.Log("Audio Paused");

            audio.Pause();
            audioPlaying = false;
        }
        else if (audioPaused && pauseToggle)
        {
            Debug.Log("Audio Unpaused");

            audio.UnPause();
            audioPlaying = true;
            audioPaused = false;
        }    
        else        
        {
            Debug.Log("Audio Playing");

            audio.Play();
            audioPlaying = true;
        }
    }
}