using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycollide : MonoBehaviour
{
	public void OnCollisionEnter(Collision node){
		if(node.gameObject.tag == "Player")
		{
			Destroy(node.gameObject);
		}
	}
}