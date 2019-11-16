﻿using System;
using System.Diagnostics;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class ObjectThrower : MonoBehaviour, IFreezable
    {
        [Range(0, 1000)] [SerializeField] private int nbMaxThrowableObjects = 100;
        [Range(-30000, 30000)] [SerializeField] private float speed = 4000;
        [Range(0, 60)] [SerializeField] private float throwNextObjectDelay = 0.11f;
        [Range(0, 60)] [SerializeField] public float removeObjectDelay = 5;

        private ThrowableObject throwableObjectModel;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Stopwatch throwNewObjectStopwatch;
        private ObjectPool<ThrowableObject> throwableObjects;

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
            objectToThrow.transform.position = transform.position;
            objectToThrow.Rigidbody2D.velocity = transform.right * (speed * Time.fixedDeltaTime);
        }

        public void RemoveThrownObject(ThrowableObject thrownObject)
        {
            thrownObject.gameObject.SetActive(false);
            throwableObjects.PutObject(thrownObject);
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