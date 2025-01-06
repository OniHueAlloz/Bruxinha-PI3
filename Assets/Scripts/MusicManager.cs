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
    private static MusicManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;

        previousSceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        Debug.Log("Number of MusicManager instances: " + FindObjectsOfType<MusicManager>().Length);
        MusicPlay(SceneManager.GetActiveScene().name);
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
