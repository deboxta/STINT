using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool isEnter;

        public static Vector2 AdjustedForce;
        
        public float XAxisAdjust => xAxisAdjust;

        public float YAxisAdjust => yAxisAdjust;
        
        private MainController mainController;
        private PointEffector2D pointEffector2D;
        private Vector2 forceVector;
        private float radius;

        // Start is called before the first frame update
        void Start()
        {
            AdjustedForce = new Vector2(1,1);
            isEnter = false;
            mainController = Finder.MainController;
            player = Finder.Player;
            pointEffector2D = GetComponent<PointEffector2D>();
            radius = GetComponent<CircleCollider2D>().radius;
        }

        private Vector2 calculateForceDirection()
        {
            return player.transform.position - transform.position;
        }

        public Vector2 calculatePlayerForceImpact()
        {
            //return new Vector2((xAxisAdjust/(forceVector.x%adjustForceFactor)), (yAxisAdjust/(forceVector.y%adjustForceFactor)));
            xForce = xAxisAdjust/(100-(Mathf.Abs(forceVector.x) * 100) / radius);
            return new Vector2(xAxisAdjust/(100-((forceVector.x*100)/radius)), yAxisAdjust/(100-((forceVector.y*100)/radius)));
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnter)
            {
                forceVector = calculateForceDirection();
                xvalue = forceVector.x;
                yvalue = forceVector.y;
                AdjustedForce = calculatePlayerForceImpact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(R.S.Tag.Player))
            {
                isEnter = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(R.S.Tag.Player))
            {
                isEnter = false;
                AdjustedForce = new Vector2(1,1);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.transform.position, transform.position);
        }
    } 
}

