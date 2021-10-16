using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class ArcticToDesertTransition : WorldTransition
{
    void Update()
    {
        
    }
    
    protected override IEnumerator BackwardRoutine()
    {
        yield return null;
        Debug.Log("doing routine stuff but backwards");
        //Always
        base.OnTransitionEnd();
    }

    protected override IEnumerator ForwardRoutine()
    {
        yield return null;
        Debug.Log("doing routine stuff");

        //Always
        base.OnTransitionEnd();
    }
}
