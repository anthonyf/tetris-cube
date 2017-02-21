using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieces : MonoBehaviour {

    [SerializeField]
    PuzzlePiece puzzlePiecePrefab;

    public List<PuzzlePiece> puzzlePieces;

    // Use this for initialization
    void Start () {
        puzzlePieces = new List<PuzzlePiece>();
        puzzlePieces.AddRange(MakePuzzlePieces());
        puzzlePieces.AddRange(MakePuzzlePieces());
    }

    // Update is called once per frame
    void Update () {
		
	}

    List<PuzzlePiece> MakePuzzlePieces()
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
}

