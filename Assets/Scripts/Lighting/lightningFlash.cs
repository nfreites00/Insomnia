using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Audio;

public class lightningFlash : MonoBehaviour
{
    private Coroutine lightningCoroutine;
    public AudioSource thunder;

    // Start is called before the first frame update
    void Start()
    {
        lightningCoroutine = StartCoroutine(LightningFlashDelay(gamestate.Instance.getRandomLightningDelay()));
    }

    // Update is called once per frame
    void Update()
    {
        if (lightningCoroutine == null)
        {
            lightningCoroutine = StartCoroutine(LightningFlashDelay(gamestate.Instance.getRandomLightningDelay()));
        }
    }

    IEnumerator LightningFlashDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Light2D light = GetComponent<Light2D>();
        light.intensity = .5f;
        thunder.Play();
        yield return new WaitForSeconds(.55f);
        light.intensity = 0;


        lightningCoroutine = null;
    }
}
