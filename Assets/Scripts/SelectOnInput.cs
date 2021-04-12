using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{

	public GameObject selectedobject;
	public EventSystem eventsystem;

	private bool buttonselected;

    // Update is called once per frame
    void Update()
    {
        if (selectedobject != null) {
        if (Input.GetAxisRaw ("Vertical") != 0 && buttonselected == false) {
			eventsystem.SetSelectedGameObject (selectedobject);
			buttonselected = true;
		} }
    }

	private void OnDisable()
	{
		buttonselected = false;
	}
}
