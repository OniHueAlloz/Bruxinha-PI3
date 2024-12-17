using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip menu;
    public AudioClip creditos;
    public AudioClip gameplay;
    private string currentSceneName;
    private string previousSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menu;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.Play();

        previousSceneName = SceneManager.GetActiveScene().name;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != previousSceneName)
        {
            previousSceneName = currentSceneName;
            MusicPlay(currentSceneName);
        }
    }

    void MusicPlay (string sceneName)
    {
        if(sceneName == "Caio")
        {
            if (audioSource.clip != gameplay)
            {
                audioSource.clip = gameplay;
                audioSource.Play();
            }
        }
        else if(sceneName == "Creditos")
        {
            if (audioSource.clip != creditos)
            {
                audioSource.clip = creditos;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != menu)
            {
                audioSource.clip = menu;
                audioSource.Play();
            }
        }
    }
}
