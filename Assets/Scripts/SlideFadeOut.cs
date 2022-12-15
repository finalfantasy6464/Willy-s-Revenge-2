using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideFadeOut : MonoBehaviour
{
    public Image slideRenderer;
    public float fadeTime;
    float fadeCounter;

    void Start()
    {
        fadeCounter = -0.1f;
    }

    void Update()
    {
        fadeCounter += Time.deltaTime;
        slideRenderer.color = Color.Lerp(Color.black, Color.clear, fadeCounter / fadeTime);

        if(fadeCounter >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
