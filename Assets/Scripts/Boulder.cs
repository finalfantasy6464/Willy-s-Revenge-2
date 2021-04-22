using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boulder : MonoBehaviour
{
    BigOrange orangeScript;
    PlayerController playercontroller;
    GameObject orange;

    GameObject[] FloorSwitches;
    RadialActivate activate;

    int bouldertotal;

    void Start()
    {
        orange = GameObject.FindGameObjectWithTag("Boss");
        orangeScript = orange.GetComponent<BigOrange>();
        FloorSwitches = GameObject.FindGameObjectsWithTag("Switch");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.tag == "Boss")
        {
            foreach (GameObject floorswitch in FloorSwitches)
            {
                activate = floorswitch.GetComponent<RadialActivate>();
                bouldertotal += activate.boulderamount;
            }

            orangeScript.HP -= (int)(100 * Mathf.Pow(bouldertotal, 3));
            Debug.Log(orangeScript.HP);
            Destroy(gameObject);
        }
    }
}