﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TetrisCube : MonoBehaviour {

    [SerializeField]
    Text startButtonText;

    [SerializeField]
    PuzzlePiece puzzlePiecePrefab;

    [SerializeField]
    Transform PiecesContainer;

    [SerializeField]
    Transform CubeContainer;

    [SerializeField]
    Transform OuterCubeContainer;

    public List<PuzzlePiece> puzzlePieces;


    bool isDragging = false;
    float rotationSpeed = 8f;
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            var rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            var rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
            OuterCubeContainer.transform.Rotate(Vector3.up, -rotX, Space.World);
            OuterCubeContainer.transform.Rotate(Vector3.right, rotY, Space.World);
            
        } else
        {
            isDragging = false;
        }
    }

    // Use this for initialization
    void Start () {
        puzzlePieces = new List<PuzzlePiece>();
        puzzlePieces.AddRange(SpawnAllPuzzlePieces());
        puzzlePieces.AddRange(SpawnAllPuzzlePieces());

        PlacePiecesInACircle();
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

    PuzzlePiece SpawnPuzzlePiece(PuzzlePieceTypes type)
    {
        var p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(PiecesContainer.transform, false);
        p.type = type;
        return p;
    }

    List<PuzzlePiece> SpawnAllPuzzlePieces()
    {
        var pieces = new List<PuzzlePiece>();

        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.IBeam));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Box));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Axis));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.L));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Bump));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.S));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Helix));
        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.ReverseHelix));

        return pieces;
    }

    float moveSpeed = .0f;

    IEnumerator Solve(IEnumerable<PuzzlePiece> unplacedPieces, IEnumerable<PuzzlePiece> placedPieces, PuzzleGrid grid, Action<IEnumerable<PuzzlePiece>> solvedFun)
    {
        if(unplacedPieces.Count() == 0)
        {
            solvedFun(placedPieces);
            yield break;
        }

        var piece = unplacedPieces.First();
        var prevRotation = piece.transform.localEulerAngles;
        var prevPosition = piece.transform.localPosition;
        var savedRotation = piece.transform.localEulerAngles;
        var savedPosition = piece.transform.localPosition;
        foreach (var validPosition in piece.ValidBoardPositions())
        {

            // place piece
            piece.transform.SetParent(CubeContainer.transform, true);
            piece.transform.localEulerAngles = validPosition.eulerAngle;
            piece.transform.localPosition = validPosition.position;
            if (grid.IsValidMove(piece))
            {
                grid.AddPiece(piece);

                yield return StartCoroutine(MovePiece(piece, prevPosition, prevRotation, validPosition.position, validPosition.eulerAngle, moveSpeed));
                yield return StartCoroutine(Solve(unplacedPieces.Skip(1), placedPieces.Concat(new PuzzlePiece[] { piece }), grid, solvedFun));
                prevRotation = validPosition.eulerAngle;
                prevPosition = validPosition.position;
                grid.RemovePiece(piece);
            }
        }
        piece.transform.SetParent(PiecesContainer.transform, true);
        yield return StartCoroutine(MovePiece(piece, prevPosition, prevRotation, savedPosition, savedRotation, moveSpeed));

        yield return null;
    }

    IEnumerator MovePiece(PuzzlePiece piece, Vector3 fromPosition, Vector3 fromRotation, Vector3 toPosition, Vector3 toRotation, float speed)
    {
        if (speed == 0f)
        {
            piece.transform.localPosition = toPosition;
            piece.transform.localEulerAngles = toRotation;
            yield break;
        }

        var elapsedTime = 0f;

        while(speed > elapsedTime)
        {
            piece.transform.localPosition = Vector3.Lerp(fromPosition, toPosition, elapsedTime / speed);
            piece.transform.localEulerAngles = Vector3.Lerp(fromRotation, toRotation, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        piece.transform.localPosition = toPosition;
        piece.transform.localEulerAngles = toRotation;
    }

    bool running = false;

    public void StartButtonClicked()
    {
        if(running)
        {
            running = false;
            StopAllCoroutines();
        }
        else
        {
            running = true;
            startButtonText.text = "Stop";
            StartCoroutine(Solve(puzzlePieces.Select(p => p), new List<PuzzlePiece>(), new PuzzleGrid(), solution =>
            {
                Debug.Log("Solved!");
                StopAllCoroutines();
            }));
        }
    }

    public void StepButtonClicked()
    {

    }
}

