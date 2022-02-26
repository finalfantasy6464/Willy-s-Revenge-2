using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class abysscollision : MonoBehaviour
{

    public PlayerController2021remake player;
    public PlayerCollision playerColl;
    public GameObject lastTail;

    public List<Collider2D> collidingWith;

    public List<Collider2D> currentTails;

    private void Update()
    {
        for (int i = 0; i < player.taillist.Count; i++)
        {
            if (player.taillist[i].activeInHierarchy)
            {
                lastTail = player.taillist[i];
            }
        }

        if (collidingWith.Contains(player.GetComponent<Collider2D>()) && collidingWith.Contains(lastTail.GetComponent<Collider2D>()))
        {
            playerColl.Die(playerColl.onFalling);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        collidingWith.Add(coll);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        collidingWith.Remove(coll);
    }
}
