using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStopper : MonoBehaviour
{
    PlayerController playerController;
    GameObject player;

    private bool currentlycolliding;
    private Sprite storedPlayerSprite;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && currentlycolliding == false)
        {
            playerController.presentdir = 5;

            player.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z);
            currentlycolliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Player" && currentlycolliding == true)
        {
            currentlycolliding = false;
        }
    }
}
