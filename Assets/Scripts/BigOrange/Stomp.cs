using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewStomp", menuName = "Scriptables/Stomp")]
    public class Stomp : BigOrangeMove
    {
        public override void Execute()
        {
            base.Execute();
        }

        public void Execute(Animator orangeAnimator)
        {
            orangeAnimator.Play("Stomp");

            /*
             -- Spawn Blocks at appropriate time during animation, based on how many successive times stomp is used.
            -- 1st time - 2 seconds
            -- 2nd time - 1 second
            -- 3rd time - 0.5 seconds
            -- When move that is *not* stomp occurs, reset to 1st time.


        AnimationClip clip;
        Animator anim;

        // new event created
        AnimationEvent evt;
        evt = new AnimationEvent();

        // put some parameters on the AnimationEvent
        //  - call the function called PrintEvent()
        //  - the animation on this object lasts 2 seconds
        //    and the new animation created here is
        //    set up to happen 1.3s into the animation
        evt.intParameter = 12345;
        evt.time = 1.3f;
        evt.functionName = "PrintEvent";

        // get the animation clip and add the AnimationEvent
        anim = GetComponent<Animator>();
        clip = anim.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evt);*/


            base.Execute();
        }
    }
}
