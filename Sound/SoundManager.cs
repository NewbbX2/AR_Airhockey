using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public AudioClip HitSound;
    public AudioClip WinSound;
    public AudioClip StrikeSound;

    private AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio = FindObjectOfType<AudioSource>();
    }

    public void PlayHitSound()
    {
        Audio.PlayOneShot(HitSound);        
    }

    public void PlayWinSound()
    {
        Audio.PlayOneShot(WinSound);
    }

    public void PlayStrikeSound()
    {
        Audio.PlayOneShot(StrikeSound);
        Handheld.Vibrate();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Goal"))
        {
            //Debug.Log("Goal");
            PlayWinSound();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Corner") || collision.collider.tag.Equals("LR_Wall"))
        {
            //Debug.Log("Hit from col");
            PlayHitSound();
        }
        if (collision.collider.tag.Equals("Striker") || collision.collider.tag.Equals("HockeyStriker"))
        {
            //Debug.Log("Hit Striker from col");
            PlayStrikeSound();
        }
    }
}
