using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public bool isMainMenu;

    public bool isCredits;

    public void Start()
    {
        if (isMainMenu)
        {
            GameObject.Find("SoundManager").GetComponent<MusicManagement>().onMainMenu.Invoke();
        }

        if (isCredits)
        {
            GameObject.Find("SoundManager").GetComponent<MusicManagement>().onCredits.Invoke();
        }
    }
}
