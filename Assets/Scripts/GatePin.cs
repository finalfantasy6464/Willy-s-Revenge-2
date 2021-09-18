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

    public AudioClip shatter;
    public AudioClip bounce;

    public bool locked;
    public bool destroyed;

    public GameObject Crackedorb;
    public GameObject Completeorb;
    public GameObject Barrier;

    public MapManager mapmanager;

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

        for (int i = 0; i < mapmanager.worldgates.Count; i++)
        {
            if (mapmanager.worldgates[i] == this && GameControl.control.lockedgatescache.Count != 0)
            {
                locked = GameControl.control.lockedgatescache[i];
                destroyed = GameControl.control.destroyedgatescache[i];
            }
            else
            {
                if (mapmanager.worldgates[i] == this && GameControl.control.lockedgatescache.Count == 0)
                {
                    locked = GameControl.control.lockedgates[i];
                    destroyed = GameControl.control.destroyedgates[i];
                }
            }
        }

        SetOrbState(locked, destroyed);
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
            Crackedorb.SetActive(true);
        }
    }

    //This checks the state of the world orbs when the player attempts to pass through it

    public bool LockCheck()
    {
        
        if (GameControl.control.complete >= completerequired)
        {
            locked = false;
            if (!destroyed)
            {
                PlaySound(shatter);
            }
            destroyed = true;
            

            for(int i = 0; i < mapmanager.worldgates.Count; i++)
            {
               if(mapmanager.worldgates[i] == this)
                {
                    GameControl.control.lockedgates[i] = false;
                    GameControl.control.destroyedgates[i] = true;
                }
            }
        }
        Barrier.SetActive(locked);
        Completeorb.SetActive(locked);
        Crackedorb.SetActive(!locked);
        return locked;
    }

    public void SetOrbState(bool locked, bool destroyed)
    {
        if (!locked && destroyed)
        {
            Completeorb.SetActive(false);
            Crackedorb.SetActive(true);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        GameSoundManagement.instance.PlayOneShot(clip);
    }
}
