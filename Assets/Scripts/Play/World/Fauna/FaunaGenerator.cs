using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.Fauna)]
    public class FaunaGenerator : MonoBehaviour
    {
        private const string FAUNA_ROOT_NAME = "FaunaGameObjects";

        [SerializeField] [Range(0f, 1f)] private float bunniesDensity = 0.05f;
        [SerializeField] [Range(0f, 1f)] private float foxDensity = 0.005f;

        private RandomSeed randomSeed;
        private PrefabFactory prefabFactory;
        private NavigationMesh navigationMesh;

        private GameObject faunaRoot;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
            prefabFactory = Finder.PrefabFactory;
            navigationMesh = Finder.NavigationMesh;

            faunaRoot = CreateRoot(FAUNA_ROOT_NAME);
        }

        private GameObject CreateRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        public void Generate()
        {
            DestroyFauna();
            CreateFauna();
        }

        private void CreateFauna()
        {
            var nodes = navigationMesh.Nodes.ToList();
            var nbGrassBlocks = nodes.Count;

            var faunaPrefabs = new Dictionary<int, int>
            {
                [0] = (int) (nbGrassBlocks * bunniesDensity),
                [1] = (int) (nbGrassBlocks * foxDensity),
            };

            var random = randomSeed.CreateRandom();

            while (faunaPrefabs.Count > 0 && nodes.Count > 0)
            {
                var position = nodes.RemoveRandom(random).Position3D;
                var prefab = faunaPrefabs.SubtractRandom(random);

                if (prefab == 0) prefabFactory.CreateBunny(position, faunaRoot);
                else prefabFactory.CreateFox(position, faunaRoot);
            }
        }

        private void DestroyFauna()
        {
            for (var i = 0; i < faunaRoot.transform.childCount; i++)
                Destroy(faunaRoot.transform.GetChild(i).gameObject);
        }
    }
}