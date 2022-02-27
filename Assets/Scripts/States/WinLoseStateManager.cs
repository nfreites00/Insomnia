using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseStateManager : MonoBehaviour
{
    bool gameOver = false;
    public GameObject winStateUI, loseStateUI, quitMenuPrompt;

    public void completeLevel()
    {
        Time.timeScale = 0f;
        winStateUI.SetActive(true);
    }
}
