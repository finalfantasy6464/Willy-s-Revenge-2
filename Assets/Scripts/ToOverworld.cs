using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToOverworld : MonoBehaviour
{
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void toOverworld()
    {
        SceneManager.LoadScene("Overworld");
        GameControl.control.AutoLoadCheck();
    }
}
