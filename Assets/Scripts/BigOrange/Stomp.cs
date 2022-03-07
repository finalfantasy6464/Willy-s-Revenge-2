using System.Collections;
using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewStomp", menuName = "Scriptables/Stomp")]
    public class Stomp : BigOrangeMove
    {
        BigOrange bigOrange;
        const float hitProgress = 0.5f;
        const string STOMP = "Stomp";

        public override void Execute()
        {
            base.Execute();
        }

        public void Execute(BigOrange bigOrange)
        {
            this.bigOrange = bigOrange;

            base.Execute();
            bigOrange.m_animator.Play(STOMP, -1);
            bigOrange.StartCoroutine(WaitForStompRoutine());
        }

        IEnumerator WaitForStompRoutine()
        {
            while(!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName(STOMP))
                yield return null;
            
            while(bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < hitProgress)
                yield return null;

            bigOrange.switchControl.SpawnBlocks(bigOrange.speeds[bigOrange.stompspeedindex]);
        }
    }
}
