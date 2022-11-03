using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrangeEntrance : MonoBehaviour
{
    public BigOrange BO;
    public Animator m_animator;
    public Transform[] animatedTransforms;

    void Start()
    {
        //Time.timeScale = 0.1f;
    }

    public void SpawnOrange()
    {
        BO.m_animator.enabled = true;
        BO.m_animator.SetFloat("EntranceSpeed", 1);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
