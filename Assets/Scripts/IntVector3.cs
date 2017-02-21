using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntVector3 {

    public IntVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    internal Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}
