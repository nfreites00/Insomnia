using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private int firstPlayInt;
    public Slider musicSlider, sfxSlider;
    private float musicVol, sfxVol;
    public AudioSource[] musicAudios;
    public AudioSource[] sfxAudios;


    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            musicVol = 0.3f;
            sfxVol = 0.3f;
            musicSlider.value = musicVol;
            sfxSlider.value = sfxVol;
            PlayerPrefs.SetFloat("music", musicVol);
            PlayerPrefs.SetFloat("sfx", sfxVol);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            musicVol = PlayerPrefs.GetFloat("music");
            musicSlider.value = musicVol;
            sfxVol = PlayerPrefs.GetFloat("sfx");
            sfxSlider.value = sfxVol;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.SetFloat("sfx", sfxSlider.value);
    }

    public void UpdateVolume()
    {
        for (int i = 0; i < musicAudios.Length; i++)
        {
            musicAudios[i].volume = musicSlider.value;
        }

        for (int i = 0; i < sfxAudios.Length; i++)
        {
            sfxAudios[i].volume = sfxSlider.value;
        }
    }
}
