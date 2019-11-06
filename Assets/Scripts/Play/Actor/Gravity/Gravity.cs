using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private Player player = null;

        public static float force;

        private MainController mainController;
        private PointEffector2D pointEffector2D;
        
        // Start is called before the first frame update
        void Start()
        {
            mainController = Finder.MainController;
            player = Finder.Player;
            pointEffector2D = GetComponent<PointEffector2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //player.GetComponent	<Rigidbody>()
            force = pointEffector2D.forceMagnitude;
        }
    } 
}

