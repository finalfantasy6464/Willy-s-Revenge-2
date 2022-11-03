using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BridgeSwitch : MonoBehaviour
{
    public PlayerController2021remake player;
    public PlayerCollision playerColl;
    public GameObject lastTail;

    public List<Collider2D> collidingWith;

    public BridgeSwitch[] BridgeSwitches;

    public List<Collider2D> currentTails;

    public GameObject Bridge;

    public SpriteRenderer spriteRenderer;

    public Sprite activeSprite;
    private Sprite defaultSprite;

    public bool isEnabled = false;

    private void Start()
    {
        defaultSprite = spriteRenderer.sprite;
    }

    private void FixedUpdate()
    {
        {
            if(BridgeSwitches.All(sw => !sw.isEnabled))
            {
                foreach(BridgeSwitch sw in BridgeSwitches)
                {
                    sw.spriteRenderer.sprite = sw.defaultSprite;
                }
                Bridge.GetComponent<Animator>().SetBool("Extended", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if(hit.CompareTag("Player") || hit.CompareTag("Tail"))
        {
            collidingWith.Add(coll);
            Bridge.GetComponent<Animator>().SetBool("Extended", true);
            Bridge.GetComponent<Collider2D>().enabled = true;
            if (collidingWith.Count > 0)
            {
                isEnabled = true;
                foreach (BridgeSwitch sw in BridgeSwitches)
                {
                    sw.spriteRenderer.sprite = sw.activeSprite;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Player") || hit.CompareTag("Tail"))
        {
            collidingWith.Remove(coll);
            if(collidingWith.Count == 0)
            {
                isEnabled = false;
            }
        }
    }
}
