using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewRightSlam", menuName = "Scriptables/RightSlam")]
    public class Slam : BigOrangeMove
    {
        bool isRight;
        BigOrange bigOrange;

        const string LEFT_SLAM = "LeftFistSlam";
        const string RIGHT_SLAM = "RightFistSlam";

        public override void Execute()
        {
            base.Execute();
        }
        
        public void Execute(BigOrange bigOrange, string armString)
        {
            isRight = armString.Contains("Right");
            this.bigOrange = bigOrange;

            base.Execute();
            bigOrange.m_animator.Play(isRight ? RIGHT_SLAM : LEFT_SLAM, -1);
/*
 * 
 *          -- Spawn Either EnemySetLeft or EnemySetRight depending on the outcome of isRight, at the correct time of the
 *          respective animations.
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
