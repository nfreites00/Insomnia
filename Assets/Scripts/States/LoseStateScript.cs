using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseStateScript : MonoBehaviour
{
    public GameObject losestateHealthUI, losestateCandleUI, quitMenuHealthPrompt, quitMenuCandlePrompt;
    public AudioSource playerScream;

    public void Start()
    {
        losestateHealthUI.SetActive(false);
        losestateCandleUI.SetActive(false);
        quitMenuHealthPrompt.SetActive(false);
        quitMenuCandlePrompt.SetActive(false);
    }

    public void Update()
    {
        if ((gamestate.Instance.isCharacterAlive() == false) && gamestate.Instance.isRunning())
        {
            playerScream.Play();
            gamestate.Instance.setPaused();
            losestateHealthUI.SetActive(true);

        }
        else if ((gamestate.Instance.candleTimerRanOut() == true) && gamestate.Instance.isRunning())
        {
            playerScream.Play();
            gamestate.Instance.setPaused();
            losestateCandleUI.SetActive(true);
        }
    }


    public void restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        losestateHealthUI.SetActive(false);
        losestateCandleUI.SetActive(false);
        quitMenuHealthPrompt.SetActive(false);
        quitMenuCandlePrompt.SetActive(false);
        Time.timeScale = 1f;

        if (scene.name == "Easy")
        {
            gamestate.Instance.startEasyState();
        }

        if (scene.name == "Normal")
        {
            gamestate.Instance.startNormalState();
        }

        if (scene.name == "Hard")
        {
            gamestate.Instance.startHardState();
        }

    }

    public void quitToMenuHealth()
    {
        losestateHealthUI.SetActive(false);
        losestateCandleUI.SetActive(false);
        quitMenuHealthPrompt.SetActive(true);
        quitMenuCandlePrompt.SetActive(false);
    }
    public void noButtonHealth()
    {
        losestateHealthUI.SetActive(true);
        losestateCandleUI.SetActive(false);
        quitMenuHealthPrompt.SetActive(false);
        quitMenuCandlePrompt.SetActive(false);
    }
    public void quitToMenuCandle()
    {
        losestateCandleUI.SetActive(false);
        losestateHealthUI.SetActive(false);
        quitMenuCandlePrompt.SetActive(true);
        quitMenuHealthPrompt.SetActive(false);
    }
    public void noButtonCandle()
    {
        losestateCandleUI.SetActive(true);
        losestateHealthUI.SetActive(false);
        quitMenuCandlePrompt.SetActive(false);
        quitMenuHealthPrompt.SetActive(false);
    }

    public void yesButton()
    {
        gamestate.Instance.returnToMenu();
    }

}
