using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public SpriteRenderer[] toUnfade;
    public SpriteRenderer[] toFade;
    public SpriteRenderer[] toVanish;

    public GameObject[] objectsToTurnOff;
    public GameObject[] objectsToTurnOn;

    public GameObject TwinToggle;

    public OverworldCharacter character;


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            {
                foreach (SpriteRenderer sprite in toUnfade)
                {
                    if (sprite.enabled == false)
                        sprite.enabled = true;
                    sprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    sprite.gameObject.layer = 0;
                }

                foreach (SpriteRenderer sprite in toFade)
                {
                    sprite.color = new Color(0.25f, 0.25f, 0.25f, 0.5f);
                    sprite.gameObject.layer = 20;
                }

                foreach (SpriteRenderer sprite in toVanish)
                {
                    if (sprite.enabled == true)
                    {
                        sprite.enabled = false;
                    }
                }

                foreach (GameObject obj in objectsToTurnOff)
                {
                    obj.SetActive(false);
                }

                foreach (GameObject obj in objectsToTurnOn)
                {
                    obj.SetActive(true);
                }
            }
            this.gameObject.SetActive(false);
            TwinToggle.gameObject.SetActive(true);
        }
    }
}
