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
    
    ActivationPair none;

    public ActivationPair[] GetAll => new ActivationPair[]
    {
        none, worldLeft, worldRight, worldFull, clouds, moon, uFO
    };

    public void Set(OverworldProgressView view)
    {
        this.view = view;
        GetAll[(int)view]?.Trigger();
    }

    [System.Serializable]
    public class ActivationPair
    {
        public GameObject[] toActivate;
        public GameObject[] toDeactivate;
        public SpriteRenderer[] renderersToActivate;
        public SpriteRenderer[] renderersToDeactivate;

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
