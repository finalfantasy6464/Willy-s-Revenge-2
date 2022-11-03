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

    public event Action OnSelect;

    public Rigidbody myRigidbody;
    Vector3 rotateVector;
    Vector2 moveValue;

    void Start()
    {
        rotateVector.z = rotateSpeed;
    }
    
    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        moveVector = new Vector2(moveValue.x, moveValue.y);
        if(moveVector != Vector2.zero)
            lastLookRotation = Quaternion.LookRotation(moveVector);

        rotateVector = new Vector3(0f, 0f, moveValue.x);
    }

    void OnSelection(InputValue value)
    {
        OnSelect?.Invoke();
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotateVector * rotateSpeed * Time.fixedDeltaTime);
        
        myRigidbody.MovePosition((Vector2)myRigidbody.position + moveVector * moveSpeed * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);
        transform.rotation = Quaternion.Slerp (transform.rotation, lastLookRotation, Time.deltaTime * 20f);
    }
}
