using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Huds : MonoBehaviour
{
    //public PlayerMovement playerScript = null;
    public GameObject pedido = null;

    public Text coinText = null;
    public Text objectiveText = null;
    public Text plushText = null;

    public GameObject Dialogo1;
    public GameObject Dialogo2;
    public GameObject Dialogo3;



    void Awake()
    {
        Time.timeScale = 0f;
        Dialogo1.SetActive(true);
        Dialogo2.SetActive(false);
        Dialogo3.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (PlayerMovement.gotObjective)
        {
            pedido.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        UpdateTexts();
    }

    public void UpdateTexts()
    {
        coinText.text = PlayerMovement.coinCount.ToString();

        bool objectiveMaybe = PlayerMovement.gotObjective;
        bool plushMaybe = PlayerMovement.gotPlush;
        if (objectiveMaybe)
        {
            objectiveText.text = "Coletado";
        }

        if (plushMaybe)
        {
            plushText.text = "Coletado";
            //ativar elemento UI gatinho
        }
        else plushText.text = "Perdido";
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
