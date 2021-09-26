using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject[] toDissapear;
    public GameObject[] toAppear;

    public Camera mainCamera;
    public Color changeColour;

    public OverworldCharacter character;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if(toDissapear != null)
            {
                for (int i = 0; i < toDissapear.Length; i++)
                {
                    toDissapear[i].SetActive(false);
                }
            }

            if(toAppear != null)
            {
                for (int i = 0; i < toAppear.Length; i++)
                {
                    toAppear[i].SetActive(true);
                }
            }
        }
        mainCamera.backgroundColor = changeColour;
    }
}
