using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    Z=0,Z2,Y,X,X2
}

[Serializable]
public struct IntVector3 : IEquatable<IntVector3> {

    public IntVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static IntVector3[] AxisEulerAngles = 
    {
        new IntVector3(  0,  0,  90),
        new IntVector3(  0,  0, -90),
        new IntVector3(  0, 90,   0),
        new IntVector3( 90,  0,   0),
        new IntVector3(-90,  0,   0),
    };

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return Equals((IntVector3)obj);
    }

    public bool Equals(IntVector3 other)
    {
        return (x == other.x) && (y == other.y) && (z == other.z);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }
}

public static class IntVector3Extensions
{
    public static IntVector3 ToIntVector3(this Vector3 v)
    {
        var x = Mathf.RoundToInt(v.x);
        var y = Mathf.RoundToInt(v.y);
        var z = Mathf.RoundToInt(v.z);
        return new IntVector3(x, y, z);
    }
}