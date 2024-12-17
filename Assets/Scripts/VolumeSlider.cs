using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
     [SerializeField] AudioMixer musicMixer;  
     [SerializeField] Slider volumeBotao;
     [SerializeField] Slider volumeEfeito;

     void Start(){
          if(PlayerPrefs.HasKey("musicVolume"))
          {
               LoadVolume();
          }
          else
          {
               MudarVolume();
               MudarEfeitos();
          } 
     }

     public void MudarVolume(){
          float volume = volumeBotao.value;
          musicMixer.SetFloat("musicVolume", Mathf.Log10(volume)*20);
          PlayerPrefs.SetFloat("musicVolume" , volume);
     }

     public void MudarEfeitos(){
          float volume = volumeEfeito.value;
          musicMixer.SetFloat("efeitosVolume", Mathf.Log10(volume)*20);
          PlayerPrefs.SetFloat("efeitosVolume" , volume);
     }

     private void LoadVolume(){
          volumeBotao.value = PlayerPrefs.GetFloat("musicVolume");
          volumeEfeito.value = PlayerPrefs.GetFloat("efeitosVolume");
          MudarVolume();
          MudarEfeitos();
     }
}