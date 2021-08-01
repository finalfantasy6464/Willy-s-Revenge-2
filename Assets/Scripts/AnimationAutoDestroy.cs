﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour
{
	public float delay = 0f;

	void Start(){
		Destroy (transform.gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo (0).length + delay);
	}
}
