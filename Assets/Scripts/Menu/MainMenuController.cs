using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject difficulties;
    public GameObject audioMenu;
    public GameObject videoMenu;
    public Slider musicSlider, sfxSlider;

    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        //musicSlider.value = PlayerPrefs.GetFloat("music", 0.75f);
        //sfxSlider.value = PlayerPrefs.GetFloat("sfx", 0.75f);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> choices = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string choice = resolutions[i].width + "x" + resolutions[i].height;
            choices.Add(choice);

            if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }


        resolutionDropdown.AddOptions(choices);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void startButton()
    {
        mainMenu.SetActive(false);
        difficulties.SetActive(true);
    }

    public void optionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void videoButton()
    {
        videoMenu.SetActive(true);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void audioButton()
    {
        audioMenu.SetActive(true);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void MenuBackButton()
    {
        mainMenu.SetActive(true);
        difficulties.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void optionsBackButton()
    {
        videoMenu.SetActive(false);
        audioMenu.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
