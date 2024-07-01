using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Scriptable Tile", menuName = "Scriptable Tile")]
public class ScriptableTile : TileBase
{
    [field: SerializeField] public bool IsWalkable { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = Sprite;
        tileData.colliderType = Tile.ColliderType.Grid;
    }
}
