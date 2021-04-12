using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public int ID;

	public bool active = false;
    public bool isgateway = false;

	public bool completed = false;
	public bool golden = false;

	public Sprite greensprite;
	public Sprite goldsprite;

	public Sprite UIdefault;
	public Sprite UIcomplete;
	public Sprite UIgolden;
	public Sprite UItimer;

	Animator m_Animator;
    private Pin pin;
    private int completerequired;
    

	void Start(){

		m_Animator = this.GetComponent<Animator> ();
        pin = this.GetComponent<Pin>();
        completerequired = pin.completerequired;


		if (GameControl.control.completedlevels [ID] == true) {
			this.GetComponent<SpriteRenderer> ().sprite = greensprite;
			m_Animator.SetTrigger ("Green");
		}

		if (GameControl.control.goldenpellets [ID] == true) {
			this.GetComponent<SpriteRenderer> ().sprite = goldsprite;
			m_Animator.SetTrigger ("Gold");
		}
    }


    void Update()
    {
		if (Input.GetKey (KeyCode.Space) & active == true) {
			SceneManager.LoadScene (ID);
			GameControl.control.levelID = ID;
		}
    }

	void OnTriggerEnter2D(Collider2D coll){

		if (coll.gameObject.tag == "Player") {
			active = true;

			GameObject completeEmblemObject = GameObject.Find ("CompleteEmblem");
			completeEmblemObject.GetComponent<Image>().sprite = UIdefault;
			GameObject timeEmblemObject = GameObject.Find ("TimeEmblem");
			timeEmblemObject.GetComponent<Image>().sprite = UIdefault;
			GameObject goldenEmblemObject = GameObject.Find ("GoldenEmblem");
			goldenEmblemObject.GetComponent<Image>().sprite = UIdefault;

			if (GameControl.control.completedlevels [ID] == true) {
				completeEmblemObject.GetComponent<Image> ().sprite = UIcomplete;
			}

			if (GameControl.control.timerchallenge [ID] == true) {
				timeEmblemObject.GetComponent<Image> ().sprite = UItimer;
			}

			if (GameControl.control.goldenpellets [ID] == true) {
				goldenEmblemObject.GetComponent<Image> ().sprite = UIgolden;

            if (isgateway == true)
                {
                    UIdefault = null;
                    completeEmblemObject.GetComponent<Image>().sprite = null;
                    timeEmblemObject.GetComponent<Image>().sprite = null;
                    goldenEmblemObject.GetComponent<Image>().sprite = null;
                }
		}

	  }
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			active = false;
		}
	}
}
