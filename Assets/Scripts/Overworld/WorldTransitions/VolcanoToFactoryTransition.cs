using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class VolcanoToFactoryTransition : WorldTransition
{
    public Transform conveyor;
    public Animator conveyorAnimator;
    public Animator playerAnimator;

    protected override IEnumerator BackwardRoutine()
    {
        playerAnimator.enabled = true;
        conveyorAnimator.enabled = true;

        playerAnimator.Play("ConveyorBackward", -1);
        conveyorAnimator.SetFloat("Speed", -3.0f);
        conveyorAnimator.Play("ConveyorAnim", -1, 0f);
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node A
        playerAnimator.enabled = false;
        conveyorAnimator.enabled = false;

        secondTraversed = false;
        character.isIgnoringPath = false;

        Vector3 target = character.currentPin.previousPath.start.position;
        while (Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation = Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.1f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.previousPath.start.GetComponent<NavigationPin>(), true);
        OnTransitionEnd();
    }

    protected override IEnumerator ForwardRoutine()
    {
        playerAnimator.enabled = true;
        conveyorAnimator.enabled = true;

        playerAnimator.Play("ConveyorForward", -1);
        conveyorAnimator.SetFloat("Speed", 3.0f);
        conveyorAnimator.Play("ConveyorAnim", -1, 0f);
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node B
        playerAnimator.enabled = false;
        conveyorAnimator.enabled = false;

        secondTraversed = false;
        character.isIgnoringPath = false;

        Vector3 target = character.currentPin.nextPath.end.position;
        while (Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation = Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.1f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.nextPath.end.GetComponent<NavigationPin>(), false);
        OnTransitionEnd();
    }
}
