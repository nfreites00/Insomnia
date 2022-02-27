using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private float musicVol, sfxVol;
    public AudioSource[] musicAudios;
    public AudioSource[] sfxAudios;
    public Slider musicSlider, sfxSlider;

    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        musicVol = PlayerPrefs.GetFloat("music");
        sfxVol = PlayerPrefs.GetFloat("sfx");


        for (int i = 0; i < musicAudios.Length; i++)
        {
            musicAudios[i].volume = musicVol;
        }

        for (int i = 0; i < sfxAudios.Length; i++)
        {
            sfxAudios[i].volume = sfxVol;
        }

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;
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
