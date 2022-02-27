using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanternCounterScript : MonoBehaviour
{
    public Text lanternCounterUI;
    private int lanternsRemaining;

    void Start()
    {
        lanternsRemaining = gamestate.Instance.getLanternsRemaining();
    }

    void Update()
    {
        lanternsRemaining = gamestate.Instance.getLanternsRemaining();
        lanternCounterUI.text = lanternsRemaining.ToString();
    }
}
