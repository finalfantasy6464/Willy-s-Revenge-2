using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewCharge", menuName = "Scriptables/Charge")]
    public class Charge : BigOrangeMove
    {
        public void Execute(Animator orangeAnimator)
        {
            orangeAnimator.Play("Charge");
            base.Execute();
        }
    }
}
