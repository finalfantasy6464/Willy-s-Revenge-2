using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredDoor : MonoBehaviour
{
	public bool Red;
	public bool Green;
	public int Colour = 1;
	// Red is 1, Blue is 2, Green is 3, Yellow is 4

	Collider2D m_Collider;
	SpriteRenderer sprite;

	void Start()
	{
		//Fetch the GameObject's Collider (make sure it has a Collider component)
		m_Collider = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer> ();
	}

    // Update is called once per frame
    void Update()
    {
		switch (Colour) {

		case 4:
			if (Green == true) {
				m_Collider.enabled = false;
				sprite.enabled = false;
			} else if (Green == false) {
				m_Collider.enabled = true;
				sprite.enabled = true;
			}
			break;
		case 3:
			if (Green == true) {
				m_Collider.enabled = true;
				sprite.enabled = true;
			} else if (Green == false) {
				m_Collider.enabled = false;
				sprite.enabled = false;
			}
			break;
		case 2:
			if (Red == true) {
				m_Collider.enabled = false;
				sprite.enabled = false;
			} else if (Red == false) {
				m_Collider.enabled = true;
				sprite.enabled = true;
			}
			break;

		case 1:
			if (Red == true) {
				m_Collider.enabled = true;
				sprite.enabled = true;
			} else if (Red == false) {
				m_Collider.enabled = false;
				sprite.enabled = false;
			}
			break;
		}
}
}
