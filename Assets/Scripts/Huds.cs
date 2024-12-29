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

    public Text coinUI;
    public Text lifeUi;
    public GameObject energy1;
    public GameObject energy2;
    public GameObject energy3;
    public GameObject energy4;
    public GameObject energy5;
    public GameObject cat;
    public GameObject hide;
    public GameObject pause;

    private int i = 0;

    void Awake()
    {
        //Time.timeScale = 0f;
        Dialogo1.gameObject.SetActive(true);
        Dialogo2.gameObject.SetActive(false);
        Dialogo3.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        pedido.gameObject.SetActive(false);
    }

    void Start()
    {
        cat.gameObject.SetActive(false);
        hide.gameObject.SetActive(true);
    }

    void Update()
    {
        if (PlayerMovement.gotObjective)
        {
            pedido.gameObject.SetActive(true);
            //Time.timeScale = 0f;
        }

        UpdateTexts();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (i == 0)
            {
                Dialogo1.gameObject.SetActive(true);
                Dialogo2.gameObject.SetActive(true);
                Dialogo3.gameObject.SetActive(false);
                i++;
            }
            else if (i == 1)
            {
                Dialogo1.gameObject.SetActive(true);
                Dialogo2.gameObject.SetActive(true);
                Dialogo3.gameObject.SetActive(true);
                i++;
            }
            else
            {
                Dialogo1.gameObject.SetActive(false);
                Dialogo2.gameObject.SetActive(false);
                Dialogo3.gameObject.SetActive(false);
                //Time.timeScale = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.gameObject.SetActive(true);
        }

        int varEnergy = PlayerMovement.energy;
        switch (varEnergy)
        {
            case 0:
                energy1.gameObject.SetActive(false);
                energy2.gameObject.SetActive(false);
                energy3.gameObject.SetActive(false);
                energy4.gameObject.SetActive(false);
                energy5.gameObject.SetActive(false);
                Debug.Log("0 energia");
                break;
            case 1:
                energy1.gameObject.SetActive(true);
                energy2.gameObject.SetActive(false);
                energy3.gameObject.SetActive(false);
                energy4.gameObject.SetActive(false);
                energy5.gameObject.SetActive(false);
                Debug.Log("1 energia");
                break;
            case 2:
                energy1.gameObject.SetActive(true);
                energy2.gameObject.SetActive(true);
                energy3.gameObject.SetActive(false);
                energy4.gameObject.SetActive(false);
                energy5.gameObject.SetActive(false);
                Debug.Log("2 energia");
                break;
            case 3:
                energy1.gameObject.SetActive(true);
                energy2.gameObject.SetActive(true);
                energy3.gameObject.SetActive(true);
                energy4.gameObject.SetActive(false);
                energy5.gameObject.SetActive(false);
                Debug.Log("3 energia");
                break;
            case 4:
                energy1.gameObject.SetActive(true);
                energy2.gameObject.SetActive(true);
                energy3.gameObject.SetActive(true);
                energy4.gameObject.SetActive(true);
                energy5.gameObject.SetActive(false);
                Debug.Log("4 energia");
                break;
            case 5:
                energy1.gameObject.SetActive(true);
                energy2.gameObject.SetActive(true);
                energy3.gameObject.SetActive(true);
                energy4.gameObject.SetActive(true);
                energy5.gameObject.SetActive(true);
                Debug.Log("5 energia");
                break;
        }
    }

    public void EscapeButton()
    {
        pause.gameObject.SetActive(false);
    }
    

    public void UpdateTexts()
    {
        coinText.text = PlayerMovement.coinCount.ToString();

        string coin = PlayerMovement.coinCount.ToString();
        coinUI.text = (coin + "x");
        Debug.Log(coin + "x");
        string life = PlayerMovement.life.ToString();
        lifeUi.text = ("x" + life);
        Debug.Log("x" + life);

        bool objectiveMaybe = PlayerMovement.gotObjective;
        bool plushMaybe = PlayerMovement.gotPlush;
        if (objectiveMaybe)
        {
            objectiveText.text = "Coletado";
        }

        if (plushMaybe)
        {
            plushText.text = "Coletado";
            cat.gameObject.SetActive(true);
            hide.gameObject.SetActive(false);
        }
        else plushText.text = "Perdido";
    }

    public void ConfigSom(string cena){
        SceneManager.LoadScene(cena);
    }
    public void LoadScenes(string cena) {
        SceneManager.LoadScene(cena);
    }
    public void EndGame()
    {
        //associar aos botões de sair dos creditos e do pause
        PlayerMovement.gotObjective = false;
        PlayerMovement.gotPlush = false;
        PlayerMovement.coinCount = 0;
        PlayerMovement.life = 3; 
        PlayerMovement.energy = 5;

        i = 0;
        
        Dialogo1.SetActive(false);
        Dialogo2.SetActive(false);
        Dialogo3.SetActive(false);
        pedido.SetActive(false);
        pause.SetActive(false);

        SceneManager.LoadScene("Menu");
    }

    public void Quitexe(){
        Application.Quit();
    }
}
