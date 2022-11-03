using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchwall : MonoBehaviour
{
    PlayerCollision playerColl;

    private void Start()
    {
        playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Player"))
        {
            playerColl.Die(playerColl.onWallCollide);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
