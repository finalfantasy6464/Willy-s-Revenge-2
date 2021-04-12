using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    Queue<Vector3> futurePositions;
    public Transform following;

    public float sleepDistance;
    public bool moving;

    public void Awake()
    {
        futurePositions = new Queue<Vector3>();
    }
    public void Update()
    {
        if (futurePositions.Count == 0 || moving == true) return;

        if (Vector3.Distance(transform.position, following.position) > sleepDistance)
        {
            moving = true;
        }
    }
    public void Move()
    {
        if (futurePositions.Count == 0 || moving == false) return;
        else
            transform.position = futurePositions.Dequeue();
    }
    public void AddUpcoming()
    {
        futurePositions.Enqueue(following.position);

    }
}