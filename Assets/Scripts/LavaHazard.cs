using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using System;

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
    public SpriteShapeRenderer shapeRenderer;
    public Material lavaMaterialBase;
    public Material materialInstance;
    
    public bool isPaused { get; set; }

    bool isHeatingUp;
    float temperatureCache;
    float counter;
    float sleepCounter;

    List<GameObject> hazardIndicators;
    
    void OnDisable()
    {
        if(materialInstance != null)    
            Destroy(materialInstance);
    }

    void Start()
    {
        InitializeHazardIndicators();
        materialInstance = Instantiate<Material>(lavaMaterialBase);
        materialInstance.name = "_DynamicLavaMaterial";
        shapeRenderer.materials = new Material[] { materialInstance, shapeRenderer.materials[1] };
        isHeatingUp = true;
    }

    private void InitializeHazardIndicators()
    {
        hazardIndicators = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteShapeRenderer>())
            {
                hazardIndicators.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(!isPaused)
            UnPausedUpdate();
    }

    public void UnPausedUpdate()
    {
        TemperatureUpdate();
        HazardIndicatorUpdate();
    }

    void HazardIndicatorUpdate()
    {
        if(hazardIndicators.Count == 0)
            return;

        if(!hazardIndicators[0].activeInHierarchy
                && temperatureCache < currentTemperature
                && currentTemperature >= killThreshold)
        {
            foreach (GameObject indicator in hazardIndicators)
                indicator.SetActive(true);
        }
        else if(hazardIndicators[0].activeInHierarchy
                && temperatureCache > currentTemperature
                && currentTemperature < killThreshold)
        {
            foreach (GameObject indicator in hazardIndicators)
                indicator.SetActive(false);
        }
    }

    void TemperatureUpdate()
    {
        temperatureCache = currentTemperature;
        if(isHeatingUp)
        {
            counter += Time.deltaTime;
            if (currentTemperature < maxTemperature)
            {
                currentTemperature = Mathf.Lerp(minTemperature, maxTemperature, counter / heatUpTime);
                materialInstance.SetFloat("_Kelvin", currentTemperature);
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
                materialInstance.SetFloat("_Kelvin", currentTemperature);
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
