using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TwentyFourRotationsTest
{
	[Test]
	public void TestMatrixRotations() {
        var points = new List<IntVector3>() { new IntVector3(1, 2, 3) };
        var points2 = TwentyFourRotations.RotatePoints(points, TwentyFourRotations.rotations[1]);
        Assert.AreEqual(-2, points2[0].x);
        Assert.AreEqual(-3, points2[0].y);
        Assert.AreEqual(1, points2[0].z);
    }

    [Test]
    public void TestAllMatrixRotations()
    {
        var allRotatedPoints = new HashSet<IntVector3>();
        TwentyFourRotations.rotations.ToList().ForEach(r =>
        {
            var points = new List<IntVector3>() { new IntVector3(1, 2, 0) };
            var points2 = TwentyFourRotations.RotatePoints(points, r);
            allRotatedPoints.Add(points2[0]);
        });

        Assert.AreEqual(24, allRotatedPoints.Count);
    }

    [Test]
    public void TestAllMatrixRotations2()
    {
        var rotations = TwentyFourRotations.Calculate24Rotations();
        Assert.AreEqual(24, rotations.Count);
    }
}
