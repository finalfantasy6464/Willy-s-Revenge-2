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

    public GatePin[] gates;
    public LevelPin[] redirectedGates;

    List<Color> pairColors;
    List<int> pinIndexes;
    Dictionary<int, Color> pinDict;
    ActivationPair none;
    public IEnumerator pathBackgroundColorRoutine;

    void Start()
    {
        pairColors = new List<Color>();
        for (int i = 1; i < GetAll.Length; i++)
        {
            pairColors.Add(GetAll[i].skyboxColour);
        }

        pinIndexes = new List<int>()
        {
            0, 30, 70, 78, 80, 102
        };

        pinDict = new Dictionary<int, Color>()
        {
            { pinIndexes[0], pairColors[0]},
            { pinIndexes[1], pairColors[1]},
            { pinIndexes[2], pairColors[2]},
            { pinIndexes[3], pairColors[3]},
            { pinIndexes[4], pairColors[4]},
            { pinIndexes[5], pairColors[5]}
        };
    }

    public ActivationPair[] GetAll => new ActivationPair[]
    {
        none, worldLeft, worldRight, worldFull, clouds, moon, uFO
    };

    public void Set(OverworldProgressView view)
    {
        this.view = view;
        current = GetAll[(int)view];
        GetAll[(int)view]?.Trigger();
    }

    public IEnumerator PinToPinBackgroundColorRoutine(NavigationPin currentPin, OverworldCharacter character, bool isReturning, float moveTime)
    {
        // Can't go back
        if(isReturning)
        {
            pathBackgroundColorRoutine = null;
            yield break;
        }
        NavigationPin current = currentPin;

        if(currentPin is GatePin)
        {
            for (int i = 0; i < gates.Length; i++)
            {
                if(currentPin == gates[i])
                {
                    current = redirectedGates[i];
                    break;
                }
            }
        }

        int currentPinIndex = 0;

        for (int i = 0; i < map.levelPins.Count; i++)
        {
            if(current == map.levelPins[i])
            {
                currentPinIndex = i;
                break;
            }
        }

        Debug.Log(currentPinIndex);
        int gradientStartIndex = FindNearestLeft(currentPinIndex);
        int gradientEndIndex = Mathf.Min(102, FindNearestRight(currentPinIndex));
        int nextIndex = Mathf.Min(currentPinIndex + 1, 102);

        Color gradientStart = pinDict[gradientStartIndex];
        Color gradientEnd = pinDict[gradientEndIndex];

        Color currentColor = overworldCamera.gameCamera.backgroundColor;
        Color nextColor = Color.Lerp(gradientStart, gradientEnd,
                (float)(nextIndex - gradientStartIndex) / (float)(gradientEndIndex - gradientStartIndex));
        
        float counter = 0f;
        
        while (counter < moveTime)
        {
            counter += Time.deltaTime;
            
            overworldCamera.gameCamera.backgroundColor = Color.Lerp(currentColor, nextColor, counter / moveTime);
            yield return null;
        }

        pathBackgroundColorRoutine = null;
    }

    int FindNearestRight(int i)
    {
        while(i < pinIndexes[pinIndexes.Count - 1])
        {
            if(pinIndexes.Contains(i))
                return i;
            else
                i++;
        }
        return i;
    }

    int FindNearestLeft(int i)
    {
        while(i > 0)
        {
            if(pinIndexes.Contains(i))
                return i;
            else
                i--;
        }
        return i;
    }
    
    public void PlayPathBackgroundColorRoutine(OverworldCharacter character, bool isReturning, float moveTime)
    {
        pathBackgroundColorRoutine = PinToPinBackgroundColorRoutine(character.currentPin, character, isReturning, moveTime);
        StartCoroutine(pathBackgroundColorRoutine);
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
    }
}
