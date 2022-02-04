
using System.Collections;
using UnityEngine;
namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewPunch", menuName = "Scriptables/Punch")]
    public class Punch : BigOrangeMove
    {
        public float moveSpeed;
        public float afterPunchSleepTime;
        public float shoulderMaxDistance;
        public float shoulderSpeedMultiplier;
        public float armSpeedMultiplier;
        public float handSpeedMultiplier;
        
        Transform shoulder;
        Transform arm;
        Transform hand;

        Vector3 targetPosition;
        Vector3 shoulderStartPosition;
        Vector3 armStartPosition;
        Vector3 handStartPosition;

        BigOrange bigOrange;
        Animator orangeAnimator;

        public void Execute(PlayerController2021remake player, BigOrange bigOrange, string armString)
        {
            this.bigOrange = bigOrange;
            shoulder = armString.Contains("Right") ? bigOrange.rightShoulder : bigOrange.leftShoulder;
            arm = armString.Contains("Right") ? bigOrange.rightArm : bigOrange.leftArm;
            hand = armString.Contains("Right") ? bigOrange.rightHand : bigOrange.leftHand;

            targetPosition = player.transform.position;

            base.Execute();
            bigOrange.StartCoroutine(WindupWaitRoutine());
        }

        public IEnumerator WindupWaitRoutine()
        {
            while(!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName("LeftPunchWindup")
                    &&!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName("RightPunchWindup"))
                yield return null;

            while(bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                yield return null;

            shoulderStartPosition = shoulder.position;
            armStartPosition = arm.position;
            handStartPosition = hand.position;

            bigOrange.StartCoroutine(PunchRoutine());
        }

        public IEnumerator PunchRoutine()
        {
            while (Vector3.Distance(hand.position, targetPosition) > 0.05f)
            {
                Vector3 shoulderTarget = (targetPosition - shoulderStartPosition).normalized * shoulderMaxDistance;
                shoulder.position = Vector3.MoveTowards(shoulder.position, shoulderTarget, moveSpeed * shoulderSpeedMultiplier * 0.01f);
                hand.position = Vector3.MoveTowards(hand.position, targetPosition, moveSpeed * handSpeedMultiplier * 0.01f);
                Vector3 midpoint = shoulder.position + (hand.position - shoulder.position) / 2f;
                arm.position = shoulder.position + (hand.position - shoulder.position) / 2f;
                //arm.position = Vector3.MoveTowards(arm.position, midpoint, moveSpeed * armSpeedMultiplier * 0.01f);
                yield return null;
            }

            yield return new WaitForSeconds(afterPunchSleepTime);

            while (Vector3.Distance(hand.position, handStartPosition) > 0.05f)
            {
                shoulder.position = Vector3.MoveTowards(shoulder.position, shoulderStartPosition, moveSpeed * shoulderSpeedMultiplier * 0.01f);
                hand.position = Vector3.MoveTowards(hand.position, handStartPosition, moveSpeed * handSpeedMultiplier * 0.01f);
                arm.position = shoulder.position + (hand.position - shoulder.position) / 2f;
                //arm.position = Vector3.MoveTowards(arm.position, armStartPosition, moveSpeed * armSpeedMultiplier * 0.01f);
                yield return null;
            }
            
            isDone = true;
            EndCheck();
        }
    }
}
