using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
   [SerializeField] Slider volumeBotao;

   void Start(){
        volumeBotao.value = PlayerPrefs.GetFloat("musicVolume", 1);
   }

   public void MudarVolume(){
        AudioListener.volume = volumeBotao.value;
        Save();
   }

   private void Save(){
        PlayerPrefs.SetFloat("musicVolume" , volumeBotao.value);
   }
}