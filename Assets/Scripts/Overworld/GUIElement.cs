using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIElement : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [HideInInspector] public bool isShowing => group.alpha != 0f;
    IEnumerator fadingRoutine;
    public bool wasShowing;
    public bool wasHiding;

    public event Action OnShow;
    public event Action OnHide;

    public void LateUpdate()
    {
        wasShowing = wasHiding = false;
    }
    
    public virtual void Hide()
    {
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        OnHide?.Invoke();
    }

    public void Hide(float time)
    {
        if(fadingRoutine != null)
            StopCoroutine(fadingRoutine);
        
        fadingRoutine = FadeRoutine(1f, 0f, time); 
        StartCoroutine(fadingRoutine);
    }

    public virtual void Show()
    {
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
        OnShow?.Invoke();
    }

    public virtual void Show(float time)
    {
        if(fadingRoutine != null)
            StopCoroutine(fadingRoutine);
        
        fadingRoutine = FadeRoutine(0f, 1f, time); 
        StartCoroutine(fadingRoutine);
    }

    public void Toggle()
    {
        if(group.alpha == 0f)
            Show();
        else
            Hide();
    }

    IEnumerator FadeRoutine(float start, float end, float time)
    {
        float counter = 0f;
        while(counter < time)
        {
            counter += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, counter / time);
            yield return null;
        }

        if(end == 1f) Show();
        else if(end == 0f) Hide();
    }


    public void InspectorHide()
    {
        if(isShowing)
            wasShowing = true;
        
        Hide();
    }

    public void InspectorShow()
    {
        if(!isShowing)
            wasHiding = true;
        
        Show();
    }

    public void InspectorToggle()
    {
        if(isShowing)
        {
            wasShowing = true;
            Hide();
        } else
        {
            wasHiding = true;
            Show();
        }
    }
}
