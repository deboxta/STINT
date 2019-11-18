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
        [SerializeField] private Player player = null;
        [SerializeField][Range(-100,100)] private int xAxisAdjust = 20;
        [SerializeField][Range(-100,100)] private int yAxisAdjust = 20;
        [SerializeField] private float xvalue;
        [SerializeField] private float yvalue;
        [SerializeField] private float xForce;
        [SerializeField] private float yForce;
        [SerializeField][Range(-200,200)] private float adjustForceFactor;
        private bool raycastIsTriggered;

        public static Vector2 AdjustedForce;
        
        public float XAxisAdjust => xAxisAdjust;

        public float YAxisAdjust => yAxisAdjust;
        
        private MainController mainController;
        private PointEffector2D pointEffector2D;
        private Vector2 forceVector;
        private float radius;
        private LayerMask playerLayer;

        // Start is called before the first frame update
        void Start()
        {
            playerLayer = (1 << LayerMask.NameToLayer(R.S.Layer.Player));
            AdjustedForce = new Vector2(1,1);
            mainController = Finder.MainController;
            player = Finder.Player;
            pointEffector2D = GetComponent<PointEffector2D>();
            radius = GetComponent<CircleCollider2D>().radius;
        }

        private Vector2 CalculateForceDirection()
        {
            return player.transform.position - transform.position;
        }

        private Vector2 CalculatePlayerForceImpact()
        {
            return new Vector2(CalculateForceToApply(forceVector.x), CalculateForceToApply(forceVector.y));
        }

        private float CalculateForceToApply(float force)
        {
            return ((force * 100) / radius);
        }

        public static float GetGravityImpactOnForce(float force, bool isPositiveDirection)
        {
            if (isPositiveDirection)
                return (force - (force * AdjustedForce.x) / 100);
            return (force + (force * AdjustedForce.x) / 100);
        }
        
        public static Vector2 CalculateForceToApply(Vector2 direction, float xSpeed)
        {
            if (direction.x > 0)
                return new Vector2(direction.x * Gravity.GetGravityImpactOnForce(xSpeed, true), 0);
            return new Vector2(direction.x * Gravity.GetGravityImpactOnForce(xSpeed, false), 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (raycastIsTriggered)
            {
                forceVector = CalculateForceDirection();
                xvalue = forceVector.x;
                yvalue = forceVector.y;
                AdjustedForce = CalculatePlayerForceImpact();
            }
            else
            {
                AdjustedForce = new Vector2(0,0);
            }
            
            xForce = CalculateForceToApply(forceVector.x);
            yForce = CalculateForceToApply(forceVector.y);

            raycastIsTriggered = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.transform.position, transform.position);
        }
    } 
}

