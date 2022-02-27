using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class followTarget : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = transform.position = target.position;
        }

    }
}
