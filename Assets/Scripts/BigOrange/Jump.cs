using UnityEngine;
namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewJump", menuName = "Scriptables/Jump")]
    public class Jump : BigOrangeMove
    {
        public override void Execute()
        {
            base.Execute();
        }

        public void Execute(Animator orangeAnimator)
        {
            orangeAnimator.Play("Jump");
            base.Execute();

            /*
             * -- Spawn EnemyTop at correct time based on animation
             * 
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

        }
    }
}
