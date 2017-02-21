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
        p.Initialize(Color.cyan, new IntVector3[4] {
            new IntVector3(0, 0, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(2, 0, 0),
            new IntVector3(3, 0, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.yellow, new IntVector3[4] {
            new IntVector3(0, 1, 0),
            new IntVector3(1, 1, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(0, 0, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.green, new IntVector3[4] {
            new IntVector3(0, 0, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(1, 0, 1),
            new IntVector3(1, 1, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.blue, new IntVector3[4] {
            new IntVector3(0, 0, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(2, 0, 0),
            new IntVector3(2, 1, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.black, new IntVector3[4] {
            new IntVector3(0, 1, 0),
            new IntVector3(1, 1, 0),
            new IntVector3(2, 1, 0),
            new IntVector3(1, 0, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.red, new IntVector3[4] {
            new IntVector3(1, 0, 0),
            new IntVector3(2, 0, 0),
            new IntVector3(0, 1, 0),
            new IntVector3(1, 1, 0) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.magenta, new IntVector3[4] {
            new IntVector3(0, 0, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(1, 1, 0),
            new IntVector3(1, 1, 1) });
        pieces.Add(p);

        p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(transform, false);
        p.Initialize(Color.gray, new IntVector3[4] {
            new IntVector3(0, 0, 0),
            new IntVector3(0, 0, 1),
            new IntVector3(1, 0, 0),
            new IntVector3(1, 1, 0) });
        pieces.Add(p);

        return pieces;
    }
}

