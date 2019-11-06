using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Harmony;
using UnityEngine;

namespace Game
{
    public interface ISensor<out T>
    {
        event SensorEventHandler<T> OnSensedObject;
        event SensorEventHandler<T> OnUnsensedObject;

        IReadOnlyList<T> SensedObjects { get; }
    }

    public sealed class Sensor : MonoBehaviour, ISensor<GameObject>
    {
        [SerializeField] private Shape shape = Shape.Circle;
        [SerializeField] [Range(1, 100)] private float xSize = 10;
        [SerializeField] [Range(1, 100)] private float ySize = 10;

        private Transform parentTransform;
        private new Collider2D collider2D;
        private readonly List<GameObject> sensedObjects;
        private ulong dirtyFlag;
        
        public float YSize
        {
            get => ySize;
            set => ySize = value;
        }

        public event SensorEventHandler<GameObject> OnSensedObject;
        public event SensorEventHandler<GameObject> OnUnsensedObject;

        public IReadOnlyList<GameObject> SensedObjects => sensedObjects;
        public ulong DirtyFlag => dirtyFlag;

        public Sensor()
        {
            sensedObjects = new List<GameObject>();
            dirtyFlag = ulong.MinValue;
        }

        private void Awake()
        {
            parentTransform = transform.parent;

            CreateCollider();
            SetSensorLayer();
        }

        private void OnEnable()
        {
            collider2D.enabled = true;
            collider2D.isTrigger = true;
        }

        private void OnDisable()
        {
            collider2D.enabled = false;
            collider2D.isTrigger = false;
            ClearSensedObjects();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherParentTransform = other.transform.parent;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.GetComponent<Stimuli>();
                if (stimuli != null)
                {
                    stimuli.OnDestroyed += RemoveSensedObject;
                    AddSensedObject(otherParentTransform.gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherParentTransform = other.transform.parent;
            if (!IsSelf(otherParentTransform))
            {
                var stimuli = other.GetComponent<Stimuli>();
                if (stimuli != null)
                {
                    stimuli.OnDestroyed -= RemoveSensedObject;
                    RemoveSensedObject(otherParentTransform.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (SensedObjects != null)
                foreach (var sensedObject in SensedObjects)
                    GizmosExtensions.DrawPoint(sensedObject.transform.position);
        }
#endif

        private void AddSensedObject(GameObject otherObject)
        {
            if (!sensedObjects.Contains(otherObject))
            {
                sensedObjects.Add(otherObject);
                dirtyFlag++;
                NotifySensedObject(otherObject);
            }
        }

        private void RemoveSensedObject(GameObject otherObject)
        {
            if (sensedObjects.Contains(otherObject))
            {
                sensedObjects.Remove(otherObject);
                dirtyFlag++;
                NotifyUnsensedObject(otherObject);
            }
        }

        public ISensor<T> For<T>()
        {
            return new Sensor<T>(this);
        }

        private void CreateCollider()
        {
            switch (shape)
            {
                case Shape.Square:
                    var squareCollider = gameObject.AddComponent<BoxCollider2D>();
                    squareCollider.size = new Vector2(xSize, ySize);
                    collider2D = squareCollider;
                    break;
                case Shape.Circle:
                    var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                    circleCollider.radius = xSize / 2;
                    collider2D = circleCollider;
                    break;
                default:
                    throw new Exception("Unknown shape named \"" + shape + "\".");
            }

            //Needed to be able to detect something when moved. DO NOT REMOVE THIS!!!!!!!!
            gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
        }

        private void SetSensorLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Sensor);
        }

        private void ClearSensedObjects()
        {
            sensedObjects.Clear();
            dirtyFlag++;
        }

        private bool IsSelf(Transform otherParentTransform)
        {
            return parentTransform == otherParentTransform;
        }

        private void NotifySensedObject(GameObject otherObject)
        {
            if (OnSensedObject != null) OnSensedObject(otherObject);
        }

        private void NotifyUnsensedObject(GameObject otherObject)
        {
            if (OnUnsensedObject != null) OnUnsensedObject(otherObject);
        }

        private enum Shape
        {
            Square,
            Circle
        }
    }

    [SuppressMessage("ReSharper", "DelegateSubtraction")]
    public sealed class Sensor<T> : ISensor<T>
    {
        private readonly Sensor sensor;
        private SensorEventHandler<T> onSensedObject;
        private SensorEventHandler<T> onUnsensedObject;

        private readonly List<T> sensedObjects;
        private ulong dirtyFlag;

        public IReadOnlyList<T> SensedObjects
        {
            get
            {
                if (IsDirty()) UpdateSensor();

                return sensedObjects;
            }
        }

        public event SensorEventHandler<T> OnSensedObject
        {
            add
            {
                if (onSensedObject == null || onSensedObject.GetInvocationList().Length == 0)
                    sensor.OnSensedObject += OnSensedObjectInternal;
                onSensedObject += value;
            }
            remove
            {
                if (onSensedObject != null && onSensedObject.GetInvocationList().Length == 1)
                    sensor.OnSensedObject -= OnSensedObjectInternal;
                onSensedObject -= value;
            }
        }

        public event SensorEventHandler<T> OnUnsensedObject
        {
            add
            {
                if (onUnsensedObject == null || onUnsensedObject.GetInvocationList().Length == 0)
                    sensor.OnUnsensedObject += OnUnsensedObjectInternal;
                onUnsensedObject += value;
            }
            remove
            {
                if (onUnsensedObject != null && onUnsensedObject.GetInvocationList().Length == 1)
                    sensor.OnUnsensedObject -= OnUnsensedObjectInternal;
                onUnsensedObject -= value;
            }
        }

        public Sensor(Sensor sensor)
        {
            this.sensor = sensor;
            sensedObjects = new List<T>();
            dirtyFlag = sensor.DirtyFlag;

            UpdateSensor();
        }

        private bool IsDirty()
        {
            return sensor.DirtyFlag != dirtyFlag;
        }

        private void UpdateSensor()
        {
            sensedObjects.Clear();

            foreach (var otherObject in sensor.SensedObjects)
            {
                var otherComponent = otherObject.GetComponentInChildren<T>();
                if (otherComponent != null) sensedObjects.Add(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void OnSensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && !sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Add(otherComponent);
                NotifySensedObject(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void OnUnsensedObjectInternal(GameObject otherObject)
        {
            var otherComponent = otherObject.GetComponentInChildren<T>();
            if (otherComponent != null && sensedObjects.Contains(otherComponent))
            {
                sensedObjects.Remove(otherComponent);
                NotifyUnsensedObject(otherComponent);
            }

            dirtyFlag = sensor.DirtyFlag;
        }

        private void NotifySensedObject(T otherObject)
        {
            if (onSensedObject != null) onSensedObject(otherObject);
        }

        private void NotifyUnsensedObject(T otherObject)
        {
            if (onUnsensedObject != null) onUnsensedObject(otherObject);
        }
    }

    public delegate void SensorEventHandler<in T>(T otherObject);
}