using System;
using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.Flora)]
    public class FloraGrid : MonoBehaviour
    {
        private const string FLORA_ROOT_NAME = "FloraGameObjects";

        private RandomSeed randomSeed;
        private PrefabFactory prefabFactory;
        private TerrainGrid terrain;

        private GameObject floraRoot;

        private FloraType[,] blocks;

        public FloraType[,] Blocks
        {
            get => blocks;
            set
            {
                blocks = value;

                UpdateFlora();
            }
        }

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
            prefabFactory = Finder.PrefabFactory;
            terrain = Finder.TerrainGrid;

            floraRoot = CreateRoot(FLORA_ROOT_NAME);
        }

        private GameObject CreateRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        private void UpdateFlora()
        {
            for (var i = 0; i < floraRoot.transform.childCount; i++) Destroy(floraRoot.transform.GetChild(i).gameObject);

            if (blocks != null)
            {
                var random = randomSeed.CreateRandom();
                var terrainBlocks = terrain.Blocks;
                var gridSize = terrain.GridSize;

                for (var x = 0; x < gridSize.x; x++)
                {
                    for (var y = 0; y < gridSize.y; y++)
                    {
                        var flora = blocks[x, y];
                        if (flora != FloraType.None)
                        {
                            var position = terrainBlocks[x, y].WorldCenterPosition;
                            switch (flora)
                            {
                                case FloraType.Grass:
                                    prefabFactory.CreateGrass(position, random, floraRoot);
                                    break;
                                case FloraType.Tree:
                                    prefabFactory.CreateTree(position, random, floraRoot);
                                    break;
                                case FloraType.Rock:
                                    prefabFactory.CreateRock(position, random, floraRoot);
                                    break;
                                default:
                                    throw new Exception("Unknown flora type named \"" + flora + "\".");
                            }
                        }
                    }
                }
            }
        }
    }

    public enum FloraType
    {
        None,
        Grass,
        Tree,
        Rock
    }
}