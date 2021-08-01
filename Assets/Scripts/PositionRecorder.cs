using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRecorder : MonoBehaviour
{
    public void PositionSave()
    {
        Character character = GameObject.Find("Character").GetComponent<Character>();
        GameControl.control.AutosavePosition = character.transform.position;
    }
}
