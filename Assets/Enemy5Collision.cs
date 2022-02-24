using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Collision : MonoBehaviour
{
    SpriteRenderer m_sprite;
    Color defaultColor;
    Color changeColor = new Color(1, 1, 1, 0.4f);

    public void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        defaultColor = m_sprite.color;
    }
    private void OnTriggerStay2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if(hit.CompareTag("Enemy3") || hit.CompareTag("Bullet"))
        {
            m_sprite.color = changeColor;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if(hit.CompareTag("Enemy3") || hit.CompareTag("Bullet"))
        {
            m_sprite.color = defaultColor;
        }
    }
}
