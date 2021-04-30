using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boulder : MonoBehaviour
{
    BigOrange orangeScript;
    PlayerController playercontroller;
    GameObject orange;
    SpriteRenderer sprite;

    GameObject[] FloorSwitches;
    RadialActivate[] activate;

    int bouldertotal;

    public List<bool> activated = new List<bool>();

    void Start()
    {
        orange = GameObject.FindGameObjectWithTag("Boss");
        orangeScript = orange.GetComponent<BigOrange>();
        FloorSwitches = GameObject.FindGameObjectsWithTag("Switch");
        activate = new RadialActivate[FloorSwitches.Length];
        sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < FloorSwitches.Length; i++)
        {
            activate[i] = FloorSwitches[i].GetComponent<RadialActivate>();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.tag == "Boss")
        {
            for (int l = 0; l < activate.Length; l++)
            {
                bouldertotal += activate[l].boulderamount;
                activated.Add(activate[l].isActive);

                if (!activated.Contains(true) && activated.Count == 4)
                {
                    for (int k = 0; k < activate.Length; k++)
                    {
                        activate[k].isActive = true;
                    }
                    activated.Clear();
                }
            }

            orangeScript.HP -= (int)(100 * Mathf.Pow(bouldertotal, 3));

            if (orangeScript.HP <= 0)
            {
                for (int l = 0; l < activate.Length; l++)
                {
                    Destroy(activate[l].gameObject);
                }
            }

            orangeScript.m_animator.Play("Damage");
            Destroy(gameObject);
        }
        }
    }