using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksand : MonoBehaviour, IPausable
{
    Transform player;
    Rigidbody2D move;
    Vector2 PlayerDirection;

    public float currentdistance;
    private float clampedDistance;
    private float currentpullstrength;

    public float maxDistance;
    public float minDistance;
    public float pullstrengthmin;
    public float pullstrengthmax;



    bool quicksandenable = false;
    GameObject PlayerInfo;

    public bool isPaused { get; set; }

    // Use this for initialization
    void Start()
    {
        PlayerInfo = GameObject.FindGameObjectWithTag("Player");
        player = PlayerInfo.transform;
        move = PlayerInfo.GetComponent<Rigidbody2D>();
    }

   
        

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
            UnPausedUpdate();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.tag == ("Player"))
        {
            quicksandenable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            quicksandenable = false;
            move.velocity = new Vector2 (0,0);
        }
    }

    public void OnPause()
    {
        move.velocity = new Vector2(0, 0);
    }

    public void OnUnpause()
    { }

    public void UnPausedUpdate()
    {
        Updatepullstrength();
        if(quicksandenable)
        {
            if (PlayerInfo != null)
            {
                PlayerDirection = -(transform.position - player.position).normalized;
                move.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y) * -currentpullstrength;
            }
        }
    }

    private void Updatepullstrength()
    {
        if(player != null)
        {
            currentdistance = Vector2.Distance(transform.position, player.position);
            clampedDistance = Mathf.Clamp(currentdistance, minDistance, maxDistance);
            currentpullstrength = Mathf.Lerp(pullstrengthmax, pullstrengthmin, clampedDistance / maxDistance);
        }
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
