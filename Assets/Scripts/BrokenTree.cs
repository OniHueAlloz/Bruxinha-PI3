using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class BrokenTree : MonoBehaviour
{
    public AudioClip breakSound;
    public AudioClip fallSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
        audioSource.minDistance = 495f;
        audioSource.maxDistance = 500f;
    }

    // Update is called once per frame
    /* void Update()
    {
        
    }*/

    public void PlayFallSound()
    {
        audioSource.PlayOneShot(fallSound);
        Debug.Log("fall sound played"); 
    }

    public void PlayBreakSound()
    {
        audioSource.PlayOneShot(breakSound);
        Debug.Log("break sound played"); 
    }
}
