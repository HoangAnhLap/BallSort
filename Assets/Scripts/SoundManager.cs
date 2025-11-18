using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource backgroundMusic;
    public AudioSource SFX;
    
    public AudioClip ballPut;
    public AudioClip ballSelected;
    public AudioClip merge;
    public AudioClip completeStack;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        backgroundMusic.Play();
    }
    public void SoundPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);   
    }

    
}
