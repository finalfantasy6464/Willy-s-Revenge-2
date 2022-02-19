using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloader : MonoBehaviour, IPausable
{

	MovingShooter mover;
	public float offset;
    public float idletime;
    public float ShootingTime = 1f;
	public float reloadtimer = 0f;
    public float ReloadingTime;

    public bool isPaused { get; set;}

    // Start is called before the first frame update
    void Start()
    {
		mover = GetComponent<MovingShooter> ();
		idletime = offset;
    }

    // Update is called once per frame
    void Update()
	{
        if (!isPaused)
            UnPausedUpdate();
	}

    public void OnPause()
    { }
    public void OnUnpause()
    { }

    public void UnPausedUpdate()
    {
        idletime += Time.deltaTime;
        if(idletime > ShootingTime)
        {
            idletime = -10000f;
            reloadtimer = 0;
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        while (reloadtimer < ReloadingTime)
        {
            reloadtimer += Time.deltaTime;
            mover.fireprogress = 0;
            yield return null;
        }

        idletime = 0;
        mover.fireprogress += Time.deltaTime;
        yield break;
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
