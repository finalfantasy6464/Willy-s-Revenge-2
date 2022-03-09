using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool isEnabled = false;

    private void Start()
    {
        defaultSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        foreach(BridgeSwitch sw in BridgeSwitches)
        {
            if (sw.isEnabled)
            {
                sw.gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
                Bridge.GetComponent<BoxCollider2D>().enabled = true;
                Bridge.GetComponent<Animator>().SetBool("Extended", true);
                return;
            }
            else
            {
                sw.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
                Bridge.GetComponent<BoxCollider2D>().enabled = false;
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
            if (collidingWith.Count > 0)
            {
                isEnabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Player") || hit.CompareTag("Tail"))
        {
            collidingWith.Remove(coll);
            foreach (BridgeSwitch sw in BridgeSwitches)
            {
                if (collidingWith.Count > 0)
                {
                    return;
                }
            }
            isEnabled = false;
        }
    }
}
