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

    public static Vector3[] AxisEulerAngles = 
    {
        new Vector3(  0,  0,  90),
        new Vector3(  0,  0, -90),
        new Vector3(  0, 90,   0),
        new Vector3( 90,  0,   0),
        new Vector3(-90,  0,   0),
    };

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    internal Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        IntVector3 v = (IntVector3)obj;
        return (x == v.x) && (y == v.y) && (z == v.z);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

    public static IntVector3 FromVector3(Vector3 v)
    {
        return new IntVector3((int)Mathf.Round(v.x), (int)Mathf.Round(v.y), (int)Mathf.Round(v.z));
    }
}
