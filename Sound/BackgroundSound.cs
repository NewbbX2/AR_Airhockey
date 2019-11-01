using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioClip PlayMusic;
    public AudioSource Audio;
            
    public void PlayPlayMusic()
    {
        Audio.Stop();
        Audio.loop = true;
        Audio.clip = PlayMusic;
        Audio.Play();
    }
}
