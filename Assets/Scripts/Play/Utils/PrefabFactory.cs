using Harmony;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class PrefabFactory : MonoBehaviour
    {
        [Header("Animals")] [SerializeField] private GameObject bunny = null;
        [SerializeField] private GameObject fox = null;
        [Header("Plants")] [SerializeField] private GameObject[] grass = new GameObject[3];
        [SerializeField] private GameObject[] tree = new GameObject[3];
        [Header("Objects")] [SerializeField] private GameObject[] rocks = new GameObject[3];
        [Header("Misc")] [SerializeField] private GameObject water = null;

        private RandomSeed randomSeed;
        private uint bunnyId;
        private uint foxId;
        private uint grassId;
        private uint treeId;
        private uint rockId;
        private uint waterId;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
        }

        public Bunny CreateBunny(Vector3 position, GameObject parent = null)
        {
            var newBunny = Create<Bunny>(bunny, position, parent);
            newBunny.name = $"Bunny({bunnyId++})";
            return newBunny;
        }

        public Fox CreateFox(Vector3 position, GameObject parent = null)
        {
            var newFox = Create<Fox>(fox, position, parent);
            newFox.name = $"Fox({foxId++})";
            return newFox;
        }

        public Grass CreateGrass(Vector3 position, Random random, GameObject parent = null)
        {
            var newGrass = Create<Grass>(grass.Random(random), position, parent);
            newGrass.name = $"Grass({grassId++})";
            return newGrass;
        }

        public GameObject CreateTree(Vector3 position, Random random, GameObject parent = null)
        {
            var newTree = Create(tree.Random(random), position, parent);
            newTree.name = $"Tree({treeId++})";
            return newTree;
        }

        public GameObject CreateRock(Vector3 position, Random random, GameObject parent = null)
        {
            var newRock = Create(rocks.Random(random), position, parent);
            newRock.name = $"Rock({rockId++})";
            return newRock;
        }

        public Water CreateWater(Vector3 position, GameObject parent = null)
        {
            var newWater = Create<Water>(water, position, parent);
            newWater.name = $"Water({waterId++})";
            return newWater;
        }

        private static GameObject Create(GameObject prefab, Vector3 position, GameObject parent = null)
        {
            if (!ReferenceEquals(parent, null)) //Comparing a GameObject to null is expensive, because of lifecycle checks.
            {
                return Instantiate(prefab, position, Quaternion.identity, parent.transform);
            }
            else
            {
                return Instantiate(prefab, position, Quaternion.identity);
            }
        }

        private static T Create<T>(GameObject prefab, Vector3 position, GameObject parent = null)
        {
            if (!ReferenceEquals(parent, null)) //Comparing a GameObject to null is expensive, because of lifecycle checks.
            {
                return Instantiate(prefab, position, Quaternion.identity, parent.transform).GetComponent<T>();
            }
            else
            {
                return Instantiate(prefab, position, Quaternion.identity).GetComponent<T>();
            }
        }
    }
}