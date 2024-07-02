using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileValidationService : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Grid _grid;
    private GridSystem _gridSystem;
    

    private void Awake()
    {
        _gridSystem = GetComponent<GridSystem>();
    }

    public List<GridPosition> GetAttackableTileGridPosition(Unit unit)
    {
        GridPosition attackOriginGridPosition = _gridSystem.GetGridPositionFromWorldPosition(unit.GetWorldPosition());
        int attackRange = unit.GetAttackRange();

        HashSet<GridPosition> attackableTileGridPositionHashSet = new();
        Queue<GridPosition> queue = new();

        // movableCells.Add(position);
        queue.Enqueue(attackOriginGridPosition);

        for (int step = 0; step < attackRange; step++)
        {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                GridPosition current = queue.Dequeue();

                GridPosition[] directions = {
                    new(current.X, current.Y + 1), // Up
                    new(current.X, current.Y - 1), // Down
                    new(current.X - 1, current.Y), // Left
                    new(current.X + 1, current.Y)  // Right
                };

                foreach (GridPosition direction in directions)
                {
                    if (!attackableTileGridPositionHashSet.Contains(direction))
                    {
                        TileBase tile = _tilemap.GetTile(direction.GetGridPositionVector3Int());
                        if (tile is ScriptableTile scriptableTile)
                        {
                            if (
                            direction != attackOriginGridPosition &&
                            scriptableTile.IsWalkable
                        )
                            {

                                if (direction != attackOriginGridPosition)
                                {
                                    attackableTileGridPositionHashSet.Add(direction);
                                }


                                queue.Enqueue(direction);
                            }
                        }
                    }
                }
            }
        }
        return new List<GridPosition>(attackableTileGridPositionHashSet);
    }

    public List<GridPosition> GetMovableTileGridPositions(Unit unit)
    {
        GridPosition movementOriginGridPosition = _gridSystem.GetGridPositionFromWorldPosition(unit.GetWorldPosition());
        int movementRange = unit.GetRange();

        HashSet<GridPosition> movableTileGridPositionHashSet = new();
        Queue<GridPosition> queue = new();

        // movableCells.Add(position);
        queue.Enqueue(movementOriginGridPosition);

        for (int step = 0; step < movementRange; step++)
        {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                GridPosition current = queue.Dequeue();

                GridPosition[] directions = {
                    new(current.X, current.Y + 1), // Up
                    new(current.X, current.Y - 1), // Down
                    new(current.X - 1, current.Y), // Left
                    new(current.X + 1, current.Y)  // Right
                };

                foreach (GridPosition direction in directions)
                {
                    if (!movableTileGridPositionHashSet.Contains(direction))
                    {
                        TileBase tile = _tilemap.GetTile(direction.GetGridPositionVector3Int());
                        if (tile is ScriptableTile scriptableTile)
                        {
                            if (
                            !_gridSystem.IsGridPositionOccupiedByUnit(direction) &&
                            scriptableTile.IsWalkable
                        )
                            {

                                if (direction != movementOriginGridPosition)
                                {
                                    movableTileGridPositionHashSet.Add(direction);
                                }


                                queue.Enqueue(direction);
                            }
                        }
                    }
                }
            }
        }
        return new List<GridPosition>(movableTileGridPositionHashSet);
    }
}
