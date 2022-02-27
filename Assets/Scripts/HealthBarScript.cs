using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        SetMaxHealth();
    }

    void Update()
    {
        SetCurrentHealth();
    }

    public void SetMaxHealth()
    {
        slider.maxValue = gamestate.Instance.getCurrentHP();
        slider.value = gamestate.Instance.getCurrentHP();
    }

    public void SetCurrentHealth()
    {
        slider.value = gamestate.Instance.getCurrentHP();
    }
}
