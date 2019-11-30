﻿using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    [Findable(R.S.Tag.MainController)]
    public class PrefabFactory : MonoBehaviour
    {
        [SerializeField] private GameObject throwableObjectPrefab;

        public GameObject CreateThrowableObject(Transform parent = null)
        {
            return CreateThrowableObject(Vector3.zero, Quaternion.identity, parent);
        }
        public GameObject CreateThrowableObject(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var throwableObject = Instantiate(throwableObjectPrefab, position, rotation);
            if (parent) throwableObject.transform.parent = parent;
            return throwableObject;
        }
    }
}