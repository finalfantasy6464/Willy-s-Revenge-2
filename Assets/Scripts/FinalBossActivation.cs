﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class FinalBossActivation : MonoBehaviour
{
    public BigOrangeEntrance boEntrance;
    BigOrange orangescript;
    public GameObject orange;

    public bool battleended;

    public Light2D[] arenaLights;
    public Light2D globalLight;

    public GameObject Timer;
    public TextMeshProUGUI timerText;

    public GameObject HPBarImage;
    public GameObject CurrentHPtext;
    public GameObject HPText;

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
            BattleActivated();
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
        Timer.SetActive(true);
        timerText.gameObject.SetActive(true);

        foreach (Light2D arenalight in arenaLights)
        {
            arenalight.gameObject.SetActive(true);
        }

        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        globalLight.intensity = 0.05f;

        HPBarImage.SetActive(true);
        HPText.SetActive(true);
        CurrentHPtext.SetActive(true);
        playerScript.canmove = true;
    }

    public void BattleEnd()
    {
        {
            cameras[1].gameObject.SetActive(false);
            cameras[0].gameObject.SetActive(true);
            battleended = true;

            foreach(Light2D arenalight in arenaLights)
            {
                arenalight.gameObject.SetActive(false);
            }
            HPBarImage.SetActive(false);
            HPText.SetActive(false);
            CurrentHPtext.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.tag == "Player")
        {
            playerScript.canmove = false;
            boEntrance.m_animator.enabled = true;
            boEntrance.m_animator.Play("entranceMain");
            m_collider.enabled = false;
            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
        }
    }
}
