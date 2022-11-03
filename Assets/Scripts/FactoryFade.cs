using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryFade : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.33f);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
    }
}
