using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandleTimerScript : MonoBehaviour
{
    public PlayerController2D player;
    public Slider candleSlider;
    public Animator musicAnim;
    public AudioSource gameMusic1;
    public AudioSource gameMusic2;
    public static bool timerStop;
    private float timeLeft = 540.0f; // set ur desired game time
    private float musicSwitch = 270.0f;

    // Start is called before the first frame update
    void Start()
    {
        timerStop = false;
        candleSlider.maxValue = timeLeft;
        candleSlider.value = timeLeft;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        // Dash penalty
        if (player.DidPlayerDash())
        {
            timeLeft -= player.dashPenalty;
        }

        if (timeLeft <= 0)
            timerStop = true;

        candleSlider.value = timeLeft;
    }
}
