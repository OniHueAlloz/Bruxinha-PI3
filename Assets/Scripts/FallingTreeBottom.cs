using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTreeBottom : MonoBehaviour
{
    public FallingTreeTop fallingTree;
    public Rigidbody treeRb;
    private AudioSource audioSource;
    public AudioClip breakSound;

    // Start is called before the first frame update
    void Start()
    {
        fallingTree = FindObjectOfType<FallingTreeTop>();
        if (fallingTree != null)
        {
            treeRb = fallingTree.rb;
        }
        else
        {
            Debug.LogError("Associa o FallingTreeTop no treeRb do FallingTreeBottom no Inspector");
        }

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

    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Thrown"))
        {
            audioSource.PlayOneShot(breakSound);
            treeRb.constraints = RigidbodyConstraints.None;
        }
    }
}
