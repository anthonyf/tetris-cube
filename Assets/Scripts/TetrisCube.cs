using System;
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
            piece.transform.SetParent(CubeContainer.transform, true);
            piece.transform.localEulerAngles = validPosition.eulerAngle;
            piece.transform.localPosition = validPosition.position;
            if (grid.IsValidMove(piece))
            {
                grid.AddPiece(piece);
                yield return StartCoroutine(Solve(unplacedPieces.Skip(1), placedPieces.Concat(new PuzzlePiece[] { piece }), grid, solvedFun));
                grid.RemovePiece(piece);
            }
        }
        piece.transform.SetParent(PiecesContainer.transform, true);
        piece.transform.localPosition = savedPosition;
        piece.transform.localEulerAngles = savedRotation;

        yield return null;
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

