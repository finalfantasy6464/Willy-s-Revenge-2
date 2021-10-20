using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// Add properties and use Animations or manual programming as necessary, 
///</Summary>
public class ArcticToDesertTransition : WorldTransition
{
    public ParticleSystem forwardTeleport1;
    public ParticleSystem forwardTeleport2;
    public ParticleSystem backwardTeleport1;
    public ParticleSystem backwardTeleport2;
    public Animator playerAnimator;
    
        
    protected override IEnumerator BackwardRoutine()
    {
        playerAnimator.enabled = true;
        
        playerAnimator.Play("TeleportSpinBackward", -1);
        yield return null;
        while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        character.transform.position = backwardTeleport2.transform.position;
        playerAnimator.GetComponent<SpriteRenderer>().color = Color.white;
        backwardTeleport1.Play();
        backwardTeleport2.Play();

        playerAnimator.enabled = true;
        playerAnimator.Play("TeleportSpinBackwardReverse", -1);
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
        playerAnimator.enabled = true;
        
        playerAnimator.Play("TeleportSpinForward", -1);
        yield return null;
        while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        character.transform.position = forwardTeleport2.transform.position;
        playerAnimator.GetComponent<SpriteRenderer>().color = Color.white;
        forwardTeleport1.Play();
        forwardTeleport2.Play();

        playerAnimator.enabled = true;
        playerAnimator.Play("TeleportSpinForwardReverse", -1);
        yield return null;
        while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // to simulate touching node B
        playerAnimator.enabled = false;
        secondTraversed = false;
        character.isIgnoringPath = false;

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
