using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class ObjectThrower : MonoBehaviour, IFreezable
    {
        [SerializeField] [Range(0, 1000)] private int nbMaxThrowableObjects = 100;
        [SerializeField] private float speed = 1;
        [SerializeField] private float throwNextObjectDelay = 1;
        [SerializeField] public float removeObjectDelay = 3;

        private ThrowableObject throwableObjectModel;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Stopwatch throwNewObjectStopwatch;
        private ObjectPool<ThrowableObject> throwableObjects;
        private List<ThrowableObject> thrownObjects;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            throwNewObjectStopwatch = new Stopwatch();
            throwableObjectModel = this.GetRequiredComponentInChildren<ThrowableObject>(true);
            throwableObjectModel.gameObject.SetActive(false);
            throwableObjects = new ObjectPool<ThrowableObject>(
                () => Instantiate(throwableObjectModel, transform),
                nbMaxThrowableObjects
            );
            thrownObjects = new List<ThrowableObject>();
        }

        private void Start()
        {
            throwNewObjectStopwatch.Start();
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
        }

        private void FixedUpdate()
        {
            if (throwNewObjectStopwatch.Elapsed >= TimeSpan.FromSeconds(throwNextObjectDelay))
            {
                ThrowNewObject();

                throwNewObjectStopwatch.Restart();
            }
        }

        private void ThrowNewObject()
        {
            var objectToThrow = throwableObjects.GetObject();
            objectToThrow.gameObject.SetActive(true);
            objectToThrow.Rigidbody2D.velocity = transform.right * (speed * Time.fixedDeltaTime);
            thrownObjects.Add(objectToThrow);
        }

        public void RemoveThrownObject(ThrowableObject thrownObject)
        {
            thrownObject.gameObject.SetActive(false);
            thrownObjects.Remove(thrownObject);
        }

        private void OnTimeFreezeStateChanged()
        {
            if (IsFrozen)
                throwNewObjectStopwatch.Stop();
            else
                throwNewObjectStopwatch.Start();
        }
    }
}