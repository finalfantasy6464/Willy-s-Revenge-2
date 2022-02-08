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

    public void Shake()
    {
        if(shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            transform.position = defaultPosition;
        }

        shakeRoutine = ShakeRoutine();
        StartCoroutine(shakeRoutine);
    }

    IEnumerator ShakeRoutine()
    {
        Vector3 shakeVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        float shakeMagnitude = 1f;
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
