using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SVRA_TriggerAudio : MonoBehaviour
{
    [System.Serializable]
    public class DelayedStart
    {
        [Tooltip("Enable delayed start")]
        public bool enable = false;

        [Tooltip("The time in seconds to delay the start of the audio by")]
        public float seconds = 0.0f;
    }

    [Tooltip("Allow the audio to be paused")]
    public bool pauseEnabled = true;

    [Tooltip("Setup the audio to start with a delayed start")]
    public DelayedStart delayedStart;

    AudioSource audio;

    private bool audioPlaying = false;
    private bool audioPaused = false;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void TriggerAudio()
    {        
        if (delayedStart.enable)
        {
            DelayedAudioStart();            
        }
        else
        {
            NonDelayedAudioStart();
        }
    }

    /* Play the audio with the specified delayed starting time */
    void DelayedAudioStart()
    {
        if (audioPlaying && pauseEnabled)
        {
            audio.Pause();
            audioPlaying = false;
        }
        else if (audioPaused && pauseEnabled)
        {
            audio.UnPause();
            audioPlaying = true;
            audioPaused = false;
        }
        else
        {
            audio.PlayDelayed(delayedStart.seconds);
            audioPlaying = true;
        }
    }

    void NonDelayedAudioStart()
    {
        if (audioPlaying && pauseEnabled)
        {
            audio.Pause();
            audioPlaying = false;
        }
        else if (audioPaused && pauseEnabled)
        {
            audio.UnPause();
            audioPlaying = true;
            audioPaused = false;
        }
        else
        {
            audio.Play();
            audioPlaying = true;
        }
    }

    public void StopAudio()
    {
        if (audioPlaying)
        {
            audio.Stop();
        }
    }
}