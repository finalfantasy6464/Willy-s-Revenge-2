using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public int cameraindex;

    void OnTriggerEnter2D(Collider2D coll)
    {
        GameControl.control.SetCamera(cameraindex);
    }
}
