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
        foreach (Collider2D tail in player.transform.parent.GetComponentsInChildren<Collider2D>())
        {
            if(!collidingWith.Contains(tail))
                return;
        }

        playerColl.Die(playerColl.onFalling);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
