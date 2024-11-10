using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class RollingStone : MonoBehaviour
{
    public AudioClip rollSound;
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
    /*void Update()
    {
        
    } */

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(rollSound);
        Debug.Log("Roll sound played"); 
    }
}
