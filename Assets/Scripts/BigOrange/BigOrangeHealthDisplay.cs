using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BigOrangeHealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI amountLabel;
    public Slider slider;
    public BigOrange bigOrange;
    public Gradient healthGradient;
    public Image fill;

    void Update()
    {
        slider.maxValue = bigOrange.MaxHP;
        slider.minValue = 0;
        slider.value = bigOrange.HP;
        amountLabel.SetText($"{Mathf.Max(0, bigOrange.HP).ToString("N0")} / {bigOrange.MaxHP.ToString("N0")}");
        RecalculateBarColor();
    }

    void RecalculateBarColor()
    {
        fill.color = healthGradient.Evaluate(slider.value / slider.maxValue);
    }
}
