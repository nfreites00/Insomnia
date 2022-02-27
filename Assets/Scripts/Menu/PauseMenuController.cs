using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenu;
    public GameObject videoMenu;
    public GameObject audioMenu;
    public GameObject quitMenuPrompt;
    public Slider musicSlider, sfxSlider;

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

        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamestate.Instance.isPaused() && pauseMenuUI.activeSelf)
            {
                Resume();
            }

            else if(gamestate.Instance.isRunning())
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gamestate.Instance.setRunning();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        gamestate.Instance.setPaused();
    }

    public void options()
    {
        pauseMenuUI.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void videoSettings()
    {
        optionsMenu.SetActive(false);
        videoMenu.SetActive(true);
    }

    public void audioSettings()
    {
        optionsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void optionsBackButton()
    {
        videoMenu.SetActive(false);
        audioMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void pauseBackButton()
    {
        pauseMenuUI.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void quitToMenu()
    {
        pauseMenuUI.SetActive(false);
        quitMenuPrompt.SetActive(true);
    }

    public void yesButton()
    {
        gamestate.Instance.returnToMenu();
    }

    public void noButton()
    {
        pauseMenuUI.SetActive(true);
        quitMenuPrompt.SetActive(false);
    }
}
