using System;
using Cinemachine;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class CameraConfiner : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CompositeCollider2D compositeCollider2D;

        private void Awake()
        {
            compositeCollider2D = GetComponent<CompositeCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();
            
            if (other.transform.CompareTag(R.S.Tag.Player))
            {
                cinemachineConfiner.m_BoundingShape2D = compositeCollider2D;
            
                //Need to call this function when the confiner is change during runtime
                cinemachineConfiner.InvalidatePathCache();
            }
        }
    }
}