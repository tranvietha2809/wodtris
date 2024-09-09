using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wordtris.GameCore.Objects
{
    public enum BlockType
    {
        I,
        O,
        T,
        L,
        J,
        S,
        Z,
        Single,
        Bomb,
        Dynamite,
    }

    public static class Data
    {
        public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
        public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
        public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

        public static readonly Dictionary<BlockType, Vector2Int[]> Cells = new()
    {
        { BlockType.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { BlockType.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { BlockType.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { BlockType.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { BlockType.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { BlockType.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { BlockType.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { BlockType.Single, new Vector2Int[] { new Vector2Int(0, 0) } },
        { BlockType.Bomb, new Vector2Int[] { new Vector2Int(0, 0) } },
        { BlockType.Dynamite, new Vector2Int[] { new Vector2Int(0, 0) } },
        //{ BlockType.Hammer, new Vector2Int[] { new Vector2Int(0, 0) } },
        //{ BlockType.Missle, new Vector2Int[] { new Vector2Int(0, 0) } },
        };

        private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

        private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

        public static readonly Dictionary<BlockType, Vector2Int[,]> WallKicks = new()
    {
        { BlockType.I, WallKicksI },
        { BlockType.J, WallKicksJLOSTZ },
        { BlockType.L, WallKicksJLOSTZ },
        { BlockType.O, WallKicksJLOSTZ },
        { BlockType.S, WallKicksJLOSTZ },
        { BlockType.T, WallKicksJLOSTZ },
        { BlockType.Z, WallKicksJLOSTZ },
        { BlockType.Single, new Vector2Int[1,1] { { new Vector2Int(0, 0) } } },
        { BlockType.Bomb, new Vector2Int[1,1] { { new Vector2Int(0, 0) } } },
        { BlockType.Dynamite, new Vector2Int[1,1] { { new Vector2Int(0, 0) } } },
        //{ BlockType.Missle, new Vector2Int[1,1] { { new Vector2Int(0, 0) } } },
    };
    }
}
