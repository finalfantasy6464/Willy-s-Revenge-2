using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Collision : MonoBehaviour
{
    SpriteRenderer m_sprite;
    Color defaultColor;
    Color changeColor = new Color(1, 1, 1, 0.4f);
    AIPath pathing;

    ParticleSystem particles;
    ParticleSystem.EmissionModule emissionModule;

    float defaultAccel;
    float defaultTopSpeed;
    float defaultpathrate;

    public void Start()
    {
        particles = GetComponent<ParticleSystem>();
        emissionModule = particles.emission;
        pathing = GetComponent<AIPath>();
        defaultAccel = pathing.maxAcceleration;
        defaultTopSpeed = pathing.maxSpeed;
        defaultpathrate = pathing.repathRate;
        m_sprite = GetComponent<SpriteRenderer>();
        defaultColor = m_sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hit = collision.gameObject;

        if (hit.CompareTag("Corruption"))
        {
            pathing.maxAcceleration = pathing.maxAcceleration * 2f;
            pathing.maxSpeed = pathing.maxSpeed * 2f;
            pathing.repathRate = pathing.repathRate / 2f;
            emissionModule.rateOverTime = 10f;
        }
    }
    private void OnTriggerStay2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if(hit.CompareTag("Enemy3") || hit.CompareTag("Bullet"))
        {
            m_sprite.color = changeColor;
        }


    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if(hit.CompareTag("Enemy3") || hit.CompareTag("Bullet"))
        {
            m_sprite.color = defaultColor;
        }

        if (hit.CompareTag("Corruption"))
        {
            pathing.maxAcceleration = defaultAccel;
            pathing.maxSpeed = defaultTopSpeed;
            pathing.repathRate = defaultpathrate;
            emissionModule.rateOverTime = 0f;
        }
    }
}
