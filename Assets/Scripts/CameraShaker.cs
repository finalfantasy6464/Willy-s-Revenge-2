using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    IEnumerator shakeRoutine;
    Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = transform.position;
    }

    public void Shake(float horizontalshake, float verticalshake)
    {
        if(shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            transform.position = defaultPosition;
        }

        shakeRoutine = ShakeRoutine(horizontalshake, verticalshake);
        StartCoroutine(shakeRoutine);
    }

    IEnumerator ShakeRoutine(float horizontalshake, float verticalshake)
    {
        Vector3 shakeVector = new Vector3(Random.Range(-horizontalshake, horizontalshake), Random.Range(-verticalshake, verticalshake), 0f).normalized;
        float shakeMagnitude = 0.5f;
        const float shakeMagnitudeDecay = 0.017f;

        while(shakeMagnitude > 0f)
        {
            float sin = Mathf.Sin(Time.time * 10f);
            transform.position = defaultPosition + shakeVector * sin * shakeMagnitude;
            shakeMagnitude -= shakeMagnitudeDecay;
            yield return null;
        }
    }
}
