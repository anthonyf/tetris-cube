using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;

public class TetrisCubeSolverTest
{

	[Test]
	public void TestSolveIBeams() {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllIBeamPieces())
            .Select(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
	}

    [Test]
    public void TestSolveLPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllLPieces())
            .Select(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveAustinPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAustinPuzzlePieces())
            .Select(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveNormalTetrisPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllPuzzlePieces())
            .Select(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }
}
