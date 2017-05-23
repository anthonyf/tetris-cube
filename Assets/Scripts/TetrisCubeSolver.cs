using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrisCubeSolver {

    public const int BOARD_SIZE = 4;

    public static List<TetrisPuzzlePiece> CreateAllIBeamPieces()
    {
        var pieces = new List<TetrisPuzzlePiece>();

        for (int i = 0; i < 16; i++)
        {
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.IBeam));
        }
        return pieces;
    }

    public static List<TetrisPuzzlePiece> CreateAllLPieces()
    {
        var pieces = new List<TetrisPuzzlePiece>();

        for (int i = 0; i < 16; i++)
        {
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.L));
        }
        return pieces;
    }

    public static List<TetrisPuzzlePiece> CreateAustinPuzzlePieces()
    {
        var pieces = new List<TetrisPuzzlePiece>();

        for (int i = 0; i < 3; i++)
        {
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.IBeam));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Box));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.L));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Bump));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.S));
        }

        pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Axis));

        return pieces;
    }

    public static List<TetrisPuzzlePiece> CreateAllPuzzlePieces()
    {
        var pieces = new List<TetrisPuzzlePiece>();

        for (int i = 0; i < 2; i++)
        {
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.IBeam));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Box));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Axis));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.L));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Bump));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.S));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.Helix));
            pieces.Add(new TetrisPuzzlePiece(PuzzlePieceTypes.ReverseHelix));
        }

        return pieces;
    }

    public class SolveStep
    {
        public enum StepType
        {
            AddPiece,
            RemovePiece,
            Solved
        }
        public StepType type { get; private set; }
        public IEnumerable<PuzzlePiecePosition> positions { get; private set; }
        public SolveStep(StepType type, IEnumerable<PuzzlePiecePosition> positions)
        {
            this.type = type;
            this.positions = positions;
        }
    }

    public static IEnumerable<SolveStep> Solve(
        List<TetrisPuzzlePiece> pieces, bool includeSteps)
    {
        return SolveHoles(pieces, new List<PuzzlePiecePosition>(), new PuzzleGrid(), includeSteps);        
        //return SolveBruteForce(pieces, new List<PuzzlePiecePosition>(), new PuzzleGrid(), includeSteps);
    }

    static IEnumerable<SolveStep> SolveBruteForce(
        IEnumerable<TetrisPuzzlePiece> unplacedPieces,
        IEnumerable<PuzzlePiecePosition> placedPieces,
        PuzzleGrid grid,
        bool includeSteps)
    {
        if (unplacedPieces.Count() == 0)
        {
            // solved
            yield return new SolveStep(SolveStep.StepType.Solved, placedPieces);
        }

        var piece = unplacedPieces.First();
        foreach (var validPosition in piece.ValidBoardPositions())
        {
            if (grid.IsValidMove(validPosition.blockPositions))
            {
                grid.AddPiece(validPosition.blockPositions);
                if (grid.IsPuzzleSolvable(grid.FindHoles()) /*grid.HasOnlyOneHole(grid.FindHoles())*/)
                {
                    var newPlacedPieces = placedPieces.Concat(new PuzzlePiecePosition[] { validPosition });
                    if (includeSteps) yield return new SolveStep(SolveStep.StepType.AddPiece, newPlacedPieces);
                    var e = SolveBruteForce(unplacedPieces.Where(p => p != piece), newPlacedPieces, grid, includeSteps);
                    foreach (var s in e)
                    {
                        yield return s;
                    }
                    if (includeSteps) yield return new SolveStep(SolveStep.StepType.RemovePiece, newPlacedPieces);
                }
                grid.RemovePiece(validPosition.blockPositions);
            }
        }
    }


    static IEnumerable<SolveStep> SolveHoles(
        IEnumerable<TetrisPuzzlePiece> unplacedPieces, 
        IEnumerable<PuzzlePiecePosition> placedPieces, 
        PuzzleGrid grid,
        bool includeSteps)
    {
        var holes = grid.FindHoles();
        if (!grid.IsPuzzleSolvable(holes))
        {
            yield break;
        }

        if (holes.Count() == 0)
        {
            // solved
            yield return new SolveStep(SolveStep.StepType.Solved, placedPieces);
        }
        var hole = holes.First();
        foreach (var piece in unplacedPieces)
        {
            foreach (var validPosition in piece.ValidBoardPositions())
            {
                if (hole.IsSupersetOf(validPosition.blockPositions))
                {
                    grid.AddPiece(validPosition.blockPositions);
                    var newPlacedPieces = placedPieces.Concat(new PuzzlePiecePosition[] { validPosition });
                    if (includeSteps) yield return new SolveStep(SolveStep.StepType.AddPiece, newPlacedPieces);
                    var e = SolveHoles(unplacedPieces.Where(p => p != piece), newPlacedPieces, grid, includeSteps);
                    foreach(var s in e)
                    {
                        yield return s;
                    }
                    if (includeSteps) yield return new SolveStep(SolveStep.StepType.RemovePiece, newPlacedPieces);
                    grid.RemovePiece(validPosition.blockPositions);
                }
            }
        }
    }
}

public enum PuzzlePieceTypes
{
    IBeam,
    Box,
    L,
    Axis,
    Bump,
    S,
    Helix,
    ReverseHelix,
}

public class TetrisPuzzlePieceBlock
{
    public TetrisPuzzlePiece piece { get; private set; }
    public IntVector3 position { get; private set; }

    public TetrisPuzzlePieceBlock(TetrisPuzzlePiece piece, IntVector3 position)
    {
        this.piece = piece;
        this.position = position;
    }
}

public class TetrisPuzzlePiece
{
    public PuzzlePieceTypes type { get; private set; }
    public List<TetrisPuzzlePieceBlock> blocks { get; private set; }
    public Color color { get; private set; }
    List<PuzzlePiecePosition> _cachedValidBoardPositions = null;

    public TetrisPuzzlePiece(PuzzlePieceTypes type)
    {
        List<IntVector3> blocks;
        switch (type)
        {
            case PuzzlePieceTypes.IBeam:
                color = Color.cyan;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(3, 0, 0)};
                break;
            case PuzzlePieceTypes.Box:
                color = Color.yellow;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(0, 0, 0) };
                break;
            case PuzzlePieceTypes.Axis:
                color = Color.green;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 0, 1),
                    new IntVector3(1, 1, 0) };
                break;
            case PuzzlePieceTypes.L:
                color = Color.blue;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(2, 1, 0) };
                break;
            case PuzzlePieceTypes.S:
                color = Color.red;
                blocks = new List<IntVector3>() {
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0) };
                break;
            case PuzzlePieceTypes.Bump:
                color = Color.white;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(2, 1, 0),
                    new IntVector3(1, 0, 0) };
                break;
            case PuzzlePieceTypes.Helix:
                color = Color.magenta;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(1, 1, 1) };
                break;
            case PuzzlePieceTypes.ReverseHelix:
                color = Color.gray;
                blocks = new List<IntVector3>() {
                    new IntVector3(0, 0, 0),
                    new IntVector3(0, 0, 1),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 1, 0) };
                break;
            default:
                throw new NotImplementedException("Unsupported puzzle piece type");
        }
        // adjust block position so it rotates in the center
        var minV = new IntVector3(TetrisCubeSolver.BOARD_SIZE, TetrisCubeSolver.BOARD_SIZE, TetrisCubeSolver.BOARD_SIZE);
        var maxV = new IntVector3(0, 0, 0);
        foreach(var v in blocks)
        {
            minV = new IntVector3(Mathf.Min(minV.x, v.x), Mathf.Min(minV.y, v.y), Mathf.Min(minV.z, v.z));
            maxV = new IntVector3(Mathf.Max(maxV.x, v.x), Mathf.Max(maxV.y, v.y), Mathf.Max(maxV.z, v.z));
        }
        this.blocks = blocks.Select(v => {
            var normalizedPosition = new Vector3(v.x - (maxV.x - minV.x) / 2, v.y - (maxV.y - minV.y) / 2, v.z - (maxV.z - minV.z) / 2);
            return new TetrisPuzzlePieceBlock(this, normalizedPosition.ToIntVector3());
        }).ToList();
    }

    public List<PuzzlePiecePosition> ValidBoardPositions()
    {
        if (_cachedValidBoardPositions == null)
        {
            _cachedValidBoardPositions = CalculateValidBoardPositions(blocks.Select(b => b.position).ToList());
        }
        return _cachedValidBoardPositions;
    }

    private List<PuzzlePiecePosition> CalculateValidBoardPositions(List<IntVector3> blocks)
    {
        List<PuzzlePiecePosition> validPostions = new List<PuzzlePiecePosition>();
        PuzzleGrid grid = new PuzzleGrid();

        // for all 24 rotations rotation
        foreach (var rotation in TwentyFourRotations.rotations)
        {
            var blockRotations = TwentyFourRotations.RotatePoints(blocks, rotation).ToList();
            // for each x position
            var outsideSpaces = TetrisCubeSolver.BOARD_SIZE - 1;
            for (int x = -outsideSpaces; x < TetrisCubeSolver.BOARD_SIZE + outsideSpaces; x++)
            {
                // for each y position
                for (int y = -outsideSpaces; y < TetrisCubeSolver.BOARD_SIZE + outsideSpaces; y++)
                {
                    // for each z position
                    for (int z = -outsideSpaces; z < TetrisCubeSolver.BOARD_SIZE + outsideSpaces; z++)
                    {
                        var piecePosition = new IntVector3(x, y, z);
                        var blockLocations = blockRotations.Select(p => (p.ToVector3() + piecePosition.ToVector3()).ToIntVector3()).ToList();

                        if (grid.IsValidBoardPosition(blockLocations.ToList()))
                        {
                            // eliminate duplicate orientations
                            var shouldAdd = true;
                            var blockSetA = new HashSet<IntVector3>(blockLocations);
                            foreach (var position in validPostions)
                            {
                                var blockSetB = new HashSet<IntVector3>(position.blockPositions);
                                if (blockSetA.SetEquals(blockSetB))
                                {
                                    shouldAdd = false;
                                    break;
                                }
                            }
                            if (shouldAdd)
                            {
                                validPostions.Add(new PuzzlePiecePosition()
                                {
                                    puzzlePiece = this,
                                    eulerAngle = rotation,
                                    position = piecePosition,
                                    blockPositions = blockLocations
                                });
                            }
                        }
                    }
                }
            }
        }

        return validPostions;
    }
}