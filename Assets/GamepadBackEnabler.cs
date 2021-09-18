using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadBackEnabler : MonoBehaviour
{
    Button button;

    public int buttontype;

    public bool selected;
    public bool cancelled;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    void OnCancellation(InputAction action)
    {
        cancelled = action.enabled;
    }

    void OnSelection(InputAction action)
    {
        selected = action.enabled;
    }

    private void Update()
    {
    }
}
