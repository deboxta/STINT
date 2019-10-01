using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.Water)]
    public class WaterGenerator : MonoBehaviour
    {
        private const string WATER_ROOT_NAME = "WaterGameObjects";

        private PrefabFactory prefabFactory;
        private TerrainGrid terrain;

        private GameObject waterRoot;

        private void Awake()
        {
            prefabFactory = Finder.PrefabFactory;
            terrain = Finder.TerrainGrid;

            waterRoot = CreateRoot(WATER_ROOT_NAME);
        }

        private GameObject CreateRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        public void Generate()
        {
            DestroyWater();
            CreateWater();
        }

        private void CreateWater()
        {
            //Make sure the water points are inside the navigation mesh, so they are reachable.

            var blocks = terrain.Blocks;
            var waterBlocks = terrain.WaterBlocks;
            var gridSize = terrain.GridSize;

            var water = new bool[gridSize.x, gridSize.y].Fill(false);
            foreach (var waterBlock in waterBlocks)
            {
                if (waterBlock.IsEdge)
                {
                    var position = waterBlock.GridPosition;
                    var waterBlockEdges = waterBlock.Edges;

                    if (position.y > 0 && waterBlockEdges.HasFlag(TerrainEdge.North))
                        water[position.x, position.y - 1] = true;
                    if (position.x < gridSize.x - 1 && waterBlockEdges.HasFlag(TerrainEdge.East))
                        water[position.x + 1, position.y] = true;
                    if (position.x > 0 && waterBlockEdges.HasFlag(TerrainEdge.West))
                        water[position.x - 1, position.y] = true;
                    if (position.y < gridSize.y - 1 && waterBlockEdges.HasFlag(TerrainEdge.South))
                        water[position.x, position.y + 1] = true;
                }
            }

            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    if (water[x, y])
                    {
                        var position = blocks[x, y].WorldCenterPosition;

                        prefabFactory.CreateWater(position, waterRoot);
                    }
                }
            }
        }

        private void DestroyWater()
        {
            for (var i = 0; i < waterRoot.transform.childCount; i++)
                Destroy(waterRoot.transform.GetChild(i).gameObject);
        }
    }
}