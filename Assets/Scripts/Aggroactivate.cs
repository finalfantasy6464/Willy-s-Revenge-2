using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Aggroactivate : MonoBehaviour, IPausable
{
	public bool active = false;
	public Transform player;
	public float range;
	private AIDestinationSetter pathing;

    public bool isPaused { get; set; }

    public void OnPause()
    {
    }

    public void OnUnpause()
    {
    }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    {
        if (player)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= range)
            {
                active = true;
            }

            if (active == false)
            {
                pathing.enabled = false;
            }
            else if (active == true)
            {
                pathing.enabled = true;
            }
        }
    }

    void Start (){

		pathing = GetComponent<AIDestinationSetter> ();
	}

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }
}
