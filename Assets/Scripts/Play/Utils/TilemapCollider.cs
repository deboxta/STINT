using System;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

//Inspired by https://stackoverflow.com/questions/51499554/using-a-tilemap-composite-collider-as-a-trigger-what-is-the-best-approach-for-g
namespace Game
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapCollider : MonoBehaviour
    {
        private Tilemap tilemap;
        private void Start()
        {
            tilemap = GetComponent<Tilemap>();
            var cellSize = tilemap.layoutGrid.cellSize / 2;
            var availablePlaces = new List<Vector3>();
            
            for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.yMax; i++)
            {
                for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
                {
                    Vector3Int localPlace = new Vector3Int(i,j,(int)tilemap.transform.position.y);
                    Vector3 place = tilemap.CellToWorld(localPlace);

                    var tile = tilemap.GetTile(localPlace);

                    if (tile)
                    {
                        availablePlaces.Add(place);
                        
                        var paradoxCollisionObject = new GameObject().AddComponent<BoxCollider2D>();
                        paradoxCollisionObject.isTrigger = true;
                        paradoxCollisionObject.transform.parent = tilemap.transform;
                        paradoxCollisionObject.transform.localPosition = tilemap.CellToLocal(localPlace) + cellSize;
                        paradoxCollisionObject.tag = R.S.Tag.DeathZone;
                        paradoxCollisionObject.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Floor);
                    }
                }
            }
        }
    }
}