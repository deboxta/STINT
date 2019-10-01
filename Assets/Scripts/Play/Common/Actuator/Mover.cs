using System.Collections;
using DG.Tweening;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Mover : MonoBehaviour
    {
        private const float DIRECTION_PRECISION = 0.5f;

        [Header("Movement")] [SerializeField] private float hopHeight = 0.2f;
        [Header("Duration")] [SerializeField] private float hopForwardDuration = 0.5f;
        [SerializeField] private float hopRotationDuration = 0.5f;

        private Transform parentTransform;
        private Tween currentTween;

        public bool IsMoving => currentTween != null;

        private void Awake()
        {
            parentTransform = transform.parent;
        }

        private void OnDestroy()
        {
            StopMove();
        }

        public void MoveTo(Vector3 position)
        {
            StopMove();

            StartCoroutine(MoveToRoutine(position));
        }

        private void StopMove()
        {
            if (currentTween != null)
            {
                currentTween.Kill(true);
                currentTween = null;
            }
        }

        private IEnumerator MoveToRoutine(Vector3 endPosition)
        {
            var startPosition = parentTransform.position;
            var currentDirection = parentTransform.forward;
            var desiredDirection = endPosition - startPosition;

            if (IsRotationNeeded(currentDirection, desiredDirection))
            {
                currentTween = DOTween.Sequence()
                    .Append(
                        RotateRoutine(currentDirection, desiredDirection)
                    )
                    .Append(
                        TranslateRoutine(startPosition, endPosition)
                    );

                yield return currentTween.WaitForCompletion();
            }
            else
            {
                currentTween = TranslateRoutine(
                    startPosition, endPosition
                );
                yield return currentTween.WaitForCompletion();
            }

            currentTween = null;
        }

        private Tweener TranslateRoutine(Vector3 startPosition, Vector3 endPosition)
        {
            var middlePosition = Vector3.Lerp(startPosition, endPosition, 0.5f) + Vector3.up * hopHeight;

            return parentTransform.DOPath(
                new[] {startPosition, middlePosition, endPosition},
                hopForwardDuration,
                PathType.CatmullRom
            );
        }

        private Tweener RotateRoutine(Vector3 currentDirection, Vector3 desiredDirection)
        {
            return parentTransform.DORotateQuaternion(
                Quaternion.LookRotation(desiredDirection),
                hopRotationDuration
            );
        }

        private static bool IsRotationNeeded(Vector3 currentDirection, Vector3 desiredDirection)
        {
            return desiredDirection != Vector3.zero && !currentDirection.IsSameDirection(desiredDirection, DIRECTION_PRECISION);
        }
    }
}