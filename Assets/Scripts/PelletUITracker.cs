using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PelletUITracker : MonoBehaviour
{
    Transform GoldenPellet;
    SpriteRenderer sprenderer;
    Color defaultColor;
    GameObject Player;

    public float distToPellet;
    private float fadeDistance = 5.0f;
    private float maxDistance = 30.0f;
    public float intensity;

    public void Start()
    {
        sprenderer = GetComponentInChildren<SpriteRenderer>();
        sprenderer.color = new Color(0.95f, 0.945f, 0.5f, 0.75f);
        defaultColor = sprenderer.color;
        sprenderer.color = Color.clear;
        FindPellet();
    }

    public void FindPellet()
    {
        GoldenPellet = GameObject.FindGameObjectWithTag("GoldenPickup").transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.parent = Player.transform;
        transform.localPosition = Vector2.zero;
    }
    private void Update()
    {
        if(GoldenPellet != null && Player.GetComponent<PlayerCollision>().canbehit == true)
        {
            Vector3 diff = GoldenPellet.position - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

            distToPellet = Vector2.Distance(GoldenPellet.position, Player.transform.position);

            if (distToPellet < fadeDistance || distToPellet > maxDistance)
            {
                sprenderer.color = Color.Lerp(sprenderer.color, Color.clear, Time.deltaTime * 2f);
                if(sprenderer.color.a < 0.1f)
                {
                    sprenderer.enabled = false;
                }
            }
            else
            {
                if (sprenderer.enabled == false)
                {
                    sprenderer.enabled = true;
                }
                    intensity = fadeDistance / distToPellet;
                    sprenderer.color = Color.Lerp(sprenderer.color, defaultColor * intensity, Time.deltaTime * 16f);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
