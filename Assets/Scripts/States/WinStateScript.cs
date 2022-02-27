using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinStateScript : MonoBehaviour
{
    public GameObject winstateENUI, winstateHUI, quitMenuENPrompt, quitMenuHPrompt;

    public void Start()
    {
        winstateENUI.SetActive(false);
        winstateHUI.SetActive(false);
        quitMenuENPrompt.SetActive(false);
        quitMenuHPrompt.SetActive(false);
    }

    // Update
    public void Update()
    {
        if (gamestate.Instance.allCandlesLit() == true && gamestate.Instance.isRunning())
        {
            gamestate.Instance.setPaused();
            completeLevel();
        }
    }

    public void completeLevel()
    {
        gamestate.Instance.setPaused();
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Easy" || scene.name == "Normal")
        {
            winstateENUI.SetActive(true);
        }
        else if (scene.name == "Hard")
        {
            winstateHUI.SetActive(true);
        }
    }

    public void nextLevelButton()
    {
        winstateENUI.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Easy")
        {
            winstateENUI.SetActive(false);
            gamestate.Instance.startNormalState();
            Time.timeScale = 1f;
        }

        if (scene.name == "Normal")
        {
            winstateENUI.SetActive(false);
            gamestate.Instance.startHardState();
            Time.timeScale = 1f;
        }
    }

    public void quitToMenuEN()
    {
        winstateENUI.SetActive(false);
        quitMenuENPrompt.SetActive(true);
    }
    public void noButtonEN()
    {
        winstateENUI.SetActive(true);
        quitMenuENPrompt.SetActive(false);
    }

    public void quitToMenuH()
    {
        winstateHUI.SetActive(false);
        quitMenuHPrompt.SetActive(true);
    }

    public void noButtonH()
    {
        winstateHUI.SetActive(true);
        quitMenuHPrompt.SetActive(false);
    }

    public void yesButton()
    {
        gamestate.Instance.returnToMenu();
    }


}
