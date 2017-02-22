using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PuzzleGrid
{
    PuzzlePiece[,,] grid = new PuzzlePiece[4,4,4];

    public bool IsValidBoardPosition(PuzzlePiece piece)
    {
        return piece.BlockLocations.TrueForAll(b =>
            b.x >= 0 &&
            b.y >= 0 &&
            b.z >= 0 &&
            b.x < 4 &&
            b.y < 4 &&
            b.z < 4);
    }

    public bool IsValidMove(PuzzlePiece piece)
    {
        return IsValidBoardPosition(piece) && 
               piece.BlockLocations.TrueForAll(b =>
                   grid[b.z, b.y, b.x] == null) &&
               !DoUnfillableHolesExist();
    }

    static IntVector3[] offsets = new IntVector3[] {
        new IntVector3(-1, 0, 0),
        new IntVector3(+1, 0, 0),
        new IntVector3(0, -1, 0),
        new IntVector3(0, +1, 0),
        new IntVector3(0, 0, -1),
        new IntVector3(0, 0, +1),
    };

    bool DoUnfillableHolesExist()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if(grid[z, y, x] == null && offsets.All(o => 
                       x + o.x >= 0 && x + o.x < 4 &&
                       y + o.y >= 0 && y + o.y < 4 &&
                       z + o.z >= 0 && z + o.z < 4 &&
                       grid[z + o.z, y + o.y, x + o.x] != null))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void AddPiece(PuzzlePiece piece)
    {
        piece.BlockLocations.ForEach(b => grid[b.z, b.y, b.x] = piece);
    }    

    public void RemovePiece(PuzzlePiece piece)
    {
        piece.BlockLocations.ForEach(b => grid[b.z, b.y, b.x] = null);
    }
}
