using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PuzzleGrid
{
    bool[,,] grid = new bool[4,4,4];

    public bool IsValidBoardPosition(List<IntVector3> blocks)
    {
        return blocks.TrueForAll(b =>
            b.x >= 0 &&
            b.y >= 0 &&
            b.z >= 0 &&
            b.x < 4 &&
            b.y < 4 &&
            b.z < 4);
    }

    public bool IsValidMove(List<IntVector3> blocks)
    {
        return IsValidBoardPosition(blocks) && 
               blocks.ToList().TrueForAll(b =>
                   grid[b.z, b.y, b.x] == false);
    }

    static IntVector3[] offsets = new IntVector3[] {
        new IntVector3(-1, 0, 0),
        new IntVector3(+1, 0, 0),
        new IntVector3(0, -1, 0),
        new IntVector3(0, +1, 0),
        new IntVector3(0, 0, -1),
        new IntVector3(0, 0, +1),
    };

    IEnumerable<IntVector3> AdjacentCells(IntVector3 cell)
    {
        return offsets.Select(o => new IntVector3(
            o.x + cell.x, 
            o.y + cell.y, 
            o.z + cell.z))
            .Where(c => 
                c.x >= 0 &&
                c.x < 4 &&
                c.y >= 0 &&
                c.y < 4 &&
                c.z >= 0 && 
                c.z < 4);
    }

    bool IsEmpty(IntVector3 v)
    {
        return !grid[v.z, v.y, v.x];
    }

    void ForEachCell(Action<int, int, int> action)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    action.Invoke(x, y, z);
                }
            }
        }
    }

    public IEnumerable<HashSet<IntVector3>> FindHoles()
    {
        var holes = new List<HashSet<IntVector3>>();
        ForEachCell((x, y, z) =>
        {
            var currentCell = new IntVector3(x, y, z);
            if (IsEmpty(currentCell))
            {
                // find neighboring holes
                var neighborHoles = new List<HashSet<IntVector3>>();
                foreach (var neighborCell in AdjacentCells(currentCell))
                {
                    if (IsEmpty(neighborCell))
                    {
                        foreach(var hole in holes)
                        {
                            if(hole.Contains(neighborCell))
                            {
                                neighborHoles.Add(hole);
                            }
                        }
                    }
                }

                if(neighborHoles.Count == 0)
                {
                    var newHole = new HashSet<IntVector3>();
                    newHole.Add(currentCell);
                    holes.Add(newHole);
                }
                else if(neighborHoles.Count == 1)
                {
                    neighborHoles[0].Add(currentCell);
                }
                else // neighborHoles.Count > 1
                {
                    var combined = new HashSet<IntVector3>();
                    combined.Add(currentCell);
                    foreach (var hole in neighborHoles)
                    {
                        holes.Remove(hole);
                        foreach (var cell in hole)
                        {
                            combined.Add(cell);
                        }
                    }
                    holes.Add(combined);
                }

            }
        });

        holes = holes.OrderBy(h => h.Count).ToList();

        /*
        if(Debug.isDebugBuild)
        {
            foreach(var hole1 in holes)
            {
                foreach (var hole2 in holes)
                {
                    if (hole1 == hole2) continue;
                    foreach(var cell1 in hole1)
                    {
                        foreach (var cell2 in hole2)
                        {
                            Debug.Assert(cell1 != cell2, "overlapping holes!");
                        }
                    }
                }
            }
        }*/

        return holes;
    }

    /// <summary>
    /// Determines if a puzzle is in a state where it is solvable.  A puzzle is
    /// unsolvable if any contiguous empty spaces (holes) are not divisible by 4.
    /// </summary>
    /// <returns></returns>
    public bool IsPuzzleSolvable()
    {
        var holes = FindHoles();
        return IsPuzzleSolvable(holes);
    }

    public bool IsPuzzleSolvable(IEnumerable<HashSet<IntVector3>> holes)
    {
        foreach (var set in holes)
        {
            if (set.Count % 4 != 0)
            {
                return false;
            }
        }
        return true;
    }

    public void AddPiece(List<IntVector3> blocks)
    {
        blocks.ForEach(b => grid[b.z, b.y, b.x] = true);
    }    

    public void RemovePiece(List<IntVector3> blocks)
    {
        blocks.ForEach(b => grid[b.z, b.y, b.x] = false);
    }
}
