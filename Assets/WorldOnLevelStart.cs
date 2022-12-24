using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOnLevelStart : MonoBehaviour
{
    public Animator m_Animator;

    private void Start()
    {
        StartCoroutine(waitforplayermove());
    }

    IEnumerator waitforplayermove()
    {
        yield return null;
        m_Animator.enabled = true;
        m_Animator.Play("WorldStandBy");
    }
    void StartCompleted()
    {
        m_Animator.enabled = false;
    }
}
