using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzlePiecePosition
{
    public TetrisPuzzlePiece puzzlePiece { get; set; }
    public IntVector3 position { get; set; }
    public IntVector3 eulerAngle { get; set; }
    public List<IntVector3> blockPositions { get; set; }
}
