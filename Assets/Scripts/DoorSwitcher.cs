using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorSwitcher : MonoBehaviour
{

    public UnityEngine.Rendering.Universal.Light2D switchlight;

	public int type = 1;

	GameObject[] Colourdoor;

	private ColouredDoor[] door;

    private bool justchanged;

    public Sprite initialsprite;

    public Color initialColor;

    public Sprite[] colours;

    public SpriteRenderer m_spriterenderer;

    public AudioClip switchhit;

    // Start is called before the first frame update
    void Start()
    {
		Colourdoor = GameObject.FindGameObjectsWithTag ("Colour");
		door = new ColouredDoor[Colourdoor.Length];
        m_spriterenderer.sprite = initialsprite;
        switchlight.color = initialColor;
        justchanged = false;
}

	void OnTriggerEnter2D(Collider2D coll)
    {
		var hit = coll.gameObject;

		if (hit.tag == "Player" & type == 1) {

            ChangeSprite();
            GameSoundManagement.instance.PlaySingle(switchhit);

            for (int i = 0; i < Colourdoor.Length; i++) {
				door [i] = Colourdoor [i].GetComponent<ColouredDoor> ();
				door [i].Red = !door [i].Red;
			}
		}

		if (hit.tag == "Player" & type == 2) {

           ChangeSprite();
           GameSoundManagement.instance.PlaySingle(switchhit);

            for (int i = 0; i < Colourdoor.Length; i++) {
				door [i] = Colourdoor [i].GetComponent<ColouredDoor> ();
				door [i].Green = !door [i].Green;
			}
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            justchanged = false ;
        }
    }

    void ChangeSprite()
    {
        if (m_spriterenderer.sprite == colours[0] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[1];
            switchlight.color = new Color(0, 0, 0.5f);
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[1] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[0];
            switchlight.color = new Color(0.5f, 0, 0);
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[2] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[3];
            switchlight.color = new Color(0, 0.5f, 0);
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[3] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[2];
            switchlight.color = new Color(0.5f, 0.5f,0);
            justchanged = true;
        }
    }
}
