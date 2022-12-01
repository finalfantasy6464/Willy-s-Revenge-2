using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTransitionPressurePlate : MonoBehaviour
{
    public string AnimationTriggerName;
    
    [Space]
    public Animator worldAnimator;
    public Animator cinematicCameraAnimator;

    [Space]
    public Camera cinematicCamera;
    public Camera followCamera;

    [Space]
    public OverworldPlayer player;
    public OverworldGUI overworldGUI;
    public Transform lerpPoint;
    public event Action OnStepIn;
    public event Action OnStepOut;
    public bool active;

    void Start()
    {
        OnStepIn  += () => SetPlateActive(true);
        OnStepOut  += () => SetPlateActive(false);
        player.OnSelect += TransitionCheck;
    }

    void SetPlateActive(bool value)
    {
        active = value;
    }

    void TransitionCheck()
    {
        if(!active || overworldGUI.isAnyShowing)
            return;

        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        player.transform.right = lerpPoint.position - player.transform.position;
        while(Vector3.Distance(player.transform.position, lerpPoint.position) > 0.05f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                    lerpPoint.position, player.moveSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        worldAnimator.enabled = true;
        yield return null;

        worldAnimator.Play(AnimationTriggerName, -1, 0f);
        cinematicCameraAnimator.Play(AnimationTriggerName, -1, 0f);
        cinematicCamera.depth = 2;
        followCamera.depth = 1;
        player.input.enabled = false;

        yield return null;
        while(cinematicCameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
        
        player.input.enabled = true;
        cinematicCamera.depth = 1;
        followCamera.depth = 2;
        worldAnimator.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
            OnStepIn.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
            OnStepOut.Invoke();
    }
}
