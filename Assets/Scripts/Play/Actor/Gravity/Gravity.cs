using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Yannick Cote
    public class Gravity : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Player player = null;
        
        [Header("Adjust force impact")]
        [SerializeField][Range(0, 100)] private int xAxisPositiveAdjust = 20;
        [SerializeField][Range(0, 100)] private int xAxisNegativeAdjust = 20;
        [SerializeField][Range(0,100)] private int yAxisPositiveAdjust = 20;
        [SerializeField][Range(0,100)] private int yAxisNegativeAdjust = 20;

        [Header("Values")]
        [SerializeField] private float xValue;
        [SerializeField] private float yValue;
        [SerializeField] private float xForce;
        [SerializeField] private float yForce;
        
        private bool raycastIsTriggered;

        private Vector2 adjustedForce;
        private Vector2 forceVector;
        private float radius;
        private LayerMask playerLayer;
        private PointEffector2D pointEffector2D;
        
        void Start()
        {
            playerLayer = (1 << LayerMask.NameToLayer(R.S.Layer.Player));
            adjustedForce = new Vector2(1,1);
            player = Finder.Player;
            radius = GetComponent<CircleCollider2D>().radius;
            pointEffector2D = GetComponent<PointEffector2D>();
        }

        private Vector2 CalculateForceDirection()
        {
            return player.transform.position - transform.position;
        }

        private Vector2 GetPlayerForceImpact()
        {
            return new Vector2(CalculateForcePercentage(forceVector.x), CalculateForcePercentage(forceVector.y));
        }

        private float CalculateForcePercentage(float force)
        {
            return ((force * 100) / radius);
        }
        
        private float GetGravityImpactOnForceY(float force)
        {
            if (adjustedForce.y <= 0)
                return force + (force * yAxisPositiveAdjust / 100);
            return force - (force * yAxisNegativeAdjust / 100);
        }

        private float GetGravityImpactOnForceX(float force, bool isPositiveDirection)
        {
            if (isPositiveDirection)
            {
                if (adjustedForce.x <= 0)
                    return force + (force * xAxisPositiveAdjust/100);
                return force - (force * xAxisNegativeAdjust/100);
            }
            if (adjustedForce.x <= 0)
                    return force - (force * xAxisNegativeAdjust/100);
            return force + (force * xAxisPositiveAdjust/100);
        }
        
        public float CalculateForceToApplyY(float yForce)
        {
            return GetGravityImpactOnForceY(yForce);
        }
        
        public float CalculateForceToApplyX(Vector2 direction, float xSpeed)
        {
            if (direction.x > 0)
                return GetGravityImpactOnForceX(xSpeed, true);
            if (direction.x < 0)
                return GetGravityImpactOnForceX(xSpeed, false);
            return xSpeed;
        }

        void Update()
        {
            if (raycastIsTriggered)
            {
                forceVector = CalculateForceDirection();
                xValue = forceVector.x;
                yValue = forceVector.y;
                adjustedForce = GetPlayerForceImpact();
            }
            else
            {
                adjustedForce = new Vector2(0,100);
            }
            
            xForce = CalculateForcePercentage(forceVector.x);
            yForce = CalculateForcePercentage(forceVector.y);

            raycastIsTriggered = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        }

        public void DeactivatePointEffector(bool isDesabled)
        {
            pointEffector2D.enabled = !isDesabled;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.transform.position, transform.position);
        }
#endif
    } 
}

