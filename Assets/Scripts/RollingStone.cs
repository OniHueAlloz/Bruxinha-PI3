using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class RollingStone : MonoBehaviour
{
    public AudioClip rollSound;
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody rb;

    [SerializeField] private float velocityThreshold = 0.1f;
    private bool isMoving = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
        audioSource.minDistance = 495f;
        audioSource.maxDistance = 500f;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = MovementCheck();
        //animator.SetBool("isMoving", isMoving);

        if (isMoving && !audioSource.isPlaying)
        {
            PlayRollSound();
        }
    }

    private bool MovementCheck()
    {
        float currentVelocity = rb.velocity.magnitude;

        Debug.Log("Current Velocity: " + currentVelocity);

        return currentVelocity > velocityThreshold;
    }

    public void PlayRollSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(rollSound);
            Debug.Log("Roll sound played");
        }
    }
}
