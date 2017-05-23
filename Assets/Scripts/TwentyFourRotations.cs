using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TwentyFourRotations
{
    public static readonly IntVector3[] rotations =
    {
        // precalculated these values using Calculate24Rotations function below
        new IntVector3(90, 270, 270),
        new IntVector3(90, 270, 0),
        new IntVector3(90, 270, 90),
        new IntVector3(90, 270, 180),
        new IntVector3(180, 90, 90),
        new IntVector3(180, 90, 180),
        new IntVector3(180, 90, 270),
        new IntVector3(180, 90, 0),
        new IntVector3(180, 180, 90),
        new IntVector3(180, 180, 180),
        new IntVector3(180, 180, 270),
        new IntVector3(180, 180, 0),
        new IntVector3(180, 270, 90),
        new IntVector3(180, 270, 180),
        new IntVector3(180, 270, 270),
        new IntVector3(180, 270, 0),
        new IntVector3(180, 0, 90),
        new IntVector3(180, 0, 180),
        new IntVector3(180, 0, 270),
        new IntVector3(180, 0, 0),
        new IntVector3(270, 270, 270),
        new IntVector3(270, 270, 0),
        new IntVector3(270, 270, 90),
        new IntVector3(270, 270, 180)
    };

    public static Matrix4x4 MakeRotationMatrix(IntVector3 rotation)
    {
        var q = Quaternion.identity;
        q.eulerAngles = rotation.ToVector3();
        var m = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
        return m;
    }

    public static List<IntVector3> RotatePoints(List<IntVector3> points, IntVector3 rotation)
    {
        var m = MakeRotationMatrix(rotation);
        return points.Select(p => IntVector3Extensions.ToIntVector3(m.MultiplyPoint(p.ToVector3()))).ToList();
    }

    /// <summary>
    /// Cubes have exactly 24 unique rotations.  This function finds them.
    /// </summary>
    /// <returns></returns>
    public static List<IntVector3> Calculate24Rotations()
    {
        var pointsToRotationsMap = new Dictionary<IntVector3, IntVector3>();

        for (var x = -270; x < 360; x += 90)
        {
            for (var y = -270; y < 360; y += 90)
            {
                for (var z = -270; z < 360; z += 90)
                {
                    var r = new IntVector3(x, y, z);
                    var points = new List<IntVector3>() { new IntVector3(1, 2, 3) };
                    var points2 = RotatePoints(points, r);
                    var point = points2[0];
                    pointsToRotationsMap[point] = r;
                }
            }
        }

        // print out all rotations
        pointsToRotationsMap.Keys.ToList().ForEach(p =>
        {
            var r = pointsToRotationsMap[p];
            Debug.Log("new IntVector3(" + r.x + ", " + r.y + ", " + r.z + "),");
        });

        return pointsToRotationsMap.Values.ToList();
    }
}

