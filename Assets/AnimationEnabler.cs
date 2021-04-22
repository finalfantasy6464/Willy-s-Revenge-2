using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnabler : MonoBehaviour
{
	Animator m_Animator;

	void Start()
	{
		//Get the Animator attached to the GameObject you are intending to animate.
		m_Animator = gameObject.GetComponent<Animator>();
    }

	void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;

		if (hit.tag == "Jump")
		{
            m_Animator.Play("PlayerJump");
		}
	}
}
