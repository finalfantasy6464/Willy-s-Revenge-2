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

    void Awake()
    {
        OnLevelLoaded.AddListener(DestroyActivate);
    }

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

    //This checks the state of the world orbs when the level is loaded.
    public void DestroyActivate()
    {
            Barrier.SetActive(false);
            Crackedorb.SetActive(true);
            Completeorb.SetActive(false);
            destroyed = true;
            locked = false;
    }

    //This checks the state of the world orbs when the player attempts to pass through it

    public void LockCheck()
    {
        if (GameControl.control.complete >= completerequired)
        {
            locked = false;
            if (!destroyed)
                PlaySound(shatter);

            destroyed = true;
            map.UnlockAndDestroyGate(this);
        } else
        {
            locked = true;
            PlaySound(bounce);
        }
        Barrier.SetActive(locked);
        Completeorb.SetActive(locked);
        Crackedorb.SetActive(!locked);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out OverworldPlayer player))
        {
            overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.gatepinPreview);
            overworldCamera.SetTarget(transform);
            GatepinPreviewWindow previewWindow = map.overworldGUI.gatepinPreview as GatepinPreviewWindow;
            previewWindow.UpdatePreviewData(this);
            previewWindow.Show();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out OverworldPlayer player))
        {
            overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.FreeRoam);
            overworldCamera.SetTarget(player.transform);
            map.overworldGUI.gatepinPreview.Hide();
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
