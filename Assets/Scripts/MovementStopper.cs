using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStopper : MonoBehaviour
{
    PlayerController2021remake playerController;
    GameObject player;

    private bool currentlycolliding;

    private float movementtimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController2021remake>();
    }

    private void Update()
    {
        if(currentlycolliding == true)
        {
            movementtimer += Time.deltaTime;
            if(movementtimer >= 0.1f)
            {
                playerController.canmove = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && currentlycolliding == false)
        {
            playerController.canmove = false;

            player.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z);
            currentlycolliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Player" && currentlycolliding == true)
        {
            currentlycolliding = false;
            movementtimer = 0;
        }
    }
}
