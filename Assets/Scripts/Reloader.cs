using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloader : MonoBehaviour
{

	MovingShooter mover;
	public float offset;
	public float reloadtimer;
	public float speed = 1.0f;
	private float defaultfirerate;
	private float newfirerate = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
		mover = GetComponent<MovingShooter> ();
		defaultfirerate = mover.firerate;
		reloadtimer = offset;
    }

    // Update is called once per frame
    void Update()
	{
		reloadtimer += Time.deltaTime * speed;

		if (reloadtimer < 0.5f){
			mover.firerate = newfirerate;
		}
		if (reloadtimer > 0.5f) {
			mover.firerate = defaultfirerate;
		}
		if (reloadtimer > 2.5f) {
			reloadtimer = 0.0f;
		}
	}
}
