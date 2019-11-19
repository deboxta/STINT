using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Harmony;
using UnityEngine;

namespace Game
{
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

        private Vector2 AdjustedForce;
        private Vector2 forceVector;
        private float radius;
        private LayerMask playerLayer;
        void Start()
        {
            playerLayer = (1 << LayerMask.NameToLayer(R.S.Layer.Player));
            AdjustedForce = new Vector2(1,1);
            player = Finder.Player;
            radius = GetComponent<CircleCollider2D>().radius;
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
            if (AdjustedForce.y <= 0)
                return force + (force * yAxisPositiveAdjust / 100);
            return force - (force * yAxisNegativeAdjust / 100);
        }

        private float GetGravityImpactOnForceX(float force, bool isPositiveDirection)
        {
            if (isPositiveDirection)
            {
                if (AdjustedForce.x <= 0)
                    return force + (force * xAxisPositiveAdjust/100);
                return force - (force * xAxisNegativeAdjust/100);
            }
            if (AdjustedForce.x <= 0)
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

        // Update is called once per frame
        void Update()
        {
            if (raycastIsTriggered)
            {
                forceVector = CalculateForceDirection();
                xValue = forceVector.x;
                yValue = forceVector.y;
                AdjustedForce = GetPlayerForceImpact();
            }
            else
            {
                AdjustedForce = new Vector2(0,100);
            }
            
            xForce = CalculateForcePercentage(forceVector.x);
            yForce = CalculateForcePercentage(forceVector.y);

            raycastIsTriggered = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.transform.position, transform.position);
        }
    } 
}

