using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Huds : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ConfigSom(string cena){
        SceneManager.LoadScene(cena);
    }
    public void LoadScenes(string cena) {
        SceneManager.LoadScene(cena);
    }

    public void Quitexe(){
        Application.Quit();
    }
}
