using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public  static SoundManager instance;
   
        
    [SerializeField] private AudioSource audioObject;

    AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
          instance = this; 
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayRandomSoundFX(float volume,AudioClip[] clip,Transform spawntransform )
    {
        int rand = Random.Range(0,clip.Length);
        AudioSource audioSource = Instantiate(audioObject, spawntransform.position, Quaternion.identity);
        audioSource.clip = clip[rand];
        audioSource.volume = volume;    
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFX(float volume, AudioClip clip, Transform spawntransform)
    {
      
        AudioSource audioSource = Instantiate(audioObject, spawntransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

}
