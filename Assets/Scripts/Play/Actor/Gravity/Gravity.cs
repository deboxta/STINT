using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private Player player = null;
        [SerializeField][Range(-100,100)] private float xAxisAdjust = 0;
        [SerializeField][Range(-100,100)] private float yAxisAdjust = 0;

        public static Vector2 force;

        public float XAxisAdjust => xAxisAdjust;

        public float YAxisAdjust => yAxisAdjust;

        public Vector2 DistanceBetweenPlayerAndGravityObject => distanceBetweenPlayerAndGravityObject;

        private MainController mainController;
        private PointEffector2D pointEffector2D;
        private Vector2 distanceBetweenPlayerAndGravityObject;
        
        // Start is called before the first frame update
        void Start()
        {
            mainController = Finder.MainController;
            player = Finder.Player;
            pointEffector2D = GetComponent<PointEffector2D>();
        }

        public void calculateForceDirection()
        {
            distanceBetweenPlayerAndGravityObject = player.transform.position - transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            calculateForceDirection();
            //player.GetComponent	<Rigidbody>()
            force = new Vector2(xAxisAdjust,yAxisAdjust);
        }
    } 
}

