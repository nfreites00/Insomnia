using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class candleLogic : MonoBehaviour
{
    private Coroutine candleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        candleCoroutine = StartCoroutine(ExtinguishCandleDelay(gamestate.Instance.getCandleDuration()));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R)) {
            RelightCandle();
        }
    }

    void RelightCandle()
    {
        if (candleCoroutine != null)
        {
            print("Stopping existing timer");
            StopCoroutine(candleCoroutine);
        }
        GetComponent<Light2D>().intensity = 1;
        candleCoroutine = StartCoroutine(ExtinguishCandleDelay(gamestate.Instance.getCandleDuration()));
        print("Timer reset to " + gamestate.Instance.getCandleDuration());
        gamestate.Instance.setCandleStatus("lit");
    }
    IEnumerator ExtinguishCandleDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Light2D light = GetComponent<Light2D>();
        light.intensity = 0;
        gamestate.Instance.setCandleStatus("out");
        candleCoroutine = null;
    }
}
