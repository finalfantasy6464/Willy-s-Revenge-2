using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIWindow : GUIElement
{
    public Selectable firstSelected;
    public event Action<GUIWindow> OnOpen;
    public event Action<GUIWindow> OnClose;

    public override void Show()
    {
        firstSelected?.Select();
        base.Show();
    }
    
    public override void Show(float time)
    {
        firstSelected?.Select();
        base.Show(time);
    }

    public virtual void Open()
    {
        Show();
        OnOpen?.Invoke(this);
    }

    public virtual void Close()
    {
        Hide();
        OnClose?.Invoke(this);
    }
}
