using System;
using Cinemachine;
using UnityEngine;

namespace Game
{
    public class CameraConfiner : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        //private BoxCollider2D boxCollider2D;
        private CompositeCollider2D compositeCollider2D;

        private void Awake()
        {
            //boxCollider2D = GetComponent<BoxCollider2D>();
            compositeCollider2D = GetComponent<CompositeCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();

            cinemachineConfiner.m_BoundingShape2D = compositeCollider2D;
            
            //Need to call this function when the confiner is change during runtime
            cinemachineConfiner.InvalidatePathCache();
        }
    }
}