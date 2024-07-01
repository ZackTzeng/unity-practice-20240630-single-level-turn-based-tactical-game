using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridPosition
{
    [field: SerializeField] public int X { get; private set; }
    
    [field: SerializeField] public int Y { get; private set; }

    public GridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public GridPosition(Vector2Int gridPositionVector2Int)
    {
        X = gridPositionVector2Int.x;
        Y = gridPositionVector2Int.y;
    }

    public GridPosition(Vector3Int gridPositionVector3Int)
    {
        X = gridPositionVector3Int.x;
        Y = gridPositionVector3Int.y;
    }

    public readonly Vector2Int GetGridPositionVector2Int()
    {
        return new Vector2Int(X, Y);
    }

    public readonly Vector3Int GetGridPositionVector3Int()
    {
        return new Vector3Int(X, Y, 0);
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is GridPosition position)
        {
            return this == position;
        }
        return false;
    }

    public override readonly int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"({X}, {Y})";
    }
}
