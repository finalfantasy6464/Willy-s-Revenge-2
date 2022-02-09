using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewHop", menuName = "Scriptables/Hop")]
    public class Hop : BigOrangeMove
    {
        public override void Execute()
        {
            base.Execute();
        }

        public void Execute(Animator orangeAnimator)
        {
            orangeAnimator.Play("QuadHop");
            base.Execute();
        }
    }
}
