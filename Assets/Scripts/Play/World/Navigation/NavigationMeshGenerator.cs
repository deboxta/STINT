using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.NavigationMesh)]
    public class NavigationMeshGenerator : MonoBehaviour
    {
        private TerrainGrid terrain;
        private FloraGrid flora;

        private void Awake()
        {
            terrain = Finder.TerrainGrid;
            flora = Finder.FloraGrid;
        }

        public Graph Generate()
        {
            var terrainBlocks = terrain.Blocks;
            var floraBlocks = flora.Blocks;
            Graph graph = null;

            if (terrainBlocks != null)
            {
                var terrainPosition = terrain.transform.position;
                var terrainWorldSize = terrain.WorldSize;

                var topLeft = new Vector3(
                    terrainPosition.x - terrainWorldSize.x / 2f,
                    terrainPosition.y,
                    terrainPosition.z + terrainWorldSize.z / 2f
                );
                var bottomRight = new Vector3(
                    terrainPosition.x + terrainWorldSize.x / 2f,
                    terrainPosition.y,
                    terrainPosition.z - terrainWorldSize.z / 2f
                );

                graph = Graph.NewXZ(
                    topLeft,
                    bottomRight
                );

                var terrainGridSize = terrain.GridSize;
                var nodes = new Node[terrainGridSize.x, terrainGridSize.y];

                for (var x = 0; x < terrainGridSize.x; x++)
                {
                    for (var y = 0; y < terrainGridSize.y; y++)
                    {
                        var terrainBlock = terrainBlocks[x, y];
                        var floraBlock = floraBlocks == null ? FloraType.None : floraBlocks[x, y];

                        if (terrainBlock.IsWalkable && floraBlock != FloraType.Tree && floraBlock != FloraType.Rock)
                            nodes[x, y] = Node.NewXZ(terrainBlock.WorldCenterPosition);
                    }
                }

                graph.BeginTransaction();

                for (var x = 0; x < terrainGridSize.x; x++)
                {
                    for (var y = 0; y < terrainGridSize.y; y++)
                    {
                        var node = nodes[x, y];
                        if (node != null)
                        {
                            var northNode = y == 0 ? null : nodes[x, y - 1];
                            var eastNode = x == terrainGridSize.x - 1 ? null : nodes[x + 1, y];
                            var southNode = y == terrainGridSize.y - 1 ? null : nodes[x, y + 1];
                            var westNode = x == 0 ? null : nodes[x - 1, y];

                            if (northNode != null) node.AddNeighbour(northNode);
                            if (eastNode != null) node.AddNeighbour(eastNode);
                            if (southNode != null) node.AddNeighbour(southNode);
                            if (westNode != null) node.AddNeighbour(westNode);

                            graph.Add(node);
                        }
                    }
                }

                graph.EndTransaction();
            }

            return graph;
        }
    }
}