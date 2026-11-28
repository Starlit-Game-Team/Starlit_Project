using System;
using UnityEngine;

public class Utilities
{
    public static Vector2 fromDirectionToVector2(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector2(0f, 1f);
            case Direction.Down:
                return new Vector2(0f, -1f);
            case Direction.Left:
                return new Vector2(-1f, 0f);
            case Direction.Right:
                return new Vector2(1f, 0f);
        }
        return new Vector2(0f, 0f);
    }
    public static Direction formVector2ToDirection(Vector2 dir)
    {
        if(dir == Vector2.up)
        {
            return Direction.Up;
        }
        else if(dir == Vector2.down)
        {
            return Direction.Down;
        }
        else if (dir == Vector2.right)
        {
            return Direction.Right;
        }
        else if (dir == Vector2.left)
        {
            return Direction.Left;
        }
        return Direction.Null;
    }
    public static int fromDirectionToInt(Direction dir)
    {
        if( dir == Direction.Null)
        {
            return -1;
        }
        return Array.IndexOf((new Direction[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right }), dir);
    }
}
