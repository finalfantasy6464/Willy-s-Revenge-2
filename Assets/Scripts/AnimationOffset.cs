using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{

	public float NewOffset;
	public float NewSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Animator>().SetFloat("Offset", NewOffset);
		GetComponent<Animator> ().SetFloat ("Speed", NewSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
