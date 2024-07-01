using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WorldPosition
{
    public float X { get; private set; }
    
    public float Y { get; private set; }

    public WorldPosition(float x, float y)
    {
        X = x;
        Y = y;
    }

    public WorldPosition(Vector2 worldPositionVector2)
    {
        X = worldPositionVector2.x;
        Y = worldPositionVector2.y;
    }

    public WorldPosition(Vector3 worldPositionVector3)
    {
        X = worldPositionVector3.x;
        Y = worldPositionVector3.y;
    }

    public readonly Vector2 GetWorldPositionVector2()
    {
        return new Vector2(X, Y);
    }

    public readonly Vector3 GetWorldPositionVector3()
    {
        return new Vector3(X, Y, 0);
    }

    public static WorldPosition operator +(WorldPosition a, WorldPosition b)
    {
        return new WorldPosition(a.X + b.X, a.Y + b.Y);
    }

    public static WorldPosition operator -(WorldPosition a, WorldPosition b)
    {
        return new WorldPosition(a.X - b.X, a.Y - b.Y);
    }

    public static WorldPosition operator *(WorldPosition a, float b)
    {
        return new WorldPosition(a.X * b, a.Y * b);
    }

    public static WorldPosition operator /(WorldPosition a, float b)
    {
        return new WorldPosition(a.X / b, a.Y / b);
    }

    public static bool operator ==(WorldPosition a, WorldPosition b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(WorldPosition a, WorldPosition b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is WorldPosition position)
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
