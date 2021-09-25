using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrowsDisplay : MonoBehaviour
{
    public Transform[] arrows;
    public float animTime;
    public float animCounter;
    
    List<Vector3> arrowStarts;
    List<SpriteRenderer> arrowRenderers;
    bool isPlayerOnPin;

    void Start()
    {
        arrowStarts = new List<Vector3>();
        arrowRenderers = new List<SpriteRenderer>();
        for (int i = 0; i < arrows.Length; i++)
        {
            arrowStarts.Add(arrows[i].position);
            arrowRenderers.Add(arrows[i].GetComponent<SpriteRenderer>());
        }
    }

    void LateUpdate()
    {
        if(!isPlayerOnPin)
            return;

        animCounter += Time.deltaTime;

        if(animCounter > animTime)
            animCounter = 0f;
        
        for (int i = 0; i < arrows.Length; i++)
        {
            if(!arrows[i].gameObject.activeInHierarchy)
                continue;

            arrows[i].position = arrowStarts[i] + arrows[i].right
                    * Mathf.Lerp(0f, 0.25f, Mathf.Min(1f, ((animCounter * 2f)/animTime)));
            arrowRenderers[i].color = Color.Lerp(
                    new Color(1f, 1f, 1f, 0f), Color.white, ((animCounter * 2f)/animTime));
        }
    }

    public void ResetAnimation()
    {
        animCounter = 0f;
    }

    public void SetArrow(PathDirection direction, bool state)
    {
        arrows[(int)direction - 1].gameObject.SetActive(state);
    }

    public void SetIsPlayerOnPin(bool value)
    {
        if(isPlayerOnPin && !value)
            ResetAnimation();

        isPlayerOnPin = value;
    }
}

