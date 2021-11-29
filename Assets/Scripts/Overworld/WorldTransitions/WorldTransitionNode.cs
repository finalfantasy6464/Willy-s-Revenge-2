using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTransitionNode : MonoBehaviour
{
    WorldTransition transition;
    
    void Start()
    {
        transition = transform.parent.GetComponent<WorldTransition>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            transition.OnEnterNode(this);
        }
    }
}
