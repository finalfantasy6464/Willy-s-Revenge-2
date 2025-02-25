﻿
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LaserCannon : MonoBehaviour, IPausable
{
    private float RaycastMax = 100f;

    private LayerMask layer;

    public bool usestimer = false;

    private bool Raycaststopped;

    public float offset;
    public float firerate;
    public float firereset;
    public float shootLength;

    public LineRenderer beamRenderer;
    public ParticleSystem Endparticles;
    public ParticleSystem Startparticles;

    public AudioClip elec;

    public AudioClip laserstart;
    public AudioClip laserstop;

    public AudioSource source;

    public LocalAudioPlayer audioplay;

    Vector3 direction;
    Vector2 endpos;
    Vector2 midpos;
    Vector2 thickness;

    public Material material;

    public Transform raycastPoint;

    RaycastHit2D hit;

    [HideInInspector] public UnityEvent onElectricHit;

    public bool isPaused { get; set; }

    private void Awake()
    {
        onElectricHit = new UnityEvent();
    }

    private void Start()
    {
        raycastPoint = this.transform;
        shootLength = offset;
        layer = LayerMask.GetMask("Walls", "Player");
    }

    private void Update()
    {
        if (!isPaused)
            UnPausedUpdate();
    }

    private void RaycastDirection()
    {
        direction = -transform.up;

        hit = Physics2D.Raycast(raycastPoint.position, direction, RaycastMax, layer);

        Vector2 endpos = hit.point;

        if (hit.collider != null)
        {
            beamRenderer.SetPosition(0, raycastPoint.position);
            beamRenderer.SetPosition(1, endpos);
        }

        PlayerCollision player = hit.collider.GetComponent<PlayerCollision>();
        if (player != null)
        {
            player.Die(onElectricHit);
            GameSoundManagement.instance.PlaySingle(elec);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void RaycastStop()
    {
        beamRenderer.enabled = false;
        Endparticles.Stop();
        Startparticles.Stop();
    }

    private void UpdateParticles()
    {
        Vector2 startpos;
        Vector2 endposition;

        if (hit.collider != null)
        {
         startpos = this.beamRenderer.GetPosition(0) + (-transform.up * 0.2f);
         endposition = this.beamRenderer.GetPosition(1);

            Startparticles.transform.position = new Vector3(startpos.x, startpos.y, -5.0f);
            Endparticles.transform.position = new Vector3 (endposition.x, endposition.y, -5.0f);
            Endparticles.Play();
            Startparticles.Play();
        }
    }

    public void OnPause()
    {
        material.SetVector("_Speed", Vector2.zero);
    }
        

    public void OnUnpause()
    {
        material.SetVector("_Speed", new Vector2(-0.2f,0));
    }

    public void UnPausedUpdate()
    {
        if (usestimer == false)
        {
            RaycastDirection();
            UpdateParticles();
            if (!source.isPlaying)
            {
                source = audioplay.SoundPlay();
            }
        }

        if (usestimer == true)
        {
            shootLength += Time.deltaTime;

            if (shootLength < firerate)
            {
                RaycastDirection();
                UpdateParticles();
                Raycaststopped = false;
            }

            if (shootLength >= firerate && shootLength < (firerate + 0.1f) && Raycaststopped == false)
            {
                RaycastStop();
                source.Stop();
                audioplay.soundData.clip = laserstop;
                source = audioplay.SoundPlay();
                Raycaststopped = true;
            }

            if (shootLength >= (firerate + firereset) && Raycaststopped == true)
            {
                shootLength = 0;
                beamRenderer.enabled = true;
                audioplay.soundData.clip = laserstart;
                source = audioplay.SoundPlay();

            }
        }
    }
    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
