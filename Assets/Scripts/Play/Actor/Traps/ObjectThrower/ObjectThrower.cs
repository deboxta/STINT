using System;
using System.Collections;
using System.Diagnostics;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class ObjectThrower : MonoBehaviour, IFreezable
    {
        [Range(0, 1000)] [SerializeField] private int throwableObjectPoolSize = 100;
        [Range(-500, 500)] [SerializeField] private float speed = 66;
        [Range(0, 60)] [SerializeField] private float throwNextObjectDelay = 0.175f;
        [Range(0, 60)] [SerializeField] public float removeObjectDelay = 5;

        private ThrowableObject throwableObjectModel;
        private ObjectPool<ThrowableObject> throwableObjectPool;
        private FreezableWaitForSeconds waitForThrowNewObjectDelay;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            throwableObjectModel = this.GetRequiredComponentInChildren<ThrowableObject>(true);
            throwableObjectModel.gameObject.SetActive(false);
            throwableObjectPool = new ObjectPool<ThrowableObject>(
                () =>
                {
                    var throwableObject = Instantiate(throwableObjectModel, transform);
                    throwableObject.Thrower = this;
                    return throwableObject;
                },
                throwableObjectPoolSize
            );

            waitForThrowNewObjectDelay = new FreezableWaitForSeconds(throwNextObjectDelay);
        }

        private void OnEnable()
        {
            StartCoroutine(ThrowNewObjectAtInterval());
        }

        private IEnumerator ThrowNewObjectAtInterval()
        {
            while (true)
            {
                yield return waitForThrowNewObjectDelay;
                ThrowNewObject();
                waitForThrowNewObjectDelay.Reset();
            }
        }

        private void ThrowNewObject()
        {
            var objectToThrow = throwableObjectPool.GetObject();
            objectToThrow.Reset();
            objectToThrow.gameObject.SetActive(true);
            objectToThrow.transform.position = transform.position;
            objectToThrow.Rigidbody2D.velocity = transform.right * speed;
        }

        public void RemoveThrownObject(ThrowableObject thrownObject)
        {
            thrownObject.gameObject.SetActive(false);
            throwableObjectPool.PutObject(thrownObject);
        }
    }
}