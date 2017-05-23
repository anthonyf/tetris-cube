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
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.CreateAllIBeamPieces(), true)
            .Where(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
	}

    [Test]
    public void TestSolveLPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.CreateAllLPieces(), true)
            .Where(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveAustinPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.CreateAustinPuzzlePieces(), true)
            .Where(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveNormalTetrisPieces()
    {
        var steps = TetrisCubeSolver.Solve(TetrisCubeSolver.CreateAllPuzzlePieces(), true)
            .Where(s => s.type == TetrisCubeSolver.SolveStep.StepType.Solved)
            .First();

        Assert.Pass();
    }
}
