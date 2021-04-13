using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 3f;
    public bool IsMoving { get; private set; }

    public Pin PreviousPin { get; private set; }
    public Pin CurrentPin { get; private set; }
    private Pin _targetPin;
    private Pin _LastPin;
    private MapManager _mapManager;

    public CanvasGroup canvas;


    public void Initialise(MapManager mapManager, Pin startPin)
    {
        _mapManager = mapManager;
        SetCurrentPin(startPin);
    }


    /// <summary>
    /// This runs once a frame
    /// </summary>
    private void Update()
    {
        if (_targetPin == null) return;

        // Get the characters current position and the targets position
        var currentPosition = transform.position;
        var targetPosition = _targetPin.transform.position;

        // If the character isn't that close to the target move closer
        if (Vector3.Distance(currentPosition, targetPosition) > .02f)
        {
            transform.position = Vector3.MoveTowards(
                currentPosition,
                targetPosition,
                Time.deltaTime * Speed
            );
        }
        else
        {
            if (_targetPin.IsAutomatic)
            {
                // Get a direction to keep moving in
                var pin = _targetPin.GetNextPin(CurrentPin);
                MoveToPin(pin);
            }
            else
            {
                if (_targetPin.IsEndOfWorld)
                {
                    Gatecheck();
                }
                else
                {
                    SetCurrentPin(_targetPin);
                }
            }
        }
    }

    
    /// <summary>
    /// Check the if the current pin has a reference to another in a direction
    /// If it does the move there
    /// </summary>
    /// <param name="direction"></param>
    public void TrySetDirection(Direction direction)
    {
        // Try get the next pin
        var pin = CurrentPin.GetPinInDirection(direction);
        
        // If there is a pin then move to it
        if (pin == null) return;
        MoveToPin(pin);
    }

    public void Gatecheck()
    {
        if (GameControl.control.complete >= _targetPin.completerequired && _targetPin.completerequired != 0)
        {
            PreviousPin = CurrentPin;
            // Get a direction to keep moving in
            var pin = _targetPin.GetNextPin(CurrentPin);
            MoveToPin(pin);
        }
        else
        {
            _targetPin = CurrentPin;
            MoveToPin(_targetPin);
        }
    }

    /// <summary>
    /// Move to a new pin
    /// </summary>
    /// <param name="pin"></param>
    private void MoveToPin(Pin pin)
    {
        _targetPin = pin;
        IsMoving = true;
    }

    
    /// <summary>
    /// Set the current pin
    /// </summary>
    /// <param name="pin"></param>
    public void SetCurrentPin(Pin pin)
    {
        CurrentPin = pin;
        _targetPin = null;
        transform.position = pin.transform.position;
        IsMoving = false;

        if (GameControl.control.levelID == 0)
        {
            PreviousPin = null;
        }

        // Tell the map manager that
        // the current pin has changed
     
        _mapManager.UpdateGui();
    }
}