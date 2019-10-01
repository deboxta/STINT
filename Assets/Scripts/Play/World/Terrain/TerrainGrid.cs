using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Game
{
    [Findable(R.S.Tag.Terrain)]
    public class TerrainGrid : MonoBehaviour
    {
        private const string TERRAIN_ROOT_NAME = "TerrainMeshes";
        private const string GRASS_TERRAIN_NAME = "Grass";
        private const string SAND_TERRAIN_NAME = "Sand";
        private const string WATER_TERRAIN_NAME = "Water";
        private const string BOTTOM_TERRAIN_NAME = "Bottom";

        [Header("Common")] [SerializeField] [Range(1, 10)] private float scale = 1f;
        [Header("Grass")] [SerializeField] private Material grassMaterial = null;
        [SerializeField] private float grassBlockHeight = 0.3f;
        [Header("Sand")] [SerializeField] private Material sandMaterial = null;
        [SerializeField] private float sandBlockHeight = 0.2f;
        [Header("Water")] [SerializeField] private Material waterMaterial = null;
        [SerializeField] private float waterBlockHeight = 0f;
        [Header("Bottom")] [SerializeField] private Material bottomMaterial = null;

        private GameObject terrainRoot;
        private Mesh grassMesh;
        private Mesh sandMesh;
        private Mesh waterMesh;
        private GameObject bottomGameObject;

        private TerrainBlock[,] blocks;
        
        public float Scale => scale;
        public float GrassBlockHeight => grassBlockHeight;
        public float SandBlockHeight => sandBlockHeight;
        public float WaterBlockHeight => waterBlockHeight;

        public Vector2Int GridSize => new Vector2Int(blocks.GetLength(0), blocks.GetLength(1));

        public Vector3 WorldSize
        {
            get
            {
                var gridSize = GridSize;
                return new Vector3(gridSize.x * scale, scale, gridSize.y * scale);
            }
        }

        public TerrainBlock[,] Blocks
        {
            get => blocks;
            set
            {
                blocks = value;

                UpdateTerrainMesh();
            }
        }

        public IEnumerable<TerrainBlock> GrassBlocks => EnumerateBlockType(TerrainType.Grass);
        public IEnumerable<TerrainBlock> SandBlocks => EnumerateBlockType(TerrainType.Sand);
        public IEnumerable<TerrainBlock> WaterBlocks => EnumerateBlockType(TerrainType.Water);

        private void Awake()
        {
            terrainRoot = CreateTerrainRoot(TERRAIN_ROOT_NAME);
            grassMesh = CreateEmptyTerrainMesh(GRASS_TERRAIN_NAME, grassMaterial);
            sandMesh = CreateEmptyTerrainMesh(SAND_TERRAIN_NAME, sandMaterial);
            waterMesh = CreateEmptyTerrainMesh(WATER_TERRAIN_NAME, waterMaterial);
            bottomGameObject = CreateTerrainBottomMesh(BOTTOM_TERRAIN_NAME);
        }

        private void OnEnable()
        {
            terrainRoot.SetActive(true);
        }

        private void OnDisable()
        {
            terrainRoot.SetActive(false);
        }

        public Vector3 GridToWorldPosition(Vector2Int position)
        {
            var gridSize = GridSize;

            var block = blocks[
                MathExtensions.Clamp(position.x, 0, gridSize.x - 1),
                MathExtensions.Clamp(position.y, 0, gridSize.y - 1)
            ];

            return block.WorldCenterPosition;
        }

        public Vector2Int WorldToGridPosition(Vector3 position)
        {
            var worldSize = WorldSize;

            position -= transform.position;
            position /= scale;

            return new Vector2Int(
                (int) MathExtensions.Clamp(position.x, 0, worldSize.x),
                (int) MathExtensions.Clamp(position.z, 0, worldSize.z)
            );
        }

        private GameObject CreateTerrainRoot(string gameObjectName)
        {
            var rootGameObject = new GameObject(gameObjectName);
            rootGameObject.transform.parent = transform;
            return rootGameObject;
        }

        private Mesh CreateEmptyTerrainMesh(string meshName, Material meshMaterial)
        {
            var meshGameObject = new GameObject(meshName);
            var meshTransform = meshGameObject.transform;
            meshTransform.parent = terrainRoot.transform;

            var mesh = new Mesh {name = meshName};
            var meshFilter = meshGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = meshMaterial;

            return mesh;
        }

        private GameObject CreateTerrainBottomMesh(string meshName)
        {
            var meshGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            meshGameObject.name = meshName;
            meshGameObject.transform.parent = terrainRoot.transform;
            meshGameObject.transform.Rotate(-90, 0, 0);
            meshGameObject.GetComponent<MeshRenderer>().material = bottomMaterial;
            return meshGameObject;
        }

        private void UpdateTerrainMesh()
        {
            grassMesh.Clear();
            sandMesh.Clear();
            waterMesh.Clear();
            bottomGameObject.SetActive(false);

            if (blocks != null)
            {
                var gridSize = GridSize;
                var grassMeshBuilder = new TerrainMeshBuilder();
                var sandMeshBuilder = new TerrainMeshBuilder();
                var waterMeshBuilder = new TerrainMeshBuilder();

                for (var x = 0; x < gridSize.x; x++)
                {
                    for (var y = 0; y < gridSize.y; y++)
                    {
                        var block = blocks[x, y];

                        TerrainMeshBuilder meshBuilder;
                        switch (block.Type)
                        {
                            case TerrainType.Grass:
                                meshBuilder = grassMeshBuilder;
                                break;
                            case TerrainType.Sand:
                                meshBuilder = sandMeshBuilder;
                                break;
                            case TerrainType.Water:
                                meshBuilder = waterMeshBuilder;
                                break;
                            default:
                                throw new Exception("Unknown terrain type named \"" + block.Type + "\".");
                        }

                        meshBuilder.Add(block);
                    }
                }

                grassMeshBuilder.Build(grassMesh);
                sandMeshBuilder.Build(sandMesh);
                waterMeshBuilder.Build(waterMesh);

                var worldSize = WorldSize;
                bottomGameObject.transform.localScale = new Vector3(worldSize.x, worldSize.z, 1);
                bottomGameObject.SetActive(isActiveAndEnabled);
            }
        }

        private IEnumerable<TerrainBlock> EnumerateBlockType(TerrainType terrainType)
        {
            foreach (var terrainBlock in blocks)
                if (terrainBlock.Type == terrainType)
                    yield return terrainBlock;
        }

        private class TerrainMeshBuilder
        {
            private readonly List<Vector3> vertices;
            private readonly List<int> indices;

            public TerrainMeshBuilder()
            {
                vertices = new List<Vector3>();
                indices = new List<int>();
            }

            public void Add(TerrainBlock block)
            {
                var position = block.WorldPosition;
                var scale = block.Scale;
                var height = block.Height;

                var edges = block.Edges;

                var initialVerticesCount = vertices.Count;

                //Vertices
                // -- Top
                vertices.Add(new Vector3(position.x, position.y, position.z));
                vertices.Add(new Vector3(position.x + scale, position.y, position.z));
                vertices.Add(new Vector3(position.x, position.y, position.z + scale));
                vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));

                // -- North
                if (edges.HasFlag(TerrainEdge.North))
                {
                    vertices.Add(new Vector3(position.x + scale, position.y, position.z));
                    vertices.Add(new Vector3(position.x, position.y, position.z));
                    vertices.Add(new Vector3(position.x + scale, position.y - height, position.z));
                    vertices.Add(new Vector3(position.x, position.y - height, position.z));
                }

                // -- East
                if (edges.HasFlag(TerrainEdge.East))
                {
                    vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));
                    vertices.Add(new Vector3(position.x + scale, position.y, position.z));
                    vertices.Add(new Vector3(position.x + scale, position.y - height, position.z + scale));
                    vertices.Add(new Vector3(position.x + scale, position.y - height, position.z));
                }

                // -- South
                if (edges.HasFlag(TerrainEdge.South))
                {
                    vertices.Add(new Vector3(position.x, position.y, position.z + scale));
                    vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));
                    vertices.Add(new Vector3(position.x, position.y - height, position.z + scale));
                    vertices.Add(new Vector3(position.x + scale, position.y - height, position.z + scale));
                }

                // -- West
                if (edges.HasFlag(TerrainEdge.West))
                {
                    vertices.Add(new Vector3(position.x, position.y, position.z));
                    vertices.Add(new Vector3(position.x, position.y, position.z + scale));
                    vertices.Add(new Vector3(position.x, position.y - height, position.z));
                    vertices.Add(new Vector3(position.x, position.y - height, position.z + scale));
                }

                //Indices
                void AddIndices()
                {
                    indices.Add(2 + initialVerticesCount);
                    indices.Add(1 + initialVerticesCount);
                    indices.Add(0 + initialVerticesCount);
                    indices.Add(2 + initialVerticesCount);
                    indices.Add(3 + initialVerticesCount);
                    indices.Add(1 + initialVerticesCount);
                }

                // -- Top
                AddIndices();

                // -- North
                if (edges.HasFlag(TerrainEdge.North))
                {
                    initialVerticesCount += 4;
                    AddIndices();
                }

                // -- East
                if (edges.HasFlag(TerrainEdge.East))
                {
                    initialVerticesCount += 4;
                    AddIndices();
                }

                // -- South
                if (edges.HasFlag(TerrainEdge.South))
                {
                    initialVerticesCount += 4;
                    AddIndices();
                }

                // -- East
                if (edges.HasFlag(TerrainEdge.West))
                {
                    initialVerticesCount += 4;
                    AddIndices();
                }
            }

            public void Build(Mesh target)
            {
                target.vertices = vertices.ToArray();
                target.triangles = indices.ToArray();

                target.RecalculateNormals();
                target.Optimize();
            }
        }
    }

    public class TerrainBlock
    {
        private readonly TerrainGrid terrain;
        private readonly Vector2Int gridPosition;
        private readonly TerrainType type;
        private readonly TerrainEdge edges;

        public TerrainBlock(TerrainGrid terrain, Vector2Int gridPosition, TerrainType type, TerrainEdge edges)
        {
            this.terrain = terrain;
            this.gridPosition = gridPosition;
            this.type = type;
            this.edges = edges;
        }

        public Vector2Int GridPosition => gridPosition;

        public Vector3 WorldPosition
        {
            get
            {
                var blockSize = terrain.Scale;

                float y;
                switch (type)
                {
                    case TerrainType.Grass:
                        y = terrain.GrassBlockHeight;
                        break;
                    case TerrainType.Sand:
                        y = terrain.SandBlockHeight;
                        break;
                    case TerrainType.Water:
                        y = terrain.WaterBlockHeight;
                        break;
                    default:
                        throw new Exception("Unknown terrain type named \"" + type + "\".");
                }

                var localPosition = new Vector3(
                    gridPosition.x * blockSize,
                    y,
                    gridPosition.y * blockSize
                );
                
                return localPosition + terrain.transform.position - terrain.WorldSize / 2f;
            }
        }

        public Vector3 WorldCenterPosition
        {
            get
            {
                var halfBlockSize = terrain.Scale / 2;
                var blockCenterOffset = new Vector3(halfBlockSize, 0, halfBlockSize);
                return WorldPosition + blockCenterOffset;
            }
        }

        public float Scale => terrain.Scale;

        public float Height
        {
            get
            {
                var height = Scale;
                switch (type)
                {
                    case TerrainType.Grass:
                        height *= 1 + terrain.GrassBlockHeight;
                        break;
                    case TerrainType.Sand:
                        height *= 1 + terrain.SandBlockHeight;
                        break;
                    case TerrainType.Water:
                        height *= 1 + terrain.WaterBlockHeight;
                        break;
                    default:
                        throw new Exception("Unknown terrain type named \"" + type + "\".");
                }

                return height;
            }
        }

        public TerrainType Type => type;

        public TerrainEdge Edges => edges;

        public bool IsWalkable => type != TerrainType.Water;

        public bool IsEdge => edges != TerrainEdge.None;
    }

    public enum TerrainType
    {
        Grass,
        Sand,
        Water
    }

    [Flags]
    public enum TerrainEdge
    {
        None = 0,
        North = 1 << 0,
        East = 1 << 1,
        South = 1 << 2,
        West = 1 << 3,
    }
}