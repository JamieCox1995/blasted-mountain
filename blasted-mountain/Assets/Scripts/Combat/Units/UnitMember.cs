using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat.Units
{
    public class UnitMember : MonoBehaviour
    {
        public Unit Unit;
        public UnitMemberType Type;

        public int MaximumHitPoints;
        private int currentHitPoints;

        public Vector3 WorldPosition;
        public Vector2 GridPosition;

        private List<Vector3> pathBuffer;
        private Vector3 originalPosition;
        private Vector3 targetPosition;

        private bool isMoving = false;

        public void Move(Vector3[] _Path)
        {
            if (isMoving) return;

            pathBuffer = _Path.ToList();
            StartCoroutine(MoveAlongPath());
        }

        private IEnumerator MoveAlongPath()
        {
            isMoving = true;

            float elapsedTime = 0f;

            originalPosition = transform.position;

            targetPosition = pathBuffer[0];
            pathBuffer.RemoveAt(0);

            Vector3 directionToTarget = (targetPosition - originalPosition).normalized;

            while(elapsedTime < 0.3f)
            {
                transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / 0.3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition = targetPosition;

            isMoving = false;

            if(pathBuffer.Count > 0)
            {
                StartCoroutine(MoveAlongPath());
            }
        }
    }
}
