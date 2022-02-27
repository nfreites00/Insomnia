using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
    public GameObject instructionsUI;

    public void okButton()
    {
        instructionsUI.SetActive(false);
        gamestate.Instance.setRunning();
    }
}
