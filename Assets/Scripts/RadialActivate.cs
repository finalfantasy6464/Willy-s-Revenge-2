using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialActivate : MonoBehaviour, IPausable
{
    public RadialGauge radial;

    public Collider2D coll;
    public SpriteRenderer s_renderer;
    Sprite newsprite;
    Color newcolor;

    RadialProgress progress;

    public int boulderamount = 0;

    public bool justspawned = false;

    public bool isActive = true;

    public GameObject boulder;

    public Transform spawn;

    private float collenable;

    public bool isPaused { get; set; }

    private void Start()
    {
        radial.CurrentValue = 0;
        coll = GetComponent<Collider2D>();
        s_renderer = GetComponent<SpriteRenderer>();
        newcolor = s_renderer.color;
    }

    private void Update()
    {
        if (!isPaused)
            UnPausedUpdate();
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out PlayerController2021remake player))
        {
            radial.group.alpha = 1f;
            radial.isSteppedOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent(out PlayerController2021remake player))
        {
            radial.isSteppedOn = false;
            radial.group.alpha = 0f;
            radial.CurrentValue = 0f;
        }
    }

    public void TriggerBoulder()
    {
        if(justspawned == false)
        {
            isActive = false;
            GameObject newboulder = Instantiate(boulder);
            PauseControl.TryAddPausable(newboulder);
            PauseControl.TryAddPausable(newboulder.transform.GetChild(0).gameObject);
            if (newboulder.TryGetComponent(out Rigidbody2D boulderBody))
			{
				boulderBody.AddForce(Vector3.down * 80 );
				newboulder.GetComponent<Boulder>().SetForce(Vector3.down * 80);
			}

            boulder.transform.position = spawn.transform.position;
            boulder.transform.localScale = new Vector3(1.5f + (0.2f * boulderamount), 1.5f + (0.2f * boulderamount), 1);
            boulderamount += 1;
            coll.enabled = false;
            newcolor.a = 0.25f;
            s_renderer.color = newcolor;
            justspawned = true;
        }
    }


    public void OnPause()
    { }

    public void OnUnpause()
    { }

    public void UnPausedUpdate()
    {
        if (coll.enabled == false && coll != null && isActive == true)
        {
            collenable += Time.deltaTime;

            if (collenable >= 2.5f)
            {
                collenable = 0;
                coll.enabled = true;
                newcolor.a = 1;
                s_renderer.color = newcolor;
                justspawned = false;
            }
        }
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
