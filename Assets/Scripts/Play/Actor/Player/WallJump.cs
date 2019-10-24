using System;
using System.Collections;
using Harmony;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class WallJump : MonoBehaviour
    {
        [SerializeField] private float distance = 1f;
        [SerializeField] private float speed = 2f;
        
        private Rigidbody2D rigidbody2D;
        private PlayerMover playerMover;
        private RaycastHit2D hit;
        private RaycastHit2D[] hits;
        private bool playerOnWall;
        bool CR_Running;

        public bool PlayerOnWall => playerOnWall;
        public RaycastHit2D Hit => hit;
        
        private void Start()
        {
            playerMover = GetComponent<PlayerMover>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerOnWall = false;
            CR_Running = false;
        }

        private void Update()
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.right * transform.localScale.x, distance);
            foreach (var singleHit in hits)
            {
                if (singleHit.collider.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Wall))
                {
                    hit = singleHit;
                    break;
                }
            }
        }

        public void JumpOnWall()
        {
            if (!CR_Running)
            {
                StartCoroutine("PlayerStickToWall");
            }
            if (PlayerOnWall)
            {
                rigidbody2D.velocity = new Vector2(speed * hit.normal.x, speed);
                //turn player when wall jumping
                transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
            }
            else if (!PlayerOnWall)
            {
                rigidbody2D.velocity = new Vector2(speed,speed);
            }
        }

        IEnumerator PlayerStickToWall()
        {
            CR_Running = true;
            playerOnWall = true; 
            yield return new WaitForSeconds(2);
            playerOnWall = false;
            playerMover.Fall();
            CR_Running = false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x  * distance);
        }
    }
}