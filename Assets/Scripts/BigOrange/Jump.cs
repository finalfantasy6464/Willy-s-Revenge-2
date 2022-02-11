using System.Collections;
using UnityEngine;
namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewJump", menuName = "Scriptables/Jump")]
    public class Jump : BigOrangeMove
    {
        BigOrange bigOrange;

        const float hitProgress = 0.8333f;
        const string JUMP = "Jump";

        public override void Execute()
        {
            base.Execute();
        }

        public void Execute(BigOrange bigOrange)
        {
            this.bigOrange = bigOrange;

            base.Execute();
            bigOrange.m_animator.Play(JUMP, -1);
            bigOrange.StartCoroutine(WaitForJumpRoutine());
        }

        IEnumerator WaitForJumpRoutine()
        {
            while(!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName(JUMP))
                yield return null;
            
            while(bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < hitProgress)
                yield return null;

            bigOrange.enemySpawner.SpawnEnemiesTop();
        }
    }
}
