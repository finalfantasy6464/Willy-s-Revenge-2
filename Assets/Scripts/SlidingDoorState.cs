using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorState : MonoBehaviour
{
    GameObject Switch1;
    SwitchCollide switchcollide;

    public AudioClip SlidingClose;
    public AudioClip SlidingOpen;

    public PositionalSFX sfx;

    // Start is called before the first frame update
    void Start()
    {
        Switch1 = GameObject.FindGameObjectWithTag("Switch");
        switchcollide = Switch1.GetComponent<SwitchCollide>();
        switchcollide.onSwitchToggle.AddListener(OnToggle);
    }

    void OnToggle(bool value)
    {
        if (value == false){
            sfx.clip = SlidingClose;
        }
        else
        {
            sfx.clip = SlidingOpen;
        }
        sfx.PlayPositionalSound();
    }
}
