using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MagicScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float floatDistance = 0.3f;
    [SerializeField] private float floatSpeed = 2f;

    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;
    private Renderer renderer; 

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
        audioSource.minDistance = 495f;
        audioSource.maxDistance = 500f;

        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        float newHeight = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatDistance;
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            renderer.enabled = false;
            StartCoroutine(WaitForSound(sound));
        }
    }

    private IEnumerator WaitForSound(AudioClip clip)
    {
        audioSource.clip = clip;
        yield return new WaitForSeconds(clip.length);
        gameObject.SetActive(false);
    }
}
