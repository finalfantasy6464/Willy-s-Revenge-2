using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangwayEmitter : MonoBehaviour, IPausable
{
    Transform m_Transform;
    public GameObject emitObject;

    public float emitTimer;
    public float emitCounter;

    public float emitrateMin = 3f;
    public float emitrateMax = 15f;

    public bool isPaused { get; set; }


    private void Start()
    {
        m_Transform = GetComponent<Transform>();
        RerollTimer();
    }

    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }

    public void OnDestroy()
    {
        
    }

    public void OnPause()
    {
        
    }

    public void OnUnpause()
    {
        
    }

    public void UnPausedUpdate()
    {
        emitCounter += Time.deltaTime;

        if(emitCounter > emitTimer)
        {
            GameObject GangwayEnemy2 = Instantiate(emitObject);
            GangwayEnemy2.transform.position = m_Transform.position;
            GangwayEnemy2.transform.rotation = m_Transform.rotation;
            emitCounter = 0f;
            RerollTimer();
        }
    }

    private void RerollTimer()
    {
        emitTimer = UnityEngine.Random.Range(emitrateMin, emitrateMax);
    }
}
