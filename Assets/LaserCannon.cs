
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserCannon : MonoBehaviour
{
    private float RaycastMax = 100f;

    private LayerMask layer;

    public bool usestimer = false;

    public float offset;
    public float firerate;
    public float firereset;
    public float shootLength;

    public LineRenderer beamRenderer;
    public ParticleSystem Endparticles;
    public ParticleSystem Startparticles;
    private ParticleSystemShapeType shapetype = ParticleSystemShapeType.Box;

    public AudioClip elec;

    Vector3 direction;
    Vector2 endpos;
    Vector2 midpos;
    Vector2 thickness;

    public Transform raycastPoint;

    RaycastHit2D hit;

    private void Start()
    {
        raycastPoint = this.transform;
        shootLength = offset;
        layer = LayerMask.GetMask("Walls", "Player");
    }

    private void Update()
    {
        if(usestimer == false)
        {
           RaycastDirection();
            UpdateParticles();
        }
       
        if (usestimer == true)
        {
            shootLength += Time.deltaTime;

            if (shootLength < firerate)
            {
                RaycastDirection();
                UpdateParticles();
            }

            if (shootLength > firerate)
            {
                RaycastStop();
            }
  
            if (shootLength >= firerate + firereset)
            { 
                shootLength = 0;
                beamRenderer.enabled = true;
            }
        }
    }

    private void RaycastDirection()
    {
        direction = -transform.up;

        hit = Physics2D.Raycast(raycastPoint.position, direction, RaycastMax, layer);

        Vector2 endpos = hit.point;

        if (hit.collider != null)
        {
            beamRenderer.SetPosition(0, raycastPoint.position);
            beamRenderer.SetPosition(1, endpos);
        }

        if (hit.collider.GetComponent<PlayerController>() != null)
        {
            Destroy(hit.collider.gameObject);
            DontDestroyme.instance.PlaySingle(elec);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void RaycastStop()
    {
        beamRenderer.enabled = false;
        Endparticles.Stop();
        Startparticles.Stop();
    }

    private void UpdateParticles()
    {
        Vector2 startpos;
        Vector2 endposition;

        if (hit.collider != null)
        {
         startpos = this.beamRenderer.GetPosition(0) + (-transform.up * 0.2f);
         endposition = this.beamRenderer.GetPosition(1);

            Startparticles.transform.position = new Vector3(startpos.x, startpos.y, -5.0f);
            Endparticles.transform.position = new Vector3 (endposition.x, endposition.y, -5.0f);
            Endparticles.Play();
            Startparticles.Play();
        }
    }
}
