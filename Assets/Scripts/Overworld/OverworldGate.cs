using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Space]
    public AudioClip failSound;
    public AudioClip unlockSound;


    IEnumerator Start()
    {
        yield return null;
        SetValue(Mathf.Max(0, requiredAmount - GameControl.control.complete));
        if(GameControl.control.complete >= requiredAmount)
        {
            if(!GameControl.control.destroyedgates[gateIndex])
                myAnimator.SetTrigger("OnUnlockReady");
            else
                myAnimator.SetTrigger("OnUnlockedFromStart");
        }

        player.OnSelect += GatePlateCheck;
        gatePlate.OnStepIn += () => SetPlateState(true);
        gatePlate.OnStepOut += () => SetPlateState(false);
    }

    void OnDisable()
    {
        player.OnSelect -= GatePlateCheck;
        gatePlate.OnStepIn -= () => SetPlateState(true);
        gatePlate.OnStepOut -= () => SetPlateState(false);
    }

    void SetPlateState(bool value)
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
        else if (GameControl.control.complete >= requiredAmount)
        {
            myAnimator.SetTrigger("OnUnlock");
            GameSoundManagement.instance.PlayOneShot(unlockSound);
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
