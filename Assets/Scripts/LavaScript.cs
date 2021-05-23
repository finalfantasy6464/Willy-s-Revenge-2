using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LavaScript : MonoBehaviour
{

    public Sprite[] LavaSprites;

    private float lavatimer = 0.0f;
    public float lavastep = 0.075f;
    public int CurrentSprite = 1;
    public int mysprite = 0;

    public AudioClip burned;

    void Update()
    {

        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = LavaSprites[mysprite];
        this.lavatimer += Time.deltaTime;

        if (lavatimer >= lavastep)
        {
            CurrentSprite += 1;
            lavatimer = 0.0f;
            mysprite++;

            if (CurrentSprite == 28)
            {
                CurrentSprite = 0;
            }

            if (mysprite == LavaSprites.Length)
            {
                mysprite = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D Lava)
    {

        var mysprite = this.GetComponent<SpriteRenderer>().sprite;
        var hit = Lava.gameObject;

        if (hit.tag == "Player" & mysprite == LavaSprites[13] | hit.tag == "Player" & mysprite == LavaSprites[14])
        {
            Destroy(hit);
            GameSoundManagement.instance.PlayOneShot(burned);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

   
        }
    }
}