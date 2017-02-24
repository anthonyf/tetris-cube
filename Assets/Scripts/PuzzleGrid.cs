﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                c.y >= 0 && 
                c.z >= 0 && 
                c.x < 4 && 
                c.y < 4 && 
                c.z < 4);
    }

    public IEnumerable<HashSet<IntVector3>> FindHoles()
    {
        var holes = new List<HashSet<IntVector3>>();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    var currentCell = new IntVector3(x, y, z);
                    if (grid[currentCell.z, currentCell.y, currentCell.x] == false)
                    {
                        var neighborFoundInSet = false;
                        foreach (var neighborCell in AdjacentCells(currentCell))
                        {
                            if (grid[neighborCell.z, neighborCell.y, neighborCell.x] == false)
                            {
                                foreach (var set in holes)
                                {
                                    if (set.Contains(neighborCell))
                                    {
                                        set.Add(currentCell);
                                        neighborFoundInSet = true;
                                    }
                                }
                            }
                        }
                        if (!neighborFoundInSet)
                        {
                            var newSet = new HashSet<IntVector3>();
                            newSet.Add(currentCell);
                            holes.Add(newSet);
                        }
                    }
                }
            }
        }        
        return holes.OrderBy(h => h.Count);
    }

    /// <summary>
    /// Determines if a puzzle is in a state where it is unsolveable.  A puzzle is
    /// unsolvable if any contiguous empty spaces (holes) are not divisible by 4.
    /// </summary>
    /// <returns></returns>
    public bool IsPuzzleUnsolveable()
    {
        var holes = FindHoles();
        foreach(var set in holes)
        {
            if(set.Count % 4 != 0)
            {
                return true;
            }
        }
        return false;
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
