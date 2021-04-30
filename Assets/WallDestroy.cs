using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroy : MonoBehaviour
{
    Animator m_animator;

    BigOrange orangeScript;
    GameObject orange;
    // Start is called before the first frame update
    void Start()
    {
        orange = GameObject.FindGameObjectWithTag("Boss");
        orangeScript = orange.GetComponent<BigOrange>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     if(orangeScript.HP <= 0 && orangeScript != null)
        {
            m_animator.SetFloat("Speed",1);
        }   
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
