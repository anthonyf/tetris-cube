using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TetrisCube : MonoBehaviour {

    [SerializeField]
    PuzzlePiece puzzlePiecePrefab;

    public List<PuzzlePiece> puzzlePieces;

    // Use this for initialization
    IEnumerator Start () {
        puzzlePieces = new List<PuzzlePiece>();
        puzzlePieces.AddRange(SpawnAllPuzzlePieces());
        puzzlePieces.AddRange(SpawnAllPuzzlePieces());

        PlacePiecesInACircle();

        yield return null; yield return null;
        StartCoroutine(Solve(puzzlePieces.Select(p => p), new List<PuzzlePiece>(), new PuzzleGrid(), solution =>
        {
            Debug.Log("Solved!");
            StopAllCoroutines();
        }));
    }

    private void PlacePiecesInACircle()
    {
        for(int i = 0; i < puzzlePieces.Count; i++)
        {
            var radius = 8f;
            var angle = i * Mathf.PI * 2f / puzzlePieces.Count;
            puzzlePieces[i].transform.localPosition = new IntVector3(
                (int)Mathf.Round(Mathf.Cos(angle) * radius), 
                (int)Mathf.Round(Mathf.Sin(angle) * radius), 0).ToVector3();
        }
    }

    List<PuzzlePiece> SpawnAllPuzzlePieces()
    {
        var pieces = new List<PuzzlePiece>();

        var p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.IBeam;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.Box;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.Axis;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.L;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.Bump;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.S;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.Helix;
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.type = PuzzlePieceTypes.ReverseHelix;
        pieces.Add(p);

        return pieces;
    }

    IEnumerator Solve(IEnumerable<PuzzlePiece> unplacedPieces, IEnumerable<PuzzlePiece> placedPieces, PuzzleGrid grid, Action<IEnumerable<PuzzlePiece>> solvedFun)
    {
        if(unplacedPieces.Count() == 0)
        {
            solvedFun(placedPieces);
            yield break;
        }

        var piece = unplacedPieces.First();
        var savedRotation = piece.transform.localEulerAngles;
        var savedPosition = piece.transform.localPosition;
        foreach (var validPosition in piece.ValidBoardPositions())
        {
            // place piece
            piece.transform.localEulerAngles = validPosition.eulerAngle;
            piece.transform.localPosition = validPosition.position;
            if (grid.IsValidMove(piece))
            {
                grid.AddPiece(piece);
                yield return StartCoroutine(Solve(unplacedPieces.Skip(1), placedPieces.Concat(new PuzzlePiece[] { piece }), grid, solvedFun));
                grid.RemovePiece(piece);
            }
        }
        piece.transform.localPosition = savedPosition;
        piece.transform.localEulerAngles = savedRotation;

        yield return null;
    }
}

