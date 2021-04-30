using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossActivation : MonoBehaviour
{
           BigOrange orangescript;
           GameObject orange;

    public GameObject HPBarImage;
    public GameObject CurrentHPtext;
    public GameObject HPText;

    public Camera[] cameras;

    public PlayerController playerScript;

    private Collider2D m_collider;

    private void Start()
    {
        if (GameControl.control.bosscheckpoint == true)
        {
            playerScript.transform.position = transform.position;
        }

        orange = GameObject.FindGameObjectWithTag("Boss");
        orangescript = orange.GetComponent<BigOrange>();
        m_collider = GetComponent<Collider2D>();
    }

    public void BattleEnd()
    {
        {
            cameras[1].gameObject.SetActive(false);
            cameras[0].gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if(hit.tag == "Player")
        {
           GameControl.control.bosscheckpoint = true;

            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);

            HPBarImage.SetActive(true);
            HPText.SetActive(true);
            CurrentHPtext.SetActive(true);

            orangescript.m_animator.SetFloat("EntranceSpeed", 1);
            m_collider.enabled = false;

       

        }
    }
}
