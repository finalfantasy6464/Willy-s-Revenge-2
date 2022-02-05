
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WillysRevenge2.BigOrangeMoves
{
    [CreateAssetMenu(fileName = "NewPunch", menuName = "Scriptables/Punch")]
    public class Punch : BigOrangeMove
    {
        public float moveSpeed;
        public float afterPunchSleepTime;
        public float handDistanceMultiplier;
        public float shoulderMaxDistance;
        public float shoulderSpeedMultiplier;
        public float armSpeedMultiplier;
        public float handSpeedMultiplier;
        
        [Header("Lightning")]
        public float lightningUpdateTime;
        [Range(0f, 1f)] public float xMagnitude;
        [Range(0f, 1f)] public float yMagnitude;
        
        Transform shoulder;
        Transform arm;
        Transform hand;

        Vector3 targetPosition;
        Vector3 shoulderStartPosition;
        Vector3 armStartPosition;
        Vector3 handStartPosition;

        BigOrange bigOrange;
        Animator orangeAnimator;
        bool isRight;

        const string LEFT_WINDUP = "LeftPunchWindup";
        const string RIGHT_WINDUP = "RightPunchWindup";

        public void Execute(PlayerController2021remake player, BigOrange bigOrange, string armString)
        {
            isRight = armString.Contains("Right");
            this.bigOrange = bigOrange;
            shoulder = isRight ? bigOrange.rightShoulder : bigOrange.leftShoulder;
            arm = isRight ? bigOrange.rightArm : bigOrange.leftArm;
            hand = isRight ? bigOrange.rightHand : bigOrange.leftHand;

            targetPosition = player.transform.position + (player.transform.position - handStartPosition).normalized * handDistanceMultiplier;

            base.Execute();
            bigOrange.m_animator.Play(isRight ? RIGHT_WINDUP : LEFT_WINDUP, -1);
            bigOrange.StartCoroutine(WindupWaitRoutine());
        }

        public IEnumerator WindupWaitRoutine()
        {
            while(!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName(LEFT_WINDUP)
                    &&!bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).IsName(RIGHT_WINDUP))
                yield return null;

            while(bigOrange.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                yield return null;

            shoulderStartPosition = shoulder.position;
            armStartPosition = arm.position;
            handStartPosition = hand.position;

            bigOrange.m_animator.enabled = false;
            bigOrange.StartCoroutine(PunchRoutine());
        }

        public IEnumerator PunchRoutine()
        {
            while (Vector3.Distance(hand.position, targetPosition) > 0.05f)
            {
                Vector3 shoulderTarget = shoulderStartPosition + (targetPosition - shoulderStartPosition) / 3f;
                shoulder.position = Vector3.MoveTowards(shoulder.position, shoulderTarget, moveSpeed * shoulderSpeedMultiplier * 0.1f);
                hand.position = Vector3.MoveTowards(hand.position, targetPosition, moveSpeed * handSpeedMultiplier * 0.1f);
                arm.position = MidPoint(shoulder.position, hand.position);
                yield return null;
            }

            float sleepCounter = 0f;
            float lightningUpdateCounter = 0f;
            bigOrange.lightning.gameObject.SetActive(true);
            bigOrange.lightningCollider.gameObject.SetActive(true);
            UpdateCollider();
            while(sleepCounter < afterPunchSleepTime)
            {
                sleepCounter += Time.deltaTime;
                lightningUpdateCounter += Time.deltaTime;
                if(lightningUpdateCounter > lightningUpdateTime)
                    UpdateLightning();

                yield return null;
            }
            ResetLightning();
            ResetCollider();
            bigOrange.lightning.gameObject.SetActive(false);
            bigOrange.lightningCollider.gameObject.SetActive(false);

            while (Vector3.Distance(hand.position, handStartPosition) > 0.05f)
            {
                shoulder.position = Vector3.MoveTowards(shoulder.position, shoulderStartPosition, moveSpeed * shoulderSpeedMultiplier * 0.1f);
                hand.position = Vector3.MoveTowards(hand.position, handStartPosition, moveSpeed * handSpeedMultiplier * 0.1f);
                arm.position = MidPoint(shoulder.position, hand.position);
                yield return null;
            }
            
            isDone = true;
            bigOrange.m_animator.enabled = true;
            bigOrange.m_animator.Play(isRight ? "RightPunchWinddown" : "LeftPunchWinddown", -1);
            EndCheck();
        }

        private void UpdateCollider()
        {
            Vector2[] anchorPositions = new Vector2[]
            {
                shoulder.localPosition,
                arm.localPosition,
                hand.localPosition,
            };

            bigOrange.lightningCollider.points = anchorPositions;
        }

        private void ResetCollider()
        {
            bigOrange.lightningCollider.points = new Vector2[] {Vector2.zero, Vector2.zero, Vector2.zero};
        }

        void ResetLightning()
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < bigOrange.lightning.positionCount; i++)
                positions.Add(Vector3.zero);
            bigOrange.lightning.SetPositions(positions.ToArray());
        }

        void UpdateLightning()
        {
            Vector3 shoulderArmMid = MidPoint(shoulder.position, arm.position);
            Vector3 armHandMid = MidPoint(arm.position, hand.position);

            Vector3[] anchorPositions = new Vector3[]
            {
                shoulder.position,
                MidPoint(shoulder.position, shoulderArmMid),
                shoulderArmMid,
                MidPoint(shoulderArmMid,arm.position),
                arm.position,
                MidPoint(arm.position, armHandMid),
                armHandMid,
                MidPoint(armHandMid, hand.position),
                hand.position
            };

            List<Vector3> positions = new List<Vector3>();
            int anchorIndex = 0;
            int positionCounter = 0;
            bool returning = false;

            while(positionCounter < bigOrange.lightning.positionCount)
            {
                Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-xMagnitude, xMagnitude),
                        UnityEngine.Random.Range(-yMagnitude, yMagnitude), 0f);

                positions.Add(anchorPositions[anchorIndex] + randomOffset);

                if(!returning)
                {
                    anchorIndex++;
                    if(anchorIndex == anchorPositions.Length - 1)
                        returning = true;
                } else
                {
                    anchorIndex--;
                    if(anchorIndex == 0)
                        returning = false;
                }
                positionCounter++;
            }

            bigOrange.lightning.SetPositions(positions.ToArray());
        }

        Vector3 MidPoint(Vector3 a, Vector3 b)
        {
            return a + (b - a) / 2f;
        }
    }
}
