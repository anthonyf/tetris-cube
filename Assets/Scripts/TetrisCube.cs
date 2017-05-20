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
    Text speedButtonText;

    [SerializeField]
    PuzzlePiece puzzlePiecePrefab;

    [SerializeField]
    Transform PiecesContainer;

    [SerializeField]
    Transform CubeContainer;

    [SerializeField]
    Transform OuterCubeContainer;

    public List<PuzzlePiece> puzzlePieces;


    int moveSpeedIndex = 0;
    float[] moveSpeeds = { 1f, .5f, .25f, .125f, 0f };
    string[] moveSpeedText = { "1x", "2x", "3x", "4x", "InfinityX" };

    bool isDragging = false;
    float rotationSpeed = 8f;

    bool running = false;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float pointerX = Input.GetAxis("Mouse X");
            float pointerY = Input.GetAxis("Mouse Y");
            if (Input.touchCount > 0)
            {
                pointerX = Input.touches[0].deltaPosition.x * .05f;
                pointerY = Input.touches[0].deltaPosition.y * .05f;
            }
            var rotX = pointerX * rotationSpeed;
            var rotY = pointerY * rotationSpeed;
            OuterCubeContainer.transform.Rotate(Vector3.up, -rotX, Space.World);
            OuterCubeContainer.transform.Rotate(Vector3.right, rotY, Space.World);
            
        } else
        {
            isDragging = false;
        }
    }

    // Use this for initialization
    void Start () {
        Restart();
    }

    public void Restart()
    {
        speedButtonText.text = moveSpeedText[moveSpeedIndex];
        startButtonText.text = "Start";
        if (running)
        {
            StopAllCoroutines();
            running = false;
        }
        if(puzzlePieces != null)
        {
            puzzlePieces.ForEach(p => Destroy(p.gameObject));
        }
        puzzlePieces = new List<PuzzlePiece>();

        puzzlePieces.AddRange(SpawnAllPuzzlePieces());
        //puzzlePieces.AddRange(SpawnAustinPuzzlePieces());
        //puzzlePieces.AddRange(SpawnAllIBeamPieces());

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

    List<PuzzlePiece> SpawnAllIBeamPieces()
    {
        var pieces = new List<PuzzlePiece>();

        for(int i = 0; i < 16; i++)
        {
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.IBeam));
        }
        return pieces;
    }

    List<PuzzlePiece> SpawnAustinPuzzlePieces()
    {
        var pieces = new List<PuzzlePiece>();

        for(int i = 0; i < 3; i++)
        {
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.IBeam));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Box));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.L));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Bump));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.S));
        }

        pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Axis));

        return pieces;
    }

    List<PuzzlePiece> SpawnAllPuzzlePieces()
    {
        var pieces = new List<PuzzlePiece>();

        for (int i = 0; i < 2; i++)
        {
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.IBeam));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Box));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Axis));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.L));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Bump));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.S));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.Helix));
            pieces.Add(SpawnPuzzlePiece(PuzzlePieceTypes.ReverseHelix));
        }

        return pieces;
    }

    IEnumerator SolveHoles(IEnumerable<PuzzlePiece> unplacedPieces, IEnumerable<PuzzlePiece> placedPieces, PuzzleGrid grid, Action<IEnumerable<PuzzlePiece>> solvedFun)
    {
        var holes = grid.FindHoles();
        if(!grid.IsPuzzleSolvable(holes))
        {
            // nothing to do, puzzle is in unsolvable state
            yield return null;
        }
        if(holes.Count() == 0)
        {
            solvedFun(placedPieces);
            yield break;
        }
        var hole = holes.First();
        foreach (var piece in unplacedPieces)
        {
            var savedOriginalRotation = piece.transform.localEulerAngles.ToIntVector3();
            var savedOriginalPosition = piece.transform.localPosition.ToIntVector3();
            piece.transform.SetParent(CubeContainer.transform, true);

            foreach (var validPosition in piece.ValidBoardPositions())
            {                
                if (hole.IsSupersetOf(validPosition.blockPositions))
                {
                    grid.AddPiece(validPosition.blockPositions);

                    // animate movement only when we found a valid move
                    yield return StartCoroutine(MovePiece(piece, piece.transform.localPosition.ToIntVector3(), piece.transform.localEulerAngles.ToIntVector3(), validPosition.position,
                        validPosition.eulerAngle, moveSpeeds[moveSpeedIndex]));
                    yield return StartCoroutine(SolveHoles(unplacedPieces.Where(p => p != piece), placedPieces.Concat(new PuzzlePiece[] { piece }), grid, solvedFun));

                    grid.RemovePiece(validPosition.blockPositions);
                }
            }

            // Done trying to fit this piece, put piece back in the ring where it came from
            piece.transform.SetParent(PiecesContainer.transform, true);
            yield return StartCoroutine(MovePiece(piece, piece.transform.localPosition.ToIntVector3(), piece.transform.localEulerAngles.ToIntVector3(),
                savedOriginalPosition, savedOriginalRotation, moveSpeeds[moveSpeedIndex]));
        }
    }

    IEnumerator Solve(IEnumerable<PuzzlePiece> unplacedPieces, IEnumerable<PuzzlePiece> placedPieces, PuzzleGrid grid, Action<IEnumerable<PuzzlePiece>> solvedFun)
    {
        if(unplacedPieces.Count() == 0)
        {
            solvedFun(placedPieces);
            yield break;
        }

        var piece = unplacedPieces.First();
        var savedOriginalRotation = piece.transform.localEulerAngles.ToIntVector3();
        var savedOriginalPosition = piece.transform.localPosition.ToIntVector3();
        piece.transform.SetParent(CubeContainer.transform, true);
        foreach (var validPosition in piece.ValidBoardPositions())
        {
            if (grid.IsValidMove(validPosition.blockPositions))
            {
                grid.AddPiece(validPosition.blockPositions);
                if(grid.IsPuzzleSolvable())
                {
                    // animate movement only when we found a valid move
                    yield return StartCoroutine(MovePiece(piece, piece.transform.localPosition.ToIntVector3(), piece.transform.localEulerAngles.ToIntVector3(), validPosition.position,
                        validPosition.eulerAngle, moveSpeeds[moveSpeedIndex]));
                    yield return StartCoroutine(Solve(unplacedPieces.Skip(1), placedPieces.Concat(new PuzzlePiece[] { piece }), grid, solvedFun));
                }
                grid.RemovePiece(validPosition.blockPositions);
            }
        }

        // Done trying to fit this piece, put piece back in the ring where it came from
        piece.transform.SetParent(PiecesContainer.transform, true);
        yield return StartCoroutine(MovePiece(piece, piece.transform.localPosition.ToIntVector3(), piece.transform.localEulerAngles.ToIntVector3(), 
            savedOriginalPosition, savedOriginalRotation, moveSpeeds[moveSpeedIndex]));
    }

    IEnumerator MovePiece(PuzzlePiece piece, IntVector3 _fromPosition, IntVector3 _fromRotation, IntVector3 _toPosition, IntVector3 _toRotation, float speed)
    {
        var toPosition = _toPosition.ToVector3();
        var toRotation = _toRotation.ToVector3();
        var fromRotation = _fromRotation.ToVector3();
        var fromPosition = _fromPosition.ToVector3();

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

    public void StartButtonClicked()
    {
        if(running)
        {
            Restart();
        }
        else
        {
            running = true;
            startButtonText.text = "Stop";
            StartCoroutine(SolveHoles(puzzlePieces.Select(p => p), new List<PuzzlePiece>(), new PuzzleGrid(), solution =>
            //StartCoroutine(Solve(puzzlePieces.Select(p => p), new List<PuzzlePiece>(), new PuzzleGrid(), solution =>
            {
                Debug.Log("Solved!");
                StopAllCoroutines();
            }));
        }
    }

    public void StepButtonClicked()
    {

    }

    public void SpeedButtonClicked()
    {
        moveSpeedIndex = (moveSpeedIndex + 1) % moveSpeeds.Count();
        speedButtonText.text = moveSpeedText[moveSpeedIndex];
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}

