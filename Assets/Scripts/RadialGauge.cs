using UnityEngine;
using UnityEngine.UI;

public class RadialGauge : MonoBehaviour
{
    // Public UI References
    public Image fillImage;
    public CanvasGroup group;

    public bool isSteppedOn;

    // Trackers for min/max values
    protected float maxValue = 2f, minValue = 0f;

    // Create a property to handle the slider's value
    private float currentValue = 0f;
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // Ensure the passed value falls within min/max range
            currentValue = Mathf.Clamp(value, minValue, maxValue);

            // Calculate the current fill percentage and display it
            float fillPercentage = currentValue / maxValue;
            fillImage.fillAmount = fillPercentage;
            fillImage.color = Color.Lerp(Color.red, Color.green, fillPercentage);
        }
    }

    public virtual void Start()
    {
        CurrentValue = 0f;
    }

}
