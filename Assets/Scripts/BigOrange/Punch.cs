
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

        bool isRight;

        Vector3 targetPosition;
        Vector3 shoulderStartPosition;
        Vector3 armStartPosition;
        Vector3 handStartPosition;

        BigOrange bigOrange;
        Animator orangeAnimator;

        EdgeCollider2D edge;

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
            while (Vector2.Distance(hand.position, targetPosition) > 0.05f)
            {
                Vector2 shoulderTarget = shoulderStartPosition + (targetPosition - shoulderStartPosition) / 3f;
                shoulder.position = Vector3.MoveTowards(shoulder.position, shoulderTarget, moveSpeed * shoulderSpeedMultiplier * 0.1f);
                hand.position = Vector3.MoveTowards(hand.position, targetPosition, moveSpeed * handSpeedMultiplier * 0.1f);
                arm.position = MidPoint(shoulder.position, hand.position);
                yield return null;
            }

            float sleepCounter = 0f;
            float lightningUpdateCounter = 0f;
            bigOrange.lightning.gameObject.SetActive(true);
            while(sleepCounter < afterPunchSleepTime)
            {
                sleepCounter += Time.deltaTime;
                lightningUpdateCounter += Time.deltaTime;
                if(lightningUpdateCounter > lightningUpdateTime)
                    RedrawLightning();

                yield return null;
            }
            ResetLightning();
            bigOrange.lightning.gameObject.SetActive(false);

            while (Vector2.Distance(hand.position, handStartPosition) > 0.05f)
            {
                shoulder.position = Vector2.MoveTowards(shoulder.position, shoulderStartPosition, moveSpeed * shoulderSpeedMultiplier * 0.1f);
                hand.position = Vector2.MoveTowards(hand.position, handStartPosition, moveSpeed * handSpeedMultiplier * 0.1f);
                arm.position = MidPoint(shoulder.position, hand.position);
                yield return null;
            }
            
            isDone = true;
            bigOrange.m_animator.enabled = true;
            bigOrange.m_animator.Play(isRight ? "RightPunchWinddown" : "LeftPunchWinddown", -1);
            EndCheck();
        }

        void ResetLightning()
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < bigOrange.lightning.positionCount; i++)
                positions.Add(Vector2.zero);
            bigOrange.lightning.SetPositions(positions.ToArray());
        }

        void RedrawLightning()
        {
            Vector2 ShoulderArmHalf = MidPoint(shoulder.position, arm.position);
            Vector2 ArmHandHalf = MidPoint(arm.position, hand.position);

            Vector3[] anchorPositions = new Vector3[]
            {
                shoulder.position,
                MidPoint(shoulder.position, ShoulderArmHalf),
                ShoulderArmHalf,
                MidPoint(ShoulderArmHalf, arm.position),
                arm.position,
                MidPoint(arm.position, ArmHandHalf),
                ArmHandHalf,
                MidPoint(ArmHandHalf, hand.position),
                hand.position,
            };

            Vector2[] colliderPositions = new Vector2[]
            {
                shoulder.localPosition,
                arm.localPosition,
                hand.localPosition,
            };

            edge = bigOrange.lightning.gameObject.GetComponent<EdgeCollider2D>();
            edge.points = colliderPositions;

            List<Vector3> positions = new List<Vector3>();
            int anchorIndex = 0;
            int positionCounter = 0;
            bool returning = false;

            while(positionCounter < bigOrange.lightning.positionCount)
            {
                Debug.Log($"{anchorIndex} out of {bigOrange.lightning.positionCount}");
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

        Vector2 MidPoint(Vector2 a, Vector2 b)
        {
            return a + (b - a) / 2f;
        }
    }
}
