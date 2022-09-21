using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boulder : MonoBehaviour, IPausable
{
    BigOrange orangeScript;
    PlayerController playercontroller;
    GameObject orange;
    SpriteRenderer sprite;

    GameObject[] FloorSwitches;
    RadialActivate[] activate;

    public Color currentColour;

    public AudioClip falling;
    public AudioClip hitting;

    bool justhit = false;

    public float maxYVelocity;
    int bouldertotal;
    Rigidbody2D rb;
    public Vector2 storedForce;

    public List<bool> activated = new List<bool>();

    public bool isPaused { get; set; }

    void Start()
    {
        orange = GameObject.FindGameObjectWithTag("Boss");
        orangeScript = orange.GetComponent<BigOrange>();
        FloorSwitches = GameObject.FindGameObjectsWithTag("Switch");
        activate = new RadialActivate[FloorSwitches.Length];
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GameSoundManagement.instance.PlayOneShot(falling);

        for (int i = 0; i < FloorSwitches.Length; i++)
        {
            activate[i] = FloorSwitches[i].GetComponent<RadialActivate>();
        }
    }

    void Update()
    {
        if(!isPaused)
            UnPausedUpdate();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if(hit.tag == "Enemy3")
        {
            Destroy(gameObject);
        }

        if (hit.tag == "Boss" && justhit == false)
        {
            justhit = true;
            for (int l = 0; l < activate.Length; l++)
            {
                bouldertotal += activate[l].boulderamount;
                activated.Add(activate[l].isActive);

                if (!activated.Contains(true) && activated.Count == 5)
                {
                    for (int k = 0; k < activate.Length; k++)
                    {
                        activate[k].isActive = true;
                    }
                    activated.Clear();
                }
            }

            if(orangeScript.TakeDamage((int)(35 * Mathf.Pow(bouldertotal, 3))))
            {
                for (int l = 0; l < activate.Length; l++)
                {
                    Destroy(activate[l].gameObject);
                }
            }
            GameSoundManagement.instance.efxSource.pitch = 1.0f;
            GameSoundManagement.instance.PlayOneShot(hitting);
            Destroy(gameObject);
        }
    }

    public void SetForce(Vector2 f)
	{
		storedForce = f;
	}

    public void LateUpdate()
    {
        justhit = false;
    }

    public void OnPause()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();

        storedForce = rb.velocity;
        if (rb != null && rb.constraints != RigidbodyConstraints2D.FreezeAll)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void OnUnpause()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(storedForce, ForceMode2D.Impulse);
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }

    public void UnPausedUpdate()
    {
        if(rb.velocity.y > maxYVelocity)
        {
            rb.AddForce(Vector2.down * 5f);
        }
    }
}