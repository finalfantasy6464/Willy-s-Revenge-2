using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

public class FinalBossActivation : MonoBehaviour
{
    public BigOrangeEntrance boEntrance;
    BigOrange orangescript;
    public GameObject orange;
    public ExitCollision Exit;
    public LevelTimer timer;

    public bool battleended;

    public UnityEngine.Rendering.Universal.Light2D[] arenaLights;
    public UnityEngine.Rendering.Universal.Light2D globalLight;

    public GameObject[] ActivatedObjects;
    public GameObject[] DeactivatedObjects;

    public MusicManagement music;

    public Camera[] cameras;

    public PlayerController2021remake playerScript;

    private BoxCollider2D m_collider;

    private void Start()
    {
        orangescript = orange.GetComponent<BigOrange>();
        m_collider = GetComponent<BoxCollider2D>();
        music = GameObject.Find("SoundManager").GetComponent<MusicManagement>();

        if (GameControl.control.bosscheckpoint == true)
        {
            boEntrance.SpawnOrange();
            boEntrance.DestroySelf();
            playerScript.transform.position = transform.position;
            orangescript.m_animator.Play("Idle");
            m_collider.enabled = false;
            timer.isCounting = true;

            ActivateObjects();
            UpdateCameras();
            UpdatePlayer();
        }
    }

    public void Update()
    {
      if(battleended && globalLight.intensity < 0.85f)
        {
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0.85f, Time.deltaTime);
        }  
    }

    public void BattleActivated()
    {
        if(GameControl.control.bosscheckpoint == false)
        {
            music.musicSource.clip = music.musicClips[31];
            music.musicSource.loop = true;
            music.musicSource.Play();
            GameControl.control.bosscheckpoint = true;
        }

        timer.isCounting = true;
        ActivateObjects();
        UpdateCameras();
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        playerScript.enteredCannon = false;
        playerScript.canmove = true;
        playerScript.canrotate = true;
    }

    private void UpdateCameras()
    {
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        globalLight.intensity = 0.05f;
    }

    public void ActivateObjects()
    {
        foreach (GameObject obj in ActivatedObjects)
        {
            obj.SetActive(true);
        }

        foreach (UnityEngine.Rendering.Universal.Light2D arenalight in arenaLights)
        {
            arenalight.gameObject.SetActive(true);
        }
    }

    public void BattleEnd()
    {
        {
            cameras[1].gameObject.SetActive(false);
            cameras[0].gameObject.SetActive(true);
            battleended = true;

            foreach(UnityEngine.Rendering.Universal.Light2D arenalight in arenaLights)
            {
                arenalight.gameObject.SetActive(false);
            }

            foreach (GameObject obj in DeactivatedObjects)
            {
                obj.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.tag == "Player")
        {
            playerScript.canmove = false;
            playerScript.canrotate = false;
            playerScript.enteredCannon = true;
            boEntrance.m_animator.enabled = true;
            boEntrance.m_animator.Play("entranceMain");
            m_collider.enabled = false;
            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
        }
    }
}
