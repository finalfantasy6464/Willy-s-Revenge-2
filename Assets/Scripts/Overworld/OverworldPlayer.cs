using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldPlayer : MonoBehaviour
{
    public PlayerInput input;
    public OverworldLevelPin currentPin;
    public float moveSpeed;
    public float rotateSpeed;
    public Vector2 moveVector;
    public Quaternion lastLookRotation;

    public MapManager map;

    public event Action OnSelect;
    public event Action OpenMenu;
    public event Action CloseMenu;

    public Rigidbody myRigidbody;
    Vector3 rotateVector;
    Vector2 moveValue;

    public bool canMove = true;

    void Start()
    {
        rotateVector.z = rotateSpeed;
        
        if(GameControl.control.savedPinPosition != null)
            transform.position = GameControl.control.savedPinPosition;
    }

    void OnMove(InputValue value)
    {
        if (canMove)
        {
            moveValue = value.Get<Vector2>();
            moveVector = new Vector2(moveValue.x, moveValue.y);
            if (moveVector != Vector2.zero)
                lastLookRotation = Quaternion.Euler(0,0, -Mathf.Atan2(moveValue.x, moveValue.y) * Mathf.Rad2Deg);

            rotateVector = new Vector3(0f, 0f, moveValue.x);
        }
    }

    public void SetMovementState(bool value) 
    {
        canMove = value;
    }

    void OnSelection(InputValue value)
    {
        OnSelect?.Invoke();
    }

    void OnMenu(InputValue value)
    {
        OpenMenu?.Invoke();
    }

    void OnCancellation(InputValue value)
    {
        CloseMenu?.Invoke();
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotateVector * rotateSpeed * Time.fixedDeltaTime);

        if (canMove)
        {
            myRigidbody.MovePosition((Vector2)myRigidbody.position + moveVector * moveSpeed * Time.fixedDeltaTime);
            
            if(!RotationAproxEquals(transform.rotation, lastLookRotation))
            {
                myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, lastLookRotation, Time.fixedDeltaTime * 20f);
            }
        }
        else
        {
            moveVector = Vector2.zero;
        }
    }

    bool RotationAproxEquals(Quaternion a, Quaternion b, float tolerance = 0.0001f)
    {
        return (1f - Mathf.Abs(Quaternion.Dot(a, b))) < tolerance;
    }
}
