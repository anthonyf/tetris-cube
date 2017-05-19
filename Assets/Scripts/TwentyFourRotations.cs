using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TwentyFourRotations
{
    public static IntVector3[] rotations =
    {
        new IntVector3(  0,0,0),
        new IntVector3( 90,0,0),
        new IntVector3(180,0,0),
        new IntVector3(-90,0,0),

        new IntVector3(  0,90,0),
        new IntVector3( 90,90,0),
        new IntVector3(180,90,0),
        new IntVector3(-90,90,0),

        new IntVector3(  0,-90,0),
        new IntVector3( 90,-90,0),
        new IntVector3(180,-90,0),
        new IntVector3(-90,-90,0),

        new IntVector3(  0,0,90),
        new IntVector3( 90,0,90),
        new IntVector3(180,0,90),
        new IntVector3(-90,0,90),

        new IntVector3(  0,0,180),
        new IntVector3 (90,0,180),
        new IntVector3(180,0,180),
        new IntVector3(-90,0,180),

        new IntVector3(  0,0,-90),
        new IntVector3( 90,0,-90),
        new IntVector3(180,0,-90),
        new IntVector3(-90,0,-90),
    };
}

