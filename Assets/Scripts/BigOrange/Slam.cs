using System;
using System.Collections;
using UnityEngine;

namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewRightSlam", menuName = "Scriptables/RightSlam")]
    public class Slam : BigOrangeMove
    {
        bool isRight;
        string stateName;
        BigOrange bigOrange;
        
        const float hitProgress = 0.666f;
        const string LEFT_SLAM = "LeftFistSlam";
        const string RIGHT_SLAM = "RightFistSlam";

        public override void Execute()
        {
            base.Execute();
        }
        
        public void Execute(BigOrange bigOrange, string direction)
        {
            this.bigOrange = bigOrange;
            isRight = direction.Contains("Right");
            stateName = isRight ? RIGHT_SLAM : LEFT_SLAM;

            base.Execute();
            bigOrange.m_animator.Play(stateName, -1);
            bigOrange.StartCoroutine(WaitForSlamRoutine());
        }

        IEnumerator WaitForSlamRoutine()
        {
            while(!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                yield return null;
            
            while(bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < hitProgress)
                yield return null;

            if(isRight)
                bigOrange.enemySpawner.SpawnEnemiesRight();
            else
                bigOrange.enemySpawner.SpawnEnemiesLeft();
        }
    }
}
