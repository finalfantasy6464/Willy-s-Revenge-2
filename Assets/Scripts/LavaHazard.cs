using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaHazard : MonoBehaviour, IPausable
{
    public float currentTemperature;
    [Range(0f, 100f)]
    public float minTemperature;
    [Range(0f, 100f)]
    public float maxTemperature;
    [Range(0f, 100f)]
    public float killThreshold;
    public float minSleepTime;
    public float maxSleepTime;
    public float heatUpTime;
    public float coolDownTime;
    public Material lavaMaterial;
    
    public bool isPaused { get; set; }

    float counter;
    float sleepCounter;
    bool isHeatingUp;

    void Start()
    {
        isHeatingUp = true;
    }

    void Update()
    {
        if(!isPaused)
            UnPausedUpdate();
    }

    public void UnPausedUpdate()
    {
        if(isHeatingUp)
        {
            counter += Time.deltaTime;
            if (currentTemperature < maxTemperature)
            {
                currentTemperature = Mathf.Lerp(minTemperature, maxTemperature, counter / heatUpTime);
                lavaMaterial.SetFloat("_Kelvin", currentTemperature);
            }
            else if(sleepCounter < maxSleepTime)
                sleepCounter += Time.deltaTime;
            else if(sleepCounter >= maxSleepTime)
            {
                counter = 0f;
                sleepCounter = 0f;
                isHeatingUp = false;
            }
        }
        else
        {
            counter += Time.deltaTime;
            if (currentTemperature > minTemperature)
            {
                currentTemperature = Mathf.Lerp(maxTemperature, minTemperature, counter / coolDownTime);
                lavaMaterial.SetFloat("_Kelvin", currentTemperature);
            }
            else if(sleepCounter < minSleepTime)
                sleepCounter += Time.deltaTime;
            else if(sleepCounter >= minSleepTime)
            {
                counter = 0f;
                sleepCounter = 0f;
                isHeatingUp = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(currentTemperature >= killThreshold && other.TryGetComponent(out PlayerCollision player))
        {
            player.Die(player.onLavaBurn);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnDestroy() {}        

    public void OnPause() {}
    
    public void OnUnpause() {}
}
