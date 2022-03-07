﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fliparound : MonoBehaviour
{
	public Sprite sprite2;


	Collider2D m_Collider;

	public Sprite sprite1;

	void Start(){
		m_Collider = GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D(Collider2D coll){

			var hit = coll.gameObject;
			if (hit.tag == "Flipper") {

				m_Collider.enabled = false;
				this.GetComponent<SpriteRenderer> ().sprite = sprite2;
			    this.GetComponent<SpriteRenderer>().sortingOrder = -1;
			}
				
}
}
