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
        [SerializeField][Range(-200,200)] private float adjustForceFactor;

        public static Vector2 AdjustedForce;
        
        public float XAxisAdjust => xAxisAdjust;

        public float YAxisAdjust => yAxisAdjust;
        
        private MainController mainController;
        private PointEffector2D pointEffector2D;
        private Vector2 forceVector;

        // Start is called before the first frame update
        void Start()
        {
            mainController = Finder.MainController;
            player = Finder.Player;
            pointEffector2D = GetComponent<PointEffector2D>();
        }

        private Vector2 calculateForceDirection()
        {
            return player.transform.position - transform.position;
        }

        public Vector2 calculatePlayerForceImpact()
        {
            //return new Vector2((xAxisAdjust/(forceVector.x%adjustForceFactor)), (yAxisAdjust/(forceVector.y%adjustForceFactor)));
            return new Vector2(forceVector.x%GetComponent<CircleCollider2D>().radius, forceVector.y%GetComponent<CircleCollider2D>().radius);
        }

        // Update is called once per frame
        void Update()
        {
            forceVector = calculateForceDirection();
            xvalue = forceVector.x;
            yvalue = forceVector.y;
            AdjustedForce = calculatePlayerForceImpact();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(player.transform.position, transform.position);
        }
    } 
}

