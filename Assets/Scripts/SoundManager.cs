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
    public AudioClip lose;

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
        backgroundMusic.resource = Resources.Load<AudioClip>("Sounds/BGSound");
        backgroundMusic.Play();
    }
    public void SoundPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);   
    }

    public void SourceSoundPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);   
        backgroundMusic.Stop();
        
    }
    
}
