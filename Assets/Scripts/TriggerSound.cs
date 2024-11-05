using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
    }

    // Update is called once per frame
    /* void Update()
    {
        
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sound != null)
            {
                audioSource.PlayOneShot(sound);
            }
            else
            {
                Debug.Log("Sound played"); 
            }
        }
    }
}
