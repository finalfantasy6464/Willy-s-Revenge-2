using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlReset : MonoBehaviour
{ 
    void Update()
    {
        GameControl.control.complete = 0;
        GameControl.control.golden = 0;
        GameControl.control.timer = 0;

        GameControl.control.savedOverworldPlayerPosition = new Vector3(-7.354f, 1.695f, 0);
    }
}
