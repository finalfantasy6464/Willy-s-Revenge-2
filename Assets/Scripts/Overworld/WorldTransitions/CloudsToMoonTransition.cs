using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class CloudsToMoonTransition : WorldTransition
{
    public Animator playerAnimator;
    public Animator cycloneAnimator;
    public Animator RocketAnimator;
    public Transform Rocket;
    public BezierNode rocketPath;
    
    protected override IEnumerator BackwardRoutine()
    {
        playerAnimator.enabled = true;
        RocketAnimator.enabled = true;

        playerAnimator.Play("Rocket", -1);
        RocketAnimator.Play("Rocket2Clouds", -1, 0f);
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node A
        playerAnimator.enabled = false;
        RocketAnimator.enabled = false;
        character.GetComponent<SpriteRenderer>().sprite = character.skinSprites[7];
        GameControl.control.currentCharacterSprite = 7;

        secondTraversed = false;
        character.isIgnoringPath = false;

        Vector3 target = character.currentPin.previousPath.end.position;
        while (Vector3.Distance(character.transform.position, target) > 0.01f)
        {
            Vector2 lookDirection = target - character.transform.position;
            character.transform.rotation = Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, character.moveSpeed * 0.1f);
            yield return null;
        }
        character.SetMovePin(character.currentPin.previousPath.end.GetComponent<NavigationPin>(), true);
        OnTransitionEnd();
    }

    protected override IEnumerator ForwardRoutine()
    {
        playerAnimator.enabled = true;

        playerAnimator.Play("Cyclone", -1);
        yield return null;
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node B
        playerAnimator.enabled = false;
        character.GetComponent<SpriteRenderer>().sprite = character.skinSprites[8];
        GameControl.control.currentCharacterSprite = 8;

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
