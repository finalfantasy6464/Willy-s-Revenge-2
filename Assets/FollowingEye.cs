using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class FollowingEye : MonoBehaviour
    {
        public Transform eye;
        public Transform pupil;
        public Transform target;
        public float eyeRadius;

        void OnDrawGizmos()
        {
            if(eye == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(eye.transform.position, eyeRadius);
        }
        
        void Start()
        {
            target = FindObjectOfType<PlayerController2021remake>().transform;
            if(target == null) enabled = false;
        }

        void Update()
        {
            Vector3 lookDirection = (target.position - eye.transform.position).normalized;
            pupil.localPosition = (lookDirection * eyeRadius);
        }
    }
