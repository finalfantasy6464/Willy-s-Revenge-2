using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GatePin : MonoBehaviour
{
    [Header("Gate")]
    public int completerequired;
    public int gatepinIndex;

    public Sprite greensprite;
    public Sprite destroyedsprite;

    public AudioClip shatter;
    public AudioClip bounce;

    public bool locked;
    public bool destroyed;

    public GameObject Crackedorb;
    public GameObject Completeorb;
    public GameObject Barrier;

    public MapManager map;
    [HideInInspector] public UnityEvent OnLevelLoaded = new UnityEvent();
    Animator m_Animator;

    public OverworldFollowCamera overworldCamera;

    void Start()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if(GameControl.control.complete >= completerequired)
        {
            Barrier.SetActive(false);
            m_Animator.SetTrigger("Green");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out OverworldPlayer player))
        {
            overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.gatepinPreview);
            overworldCamera.SetTarget(transform);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out OverworldPlayer player))
        {
            overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.FreeRoam);
            overworldCamera.SetTarget(player.transform);
        }
    }

    public void SetOrbState(bool locked, bool destroyed)
    {
        Completeorb.SetActive(locked && !destroyed);
        Crackedorb.SetActive(!locked && destroyed);
        Barrier.SetActive(locked);
    }

    public void PlaySound(AudioClip clip)
    {
        GameSoundManagement.instance.PlayOneShot(clip);
    }
}
