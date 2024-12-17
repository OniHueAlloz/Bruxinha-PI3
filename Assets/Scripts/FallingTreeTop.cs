using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTreeTop : MonoBehaviour
{
    [SerializeField] private Transform ground;
    private AudioSource audioSource;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameO)
    }
}
