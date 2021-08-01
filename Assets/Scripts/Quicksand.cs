using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    Transform player;
    Rigidbody2D move;
    float timeStamp;
    Vector2 PlayerDirection;
    public float pullstrength;
    bool quicksandenable = false;
    GameObject PlayerInfo;

    // Use this for initialization
    void Start()
    {
        PlayerInfo = GameObject.FindGameObjectWithTag("Player");
        move = PlayerInfo.GetComponent<Rigidbody2D>();
    }

   
        

    // Update is called once per frame
    void Update()
    {
       if (quicksandenable)
        {
            if (PlayerInfo != null)
            {
                PlayerDirection = -(transform.position - player.position).normalized;
                move.velocity = new Vector2(PlayerDirection.x, PlayerDirection.y) * pullstrength * (Time.timeSinceLevelLoad / timeStamp);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.tag == ("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            timeStamp = Time.timeSinceLevelLoad;
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
}
