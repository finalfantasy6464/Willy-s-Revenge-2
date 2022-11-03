using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PelletUIGlow : MonoBehaviour
{
    public GameObject tracker;

    private void Start()
    {
        Instantiate(tracker);
    }
}
