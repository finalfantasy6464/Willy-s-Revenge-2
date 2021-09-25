using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 3f;
    public bool IsMoving { get; private set; }

    public LevelPin PreviousPin { get; private set; }
    public LevelPin CurrentPin { get; private set; }
    private LevelPin _targetPin;
    private LevelPin _LastPin;
    private MapManager _mapManager;

    public CanvasGroup canvas;

    public void Initialise(MapManager map)
    {
        _mapManager = map;
        SetCurrentPin(map.startPin);
    }

    private void Awake()
    {
        if(Time.timeScale == 0)
            Time.timeScale = 1;
    }

    private void Start()
    {
        SetPinPosition();
    }

    public void SetPinPosition()
    {
        transform.position = GameControl.control.savedPinPosition;

        foreach (LevelPin pin in _mapManager.levelPins)
        {
            if (pin.transform.position == GameControl.control.savedPinPosition)
                SetCurrentPin(pin);
        }
    }
    /// <summary>
    /// Move to a new pin
    /// </summary>
    /// <param name="pin"></param>
    private void MoveToPin(LevelPin pin)
    {
        _targetPin = pin;
        IsMoving = true;
    }

    /// <summary>
    /// Set the current pin
    /// </summary>
    /// <param name="pin"></param>
    public void SetCurrentPin(LevelPin pin)
    {
        CurrentPin = pin;
        _targetPin = null;
        transform.position = pin.transform.position;
        IsMoving = false;

        if (GameControl.control.levelID == 0)
            PreviousPin = null;
    }
}