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
    public SpriteRenderer spriteRenderer;

    public MapManager map;

    public Sprite[] overworldSprites;

    public event Action OnSelect;
    public event Action OpenMenu;
    public event Action CloseMenu;

    public Rigidbody myRigidbody;
    Vector3 rotateVector;
    Vector2 moveValue;
    float baseMoveSpeed;

    public bool canMove = true;

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        rotateVector.z = rotateSpeed;

        lastLookRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = lastLookRotation;
        
        if(GameControl.control.savedOverworldPlayerPosition != null)
            transform.position = GameControl.control.savedOverworldPlayerPosition;

        UpdateCharacterSprite();
    }

    public void UpdateCharacterSprite()
    {
        spriteRenderer.sprite = overworldSprites[GameControl.control.currentCharacterSprite];
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

    void OnModifier(InputValue value)
    {
        moveSpeed = baseMoveSpeed * (value.Get<float>() > 0f ? 1.5f : 1f);
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

    public void OnDisable()
    {
        GetComponent<PlayerInput>().actions = null;
    }
}
