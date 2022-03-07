using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class ForestToArcticTransition : WorldTransition
{
    public float ferryTime;
    public Transform iceberg;
    public Transform pointA;
    public Transform pointB;

    void Update()
    {
        
    }
    
    protected override IEnumerator BackwardRoutine()
    {
        float counter = 0f;
        character.isIgnoringPath = true;
        characterCollider.enabled = false;
        while(counter < ferryTime)
        {
            counter += Time.deltaTime;
            Vector3 next = Vector3.Lerp(pointB.position, pointA.position, counter / ferryTime);
            iceberg.position = next;
            character.transform.position = new Vector3(next.x, next.y, character.transform.position.z);
            yield return null;
        }

        // to simulate touching node A
        secondTraversed = false;
        character.isIgnoringPath = false;
        character.SetCurrentSkin(0);

        Vector3 target = character.currentPin.previousPath.start.position;
        while(Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation =  Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.075f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.previousPath.start.GetComponent<NavigationPin>(), true);
        OnTransitionEnd();
    }

    protected override IEnumerator ForwardRoutine()
    {
        float counter = 0f;
        character.isIgnoringPath = true;
        characterCollider.enabled = false;
        while(counter < ferryTime)
        {
            counter += Time.deltaTime;
            Vector3 next = Vector3.Lerp(pointA.position, pointB.position, counter / ferryTime);
            iceberg.position = next;
            character.transform.position = new Vector3(next.x, next.y, character.transform.position.z);
            yield return null;
        }

        // to simulate touching node B
        secondTraversed = false;
        character.isIgnoringPath = false;
        character.SetCurrentSkin(1);

        Vector3 target = character.currentPin.nextPath.end.position;
        while(Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation =  Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.075f);
            yield return null;
        }        
        character.SetMovePin(character.currentPin.nextPath.end.GetComponent<NavigationPin>(), false);
        OnTransitionEnd();
    }
}
