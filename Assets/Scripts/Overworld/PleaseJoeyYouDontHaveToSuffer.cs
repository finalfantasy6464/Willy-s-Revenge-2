using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PleaseJoeyYouDontHaveToSuffer : MonoBehaviour
{
    public GameObject overworldPinPrefab;
    void Start()
    {
        LevelPin[] array = GetComponentsInChildren<LevelPin>(true);
        for (int i = 0; i < array.Length; i++)
        {
            LevelPin pin = array[i];
            GameObject newPinObject = Instantiate(overworldPinPrefab, Vector3.zero, Quaternion.identity, null);
            if(newPinObject.TryGetComponent(out OverworldLevelPin newPin))
            {
                newPinObject.name = pin.gameObject.name;
                newPin.SetWorldData(i + 1,pin.parTime, pin.levelDisplayName, pin.levelPreviewSprite);
            }
        }
    }
}
