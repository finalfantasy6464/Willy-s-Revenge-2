using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class DesertToVolcanoTransition : WorldTransition
{
    public Transform quicksandForward;
    public Transform quicksandBackward;
    public Transform orbitParentForward;
    public Transform orbitParentBackward;
    public Animator orbitForwardAnimator;
    public Animator orbitBackwardAnimator;
    public Animator playerAnimator;

    protected override IEnumerator BackwardRoutine()
    {
        playerAnimator.enabled = true;
        
        playerAnimator.Play("QuicksandBackward", -1);
        yield return null;
        while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node A
        playerAnimator.enabled = false;
        secondTraversed = false;
        character.isIgnoringPath = false;

        Vector3 target = character.currentPin.previousPath.end.position;
        while(Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation =  Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.1f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.previousPath.end.GetComponent<NavigationPin>(), true);
        OnTransitionEnd();
    }

    protected override IEnumerator ForwardRoutine()
    {
        Transform parentCache = character.transform.parent;
        playerAnimator.enabled = true;
        orbitForwardAnimator.enabled = true;
        character.transform.SetParent(orbitParentForward);
        
        playerAnimator.Play("QuicksandForward", -1);
        orbitForwardAnimator.Play("OrbitForward", -1);
        yield return null;
        while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // to simulate touching node B
        orbitForwardAnimator.enabled = false;
        playerAnimator.enabled = false;
        secondTraversed = false;
        character.isIgnoringPath = false;
        character.transform.SetParent(parentCache);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, 0f);
        Vector3 target = character.currentPin.nextPath.end.position;
        
        while(Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation =  Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.1f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.nextPath.end.GetComponent<NavigationPin>(), false);
        OnTransitionEnd();
    }
}
