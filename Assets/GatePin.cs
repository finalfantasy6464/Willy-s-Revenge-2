using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GatePin : MonoBehaviour
{
    Animator m_Animator;
    private Pin pin;
    public int completerequired;

    public Sprite greensprite;
    public Sprite destroyedsprite;

    public AudioSource source;
    public AudioClip clip;

    public bool locked;
    public bool destroyed;

    public GameObject Crackedorb;
    public GameObject Completeorb;
    public GameObject Barrier;

    [HideInInspector]
    public UnityEvent OnLevelLoaded = new UnityEvent();

    private void Awake()
    {
        OnLevelLoaded.AddListener(DestroyCheck);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        pin = this.GetComponent<Pin>();


    }

    // Update is called once per frame
    void Update()
    {
        if(GameControl.control.complete >= completerequired)
        {
            Barrier.SetActive(false);
            m_Animator.SetTrigger("Green");
        }
    }

    //This checks the state of the world orbs when the level is loaded.

    void DestroyCheck()
    {
        if (!locked)
        {
            Barrier.SetActive(false);
        }
        Crackedorb.SetActive(false);
    }

    //This checks the state of the world orbs when the player attempts to pass through it

    public bool LockCheck()
    {
        
        if (GameControl.control.complete >= completerequired)
        {
            locked = false;
        }
        Barrier.SetActive(locked);
        Completeorb.SetActive(locked);
        Crackedorb.SetActive(!locked);
        return locked;
    }
}
