using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PuzzleGrid
{
    bool[,,] grid = new bool[TetrisCubeSolver.BOARD_SIZE,TetrisCubeSolver.BOARD_SIZE,TetrisCubeSolver.BOARD_SIZE];

    public bool IsValidBoardPosition(List<IntVector3> blocks)
    {
        return blocks.TrueForAll(b =>
            b.x >= 0 &&
            b.y >= 0 &&
            b.z >= 0 &&
            b.x < TetrisCubeSolver.BOARD_SIZE &&
            b.y < TetrisCubeSolver.BOARD_SIZE &&
            b.z < TetrisCubeSolver.BOARD_SIZE);
    }

    public bool IsValidMove(List<IntVector3> blocks)
    {
        return IsValidBoardPosition(blocks) && 
               blocks.ToList().TrueForAll(b => IsCellEmpty(b));
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
                c.x < TetrisCubeSolver.BOARD_SIZE &&
                c.y >= 0 &&
                c.y < TetrisCubeSolver.BOARD_SIZE &&
                c.z >= 0 && 
                c.z < TetrisCubeSolver.BOARD_SIZE);
    }

    bool IsCellEmpty(IntVector3 v)
    {
        return !grid[v.x, v.y, v.z];
    }

    IEnumerable<IntVector3> CellIndexes()
    {
        for (int x = 0; x < TetrisCubeSolver.BOARD_SIZE; x++)
        {
            for (int y = 0; y < TetrisCubeSolver.BOARD_SIZE; y++)
            {
                for (int z = 0; z < TetrisCubeSolver.BOARD_SIZE; z++)
                {
                    yield return new IntVector3(x, y, z);
                }
            }
        }
    }

    public IEnumerable<HashSet<IntVector3>> FindHoles()
    {
        var visited = new HashSet<IntVector3>();
        foreach (var currentCell in CellIndexes())
        {
            if(!visited.Contains(currentCell) && IsCellEmpty(currentCell))
            {
                var hole = FindHoleStartingAt(currentCell, visited);
                yield return hole;
            }
        }
    }

    HashSet<IntVector3> FindHoleStartingAt(IntVector3 start, HashSet<IntVector3> visited)
    {
        var hole = new HashSet<IntVector3>();
        hole.Add(start);
        visited.Add(start);
        foreach (var neighborCell in AdjacentCells(start))
        {
            if(!visited.Contains(neighborCell) && IsCellEmpty(neighborCell))
            {
                foreach(var i in FindHoleStartingAt(neighborCell, visited))
                {
                    hole.Add(i);
                }
            }
        }
        return hole;
    }

    /// <summary>
    /// Determines if a puzzle is in a state where it is solvable.  A puzzle is
    /// unsolvable if any contiguous empty spaces (holes) are not divisible by 
    /// TetrisCubeSolver.BOARD_SIZE.
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
            if (set.Count % TetrisCubeSolver.BOARD_SIZE != 0)
            {
                return false;
            }
        }
        return true;
    }

    public void AddPiece(List<IntVector3> blocks)
    {
        blocks.ForEach(b => grid[b.x, b.y, b.z] = true);
    }    

    public void RemovePiece(List<IntVector3> blocks)
    {
        blocks.ForEach(b => grid[b.x, b.y, b.z] = false);
    }
}
