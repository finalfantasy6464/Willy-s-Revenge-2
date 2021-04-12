using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	private static CheckpointManager instance;
	public Vector2 lastCheckPointPos;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (instance);
		} else {
			Destroy (gameObject);
		}
	}
}
