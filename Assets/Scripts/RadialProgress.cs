using System;
using UnityEngine;
using UnityEngine.Events;

public class RadialProgress : RadialGauge, IPausable
{
    // Event to invoke when the progress bar fills up
    private UnityEvent onProgressComplete;
    RadialActivate activation;

    public bool activated = false;
    public bool isPaused { get; set; }

    // Create a property to handle the slider's value
    public new float CurrentValue
    {
        get
        {
            return base.CurrentValue;
        }
        set
        {
            ProgressCompleteCheck(value);
        }
    }

    private void ProgressCompleteCheck(float value)
    {
        // If the value exceeds the max fill, invoke the completion function
        if (value >= maxValue)
            onProgressComplete.Invoke();

        // Remove any overfill (i.e. 105% fill -> 5% fill)
        base.CurrentValue = value % maxValue;
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        activation = transform.parent.parent.gameObject.GetComponent<RadialActivate>();

        // Initialize onProgressComplete and set a basic callback
        if (onProgressComplete == null)
            onProgressComplete = new UnityEvent();
        onProgressComplete.AddListener(OnProgressComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if(isSteppedOn && !isPaused)
            UnPausedUpdate();
    }


    // The method to call when the progress bar fills up
    void OnProgressComplete()
    {
        activation.TriggerBoulder();
    }

    public void OnDestroy()
    { }

    public void OnPause()
    { }

    public void OnUnpause()
    { }
    
    public void UnPausedUpdate()
    {
        CurrentValue += 0.015f;
    }

    public void ForceInstant()
    {
        CurrentValue = maxValue;
    }
}
