using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class PrefabFactory : MonoBehaviour
    {
        [SerializeField] private GameObject throwableObjectPrefab;

        public GameObject CreateThrowableObject()
        {
            return CreateThrowableObject(Vector3.zero, Quaternion.identity);
        }
        public GameObject CreateThrowableObject(Vector3 position, Quaternion rotation, GameObject parent = null)
        {
            var throwableObject = Instantiate(throwableObjectPrefab, position, rotation);
            if (parent) throwableObject.transform.parent = parent.transform;
            return throwableObject;
        }
    }
}