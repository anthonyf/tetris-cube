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
        var placedPieces = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllIBeamPieces(), 
            (piece, pos) =>
            {
                // do nothing  
            }).First();

        Assert.Pass();
	}

    [Test]
    public void TestSolveLPieces()
    {
        var placedPieces = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllLPieces(),
            (piece, pos) =>
            {
                // do nothing  
            }).First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveAustinPieces()
    {
        var placedPieces = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAustinPuzzlePieces(),
            (piece, pos) =>
            {
                // do nothing  
            }).First();

        Assert.Pass();
    }

    [Test]
    public void TestSolveNormalTetrisPieces()
    {
        var placedPieces = TetrisCubeSolver.Solve(TetrisCubeSolver.SpawnAllPuzzlePieces(),
            (piece, pos) =>
            {
                // do nothing  
            }).First();

        Assert.Pass();
    }
}
