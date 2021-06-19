using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GatePin : MonoBehaviour
{
    Animator m_Animator;
    private Pin pin;
    private int completerequired;

    public Sprite greensprite;
    public Sprite destroyedsprite;

    public AudioSource source;
    public AudioClip clip;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        pin = this.GetComponent<Pin>();
        completerequired = pin.completerequired;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameControl.control.complete >= completerequired)
        {
            this.GetComponent<SpriteRenderer>().sprite = greensprite;
            m_Animator.SetTrigger("Green");
        }
    }
}
