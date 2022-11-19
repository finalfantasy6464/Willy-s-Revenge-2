using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class OverworldGate : MonoBehaviour
{
    public int gateIndex;
    public int requiredAmount;
    public bool focused;   
    public GatePromptButton promptButton;
    public TextMeshProUGUI tensLabel;
    public TextMeshProUGUI unitsLabel;
    public GatePressurePlate gatePlate;
    public OverworldPlayer player;
    public Animator myAnimator;

    public bool locked;
    public bool destroyed;

    [Space]
    public AudioClip failSound;
    public AudioClip unlockSound;


    IEnumerator Start()
    {
        yield return null;
        player.OnSelect += GatePlateCheck;
        player.OpenMenu += () => SetPlateState(false);
        gatePlate.OnStepIn += () => SetPlateState(true);
        gatePlate.OnStepOut += () => SetPlateState(false);
        SetGateState();
    }

    public void SetGateState()
    {
        SetValue(Mathf.Max(0, requiredAmount - GameControl.control.complete));
        if (GameControl.control.complete >= requiredAmount)
        {
            if (!GameControl.control.destroyedgates[gateIndex])
                myAnimator.SetTrigger("OnUnlockReady");
            else
                myAnimator.SetTrigger("OnUnlockFromStart");
        }
        else
        {
            if(gameObject.activeInHierarchy == true)
            myAnimator.SetTrigger("OnLock");
        }
    }

    void OnDisable()
    {
        player.OnSelect -= GatePlateCheck;
        player.OpenMenu -= () => SetPlateState(false);
        gatePlate.OnStepIn -= () => SetPlateState(true);
        gatePlate.OnStepOut -= () => SetPlateState(false);
    }

    public void SetPlateState(bool value)
    {
        focused = value;
        promptButton.gameObject.SetActive(value);
    }

    void GatePlateCheck()
    {
        if(focused && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Locked"))
        {
            myAnimator.SetTrigger("OnUnlockFail");
            GameSoundManagement.instance.PlayOneShot(failSound);
        }
        else if (focused && GameControl.control.complete >= requiredAmount)
        {
            myAnimator.SetTrigger("OnUnlock");
            SetPlateState(false);
            GameSoundManagement.instance.PlayOneShot(unlockSound);
            GameControl.control.destroyedgates[gateIndex] = true;
        }
    }

    void Update()
    {
        
    }

    public void SetValue(int value)
    {
        tensLabel.SetText(value < 10 ? "0" : value.ToString().Substring(0, 1));
        unitsLabel.SetText(value < 10 ? value.ToString() : value.ToString().Substring(1, 1));
    }
}
