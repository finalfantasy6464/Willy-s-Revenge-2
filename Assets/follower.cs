using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    public Transform target;
    public float speedWhenEaten;
    public float speedNow;
    public float sleepTime;
    public float sleepCounter;
    public bool moving;
    float segmentRadius;
    float distanceToTarget;
    Vector2 nextMoveDirection;
    Vector2 directionToTarget;
    Queue<Vector2> moveQueue;


    void Awake()
    {
        moveQueue = new Queue<Vector2>();
    }
    
    void Start()
    {
        segmentRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        if(moving) return;
        
        sleepCounter += Time.deltaTime;
        if(sleepCounter >= sleepTime)
            moving = true;
    }

    public void TryMoveNext(float currentMoveSpeed)
    {
        speedNow = currentMoveSpeed;
        if(!moving) return;
        transform.position = moveQueue.Dequeue(); 
        if(currentMoveSpeed > speedWhenEaten)
        {
            speedWhenEaten += 0.05f;
            transform.position = moveQueue.Dequeue(); 
        }
    }

    public void AddToQueue(Vector2 v)
    {
        moveQueue.Enqueue(v);
    }
}