using System;
using UnityEngine;

namespace Game
{
    public class OrthographicOrbitCamera : MonoBehaviour
    {
        [Header("Initialisation")] [Range(1, 100)] [SerializeField] private float initialZoom = 20;
        [Header("Position")] [Range(1, 100)] [SerializeField] private float distance = 45f;
        [SerializeField] [Range(0, 90)] private float verticalAngle = 25f;
        [SerializeField] [Range(0, 360)] private float horizontalAngle = 45f;
        [SerializeField] [Range(1, 10)] private int rotationCount = 4;
        [Header("Movement")] [SerializeField] [Range(0, 10)] private float movementSpeed = 2f;
        [SerializeField] [Range(0f, 10f)] private float zoomSpeed = 0.5f;
        [SerializeField] [Range(0, 10)] private float rotationSpeed = 5f;
        [SerializeField] [Range(0, 10)] private float runMovementSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 10)] private float runZoomSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 10)] private float runRotationSpeedMultiplier = 2f;
        [SerializeField] [Range(0, 100)] private float minZoom = 1;
        [SerializeField] [Range(0, 100)] private float maxZoom = 50;
        [Header("Key Mapping")] [SerializeField] private KeyCode upKey = KeyCode.W;
        [SerializeField] private KeyCode downKey = KeyCode.S;
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode rotateLeftKey = KeyCode.Q;
        [SerializeField] private KeyCode rotateRightKey = KeyCode.E;
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

        private new Camera camera;

        private Vector3 tracking;
        private int panning;
        private float panningAngle;
        private float zoom;

        private bool upKeyDown;
        private bool downKeyDown;
        private bool leftKeyDown;
        private bool rightKeyDown;
        private bool rotateLeftKeyDown;
        private bool rotateRightKeyDown;
        private bool runKeyDown;
        private float mouseScrollWheelDelta;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            camera.orthographic = true;
        }

        private void OnDisable()
        {
            camera.orthographic = false;
        }

        private void Start()
        {
            tracking = Vector3.zero;
            panning = 0;
            zoom = initialZoom;
        }

        private void Update()
        {
            UpdateKeys();
            UpdateTracking();
            UpdatePanning();
            UpdateZoom();
            UpdateTransform();
        }

        private void UpdateKeys()
        {
            upKeyDown = Input.GetKey(upKey);
            downKeyDown = Input.GetKey(downKey);
            leftKeyDown = Input.GetKey(leftKey);
            rightKeyDown = Input.GetKey(rightKey);
            rotateLeftKeyDown = Input.GetKeyDown(rotateLeftKey);
            rotateRightKeyDown = Input.GetKeyDown(rotateRightKey);
            runKeyDown = Input.GetKey(runKey);
            mouseScrollWheelDelta = Input.mouseScrollDelta.y;
        }

        private void UpdateTracking()
        {
            var direction = Vector3.zero;
            var speed = movementSpeed;

            if (upKeyDown) direction += Vector3.up;
            if (downKeyDown) direction += Vector3.down;
            if (leftKeyDown) direction += Vector3.left;
            if (rightKeyDown) direction += Vector3.right;
            if (runKeyDown) speed *= Input.GetKey(runKey) ? runMovementSpeedMultiplier : 1f;

            tracking += direction.normalized * (speed * zoom * Time.unscaledDeltaTime);
        }

        private void UpdatePanning()
        {
            var direction = 0;
            var speed = rotationSpeed;

            if (rotateLeftKeyDown) direction += 1;
            if (rotateRightKeyDown) direction -= 1;
            if (runKeyDown) speed *= runRotationSpeedMultiplier;

            panning += direction;

            panningAngle = Mathf.Lerp(panningAngle, panning * (360f / rotationCount), speed * Time.unscaledDeltaTime);
        }

        private void UpdateZoom()
        {
            var direction = mouseScrollWheelDelta;
            var speed = runKeyDown ? runZoomSpeedMultiplier : zoomSpeed;

            zoom = Mathf.Clamp(zoom - direction * speed, minZoom, maxZoom);
        }

        private void UpdateTransform()
        {
            var trackingRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
            var panningRotation = Quaternion.Euler(0, panningAngle, 0);
            var rotation = panningRotation * trackingRotation;
            var trackingDistance = new Vector3(0, 0, -distance);

            transform.position = rotation * (tracking + trackingDistance);
            transform.rotation = rotation;
            camera.orthographicSize =  zoom;
        }
    }
}