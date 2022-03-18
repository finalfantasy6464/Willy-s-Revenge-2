using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrangeEntrance : MonoBehaviour
{
    public BigOrange BO;
    public Animator m_animator;
    public Transform[] animatedTransforms;

    public void SpawnOrange()
    {
        BO.transform.position = new Vector3(-1.86f, -5.48f, -20f);
        BO.m_animator.enabled = true;
        BO.m_animator.SetFloat("EntranceSpeed", 1);
    }
    public void destroySelf()
    {
        Destroy(gameObject);
    }

}
