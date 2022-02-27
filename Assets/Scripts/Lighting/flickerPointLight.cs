using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class flickerPointLight : MonoBehaviour
{
    private Light2D targetLight;
    private Animator animator;

    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] float time;
    [SerializeField] bool isPlayerCandle;
    [SerializeField] bool isWallLantern;

    // Start is called before the first frame update
    void Start()
    {
        targetLight = GetComponent<Light2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isLit())
        {
            // Animation
            if (isWallLantern)
            {
                animator.SetTrigger("lightLantern");
                animator.SetBool("isIdle", true);
            }
            // Lighting
            targetLight.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, Mathf.PingPong(Time.time, time));
            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time, time));
        }
    }

    private bool isLit()
    {
        if (isPlayerCandle && gamestate.Instance.getCandleStatus() == "lit")
        {
            return true;
        }
        else if (isWallLantern && GetComponent<wallLantern>().lit)
        {
            return true;
        }

        return false; 
    }
}
