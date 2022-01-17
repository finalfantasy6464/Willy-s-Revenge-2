using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldViewToggler : MonoBehaviour
{
    public OverworldProgressView view;
    public ActivationPair worldLeft;
    public ActivationPair worldRight;
    public ActivationPair worldFull;
    public ActivationPair clouds;
    public ActivationPair moon;
    public ActivationPair uFO;
    public ActivationPair current; // Don't change
    public OverworldCharacter character;
    public OverworldCamera overworldCamera;
    public MapManager map;

    List<Color> pairColors;
    List<int> pinIndexes;

    void Start()
    {

        pairColors = new List<Color>();
        for (int i = 1; i < GetAll.Length; i++)
        {
            pairColors.Add(GetAll[i].skyboxColour);
        }

        pinIndexes = new List<int>()
        {
            0, 30, 70, 80, 90
        };
    }

    ActivationPair none;

    public ActivationPair[] GetAll => new ActivationPair[]
    {
        none, worldLeft, worldRight, worldFull, clouds, moon, uFO
    };

    public void Set(OverworldProgressView view)
    {
        this.view = view;
        current = GetAll[(int)view];
        GetAll[(int)view]?.Trigger();
        GetAll[(int)view]?.SetSkyBoxColour();
    }

    public IEnumerator BackgroundColorRoutine(Animator playerAnimator)
    {
        float counter = 0f;
        float time = 1f;
        Color start = overworldCamera.gameCamera.backgroundColor;
        Color end = current.skyboxColour;
        while (counter < time)
        {
            overworldCamera.gameCamera.backgroundColor = Color.Lerp(
                    start, end, playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null;
        }
    }

    public IEnumerator PinToPinBackgroundColorRoutine(NavigationPin currentPin, float moveTime)
    {
        Color gradientColorStart = Color.clear;
        Color gradientColorEnd = Color.clear;

        bool leftSideFound = false;
        bool rightSideFound = false;

        int currentPinIndex = 0;

        for (int i = 0; i < map.levelPins.Count; i++)
        {
            if(currentPin == map.levelPins[i])
            {
                currentPinIndex = i;
                Debug.Log(currentPinIndex);
            }
        }
        
        for (int i = 0; i < map.levelPins.Count; i++)
        {
            if(i == currentPinIndex)
            {
                // find left side
                for (int j = currentPinIndex; j > 0; j--)
                {
                    for (int k = 0; k < pinIndexes.Count; k++)
                    {
                        if(currentPinIndex == pinIndexes[k])
                        {
                            leftSideFound = true;
                            gradientColorStart = pairColors[k];
                            Debug.Log(gradientColorStart);
                        }
                    }
                }    

                // find right side
                for (int j = currentPinIndex; j < map.levelPins.Count; j++)
                {
                    for (int k = 0; k < pinIndexes.Count; k++)
                    {
                        if(currentPinIndex == pinIndexes[k])
                        {
                            rightSideFound = true;
                            gradientColorEnd = pairColors[k];
                            Debug.Log(gradientColorEnd);
                        }
                    }
                }                
            }
        }

        float counter = 0f;

        while (counter < moveTime)
        {
            counter += Time.deltaTime;
            overworldCamera.gameCamera.backgroundColor = Color.Lerp(
                    gradientColorStart, gradientColorEnd, counter / moveTime);
            yield return null;
        }
    }

    [System.Serializable]
    public class ActivationPair
    {
        public OverworldCamera owCamera;
        public GameObject[] toActivate;
        public GameObject[] toDeactivate;
        public SpriteRenderer[] renderersToActivate;
        public SpriteRenderer[] renderersToDeactivate;
        public Color skyboxColour;

        public void Trigger()
        {
            foreach (GameObject go in toActivate)
                go.SetActive(true);

            foreach (GameObject go in toDeactivate)
                go.SetActive(false);

            foreach (SpriteRenderer sr in renderersToActivate)
                sr.enabled = true;

            foreach (SpriteRenderer sr in renderersToDeactivate)
                sr.enabled = false;
        }
        
        public void SetSkyBoxColour()
        {
            if (skyboxColour != Color.white)
                owCamera.gameCamera.backgroundColor = skyboxColour;
        }
    }
}
