using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public void Start()
    {
        GameObject.Find("SoundManager").GetComponent<MusicManagement>().onMainMenu.Invoke();
    }
}
