using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    Z=0,Z2,Y,X,X2
}

[Serializable]
public class IntVector3 {

    public IntVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public const int PUZZLE_SIZE = 4;
    static IntVector3[][,,] rotationTables = {
        MakeRotationTable(v => new IntVector3((PUZZLE_SIZE-1) - v.y, v.x, v.z)),
        MakeRotationTable(v => new IntVector3(v.y, (PUZZLE_SIZE-1) - v.x, v.z)),
        MakeRotationTable(v => new IntVector3((PUZZLE_SIZE-1) - v.z, v.y, v.x)),
        MakeRotationTable(v => new IntVector3(v.x, (PUZZLE_SIZE-1) - v.z, v.y)),
        MakeRotationTable(v => new IntVector3(v.x, v.z, (PUZZLE_SIZE-1) - v.y))};

    internal Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    static IntVector3[,,] MakeRotationTable(Func<IntVector3,IntVector3> func)
    {
        IntVector3[,,] table = new IntVector3[PUZZLE_SIZE, PUZZLE_SIZE, PUZZLE_SIZE];
        for(int z = 0; z < PUZZLE_SIZE; z++)
        {
            for(int y = 0; y < PUZZLE_SIZE; y++)
            {
                for(int x = 0; x < PUZZLE_SIZE; x++)
                {
                    table[z, y, x] = func(new IntVector3(x, y, z));
                }
            }
        }
        return table;
    }

    public IntVector3 Rotate(Axis axis)
    {
        var v = rotationTables[(int)axis][z, y, x];
        return new IntVector3(v.x, v.y, v.z);
    }

    public IntVector3 Move(IntVector3 offset)
    {
        return new IntVector3(offset.x + x, offset.y + y, offset.z + z);
    }
}
