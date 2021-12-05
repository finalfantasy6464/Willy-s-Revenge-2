using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightExpansion : MonoBehaviour
{
    public float minSize;
    public float maxSize;
    public float changeSpeed = 1.0f;
    public float initialStartSize;

    public bool descending;

    Light2D lightsource;

    private void Start()
    {
        lightsource = GetComponent<Light2D>();
        if(initialStartSize != 0)
        {
            lightsource.pointLightOuterRadius = initialStartSize;
        }
    }

    private void Update()
    {
        if(lightsource.pointLightOuterRadius < maxSize && descending == false)
        {
            lightsource.pointLightOuterRadius += Time.deltaTime * changeSpeed;
            if(lightsource.pointLightOuterRadius >= maxSize)
            {
                descending = true;
            }
        }
        else if (lightsource.pointLightOuterRadius > minSize && descending == true)
        {
            lightsource.pointLightOuterRadius -= Time.deltaTime * changeSpeed;
            if(lightsource.pointLightOuterRadius <= minSize)
            {
                descending = false;
            }
        }
    }

}
