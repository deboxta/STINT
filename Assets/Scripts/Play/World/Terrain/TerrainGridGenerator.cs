using System;
using Harmony;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Game
{
    [Findable(R.S.Tag.Terrain)]
    public class TerrainGridGenerator : MonoBehaviour
    {
        [Header("Terrain")] [SerializeField] [Range(1, 1000)] private int width = 100;
        [SerializeField] [Range(1, 1000)] private int height = 100;
        [SerializeField] [Range(1, 100)] private float noiseScale = 25f;
        [SerializeField] [Range(1, 25)] private int noiseIterations = 4;
        [SerializeField] [Range(0.1f, 1)] private float noisePersistence = 0.5f;
        [SerializeField] [Range(1, 25)] private float noiseLacunarity = 2f;
        [Header("Sand")] [Range(0, 1)] [SerializeField] private float sandMaxHeight = 0.5f;
        [Header("Water")] [Range(0, 1)] [SerializeField] private float waterMaxHeight = 0.4f;

        private RandomSeed randomSeed;
        private TerrainGrid terrain;

        private void Awake()
        {
            randomSeed = Finder.RandomSeed;
            terrain = Finder.TerrainGrid;
        }

        public TerrainBlock[,] Generate()
        {
            //Generate Height Map from Perlin Noise.
            var heightMap = Noise.PerlinNoiseMap(
                width,
                height,
                randomSeed.Seed,
                noiseScale,
                noiseIterations,
                noisePersistence,
                noiseLacunarity
            );

            //Convert Height Map into TerrainType array.
            var terrainTypes = new TerrainType[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var currentHeight = heightMap[x, y];

                    if (currentHeight < waterMaxHeight)
                        terrainTypes[x, y] = TerrainType.Water;
                    else if (currentHeight < sandMaxHeight)
                        terrainTypes[x, y] = TerrainType.Sand;
                    else
                        terrainTypes[x, y] = TerrainType.Grass;
                }
            }

            //Convert TerrainType array into TerrainBlocks.
            var terrainBlocks = new TerrainBlock[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var position = new Vector2Int(x, y);

                    var terrainType = terrainTypes[x, y];

                    var terrainEdges = TerrainEdge.None;
                    if (y == 0 || terrainTypes[x, y - 1] != terrainType)
                        terrainEdges |= TerrainEdge.North;
                    if (x == width - 1 || terrainTypes[x + 1, y] != terrainType)
                        terrainEdges |= TerrainEdge.East;
                    if (y == height - 1 || terrainTypes[x, y + 1] != terrainType)
                        terrainEdges |= TerrainEdge.South;
                    if (x == 0 || terrainTypes[x - 1, y] != terrainType)
                        terrainEdges |= TerrainEdge.West;

                    terrainBlocks[x, y] = new TerrainBlock(terrain, position, terrainType, terrainEdges);
                }
            }

            return terrainBlocks;
        }
    }
}