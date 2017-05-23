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

        //puzzlePieces.AddRange(SpawnPuzzlePieces(TetrisCubeSolver.CreateAllLPieces()));
        puzzlePieces.AddRange(SpawnPuzzlePieces(TetrisCubeSolver.CreateAllPuzzlePieces()));

        PlacePiecesInACircle();
    }

    private void PlacePiecesInACircle()
    {
        for(int i = 0; i < puzzlePieces.Count; i++)
        {
            var radius = 8f;
            var angle = i * Mathf.PI * 2f / puzzlePieces.Count;
            var position = new IntVector3(
                (int)Mathf.Round(Mathf.Cos(angle) * radius),
                (int)Mathf.Round(Mathf.Sin(angle) * radius), 0).ToVector3();
            puzzlePieces[i].transform.localPosition = position;
            puzzlePieces[i].initialPosition = position;
            puzzlePieces[i].initialRotation = Vector3.zero;
        }
    }

    private IEnumerable<PuzzlePiece> SpawnPuzzlePieces(IEnumerable<TetrisPuzzlePiece> pieces)
    {
        return pieces.Select(p => SpawnPuzzlePiece(p));
    }

    PuzzlePiece SpawnPuzzlePiece(TetrisPuzzlePiece piece)
    {
        var p = Instantiate(puzzlePiecePrefab);
        p.transform.SetParent(PiecesContainer.transform, false);
        p.puzzlePiece = piece;
        return p;
    }

    IEnumerator SolveCoroutine()
    {
        var steps = TetrisCubeSolver.Solve(puzzlePieces.Select(p => p.puzzlePiece).ToList(), true);
        foreach(var step in steps)
        {
            PuzzlePiecePosition position;
            PuzzlePiece puzzlePieceGO;
            switch(step.type)
            {
                case TetrisCubeSolver.SolveStep.StepType.AddPiece:
                    position = step.positions.Last();
                    puzzlePieceGO = puzzlePieces.Where(p => p.puzzlePiece == position.puzzlePiece).First();
                    puzzlePieceGO.transform.SetParent(CubeContainer.transform, true);
                    yield return StartCoroutine(
                        MovePiece(puzzlePieceGO,
                            puzzlePieceGO.transform.localPosition, puzzlePieceGO.transform.localEulerAngles,
                            position.position.ToVector3(), position.eulerAngle.ToVector3(), 
                            moveSpeeds[moveSpeedIndex]));
                    break;
                case TetrisCubeSolver.SolveStep.StepType.RemovePiece:
                    position = step.positions.Last();
                    puzzlePieceGO = puzzlePieces.Where(p => p.puzzlePiece == position.puzzlePiece).First();
                    puzzlePieceGO.transform.SetParent(PiecesContainer.transform, true);
                    yield return StartCoroutine(
                        MovePiece(puzzlePieceGO,
                            puzzlePieceGO.transform.localPosition, puzzlePieceGO.transform.localEulerAngles, 
                            puzzlePieceGO.initialPosition, puzzlePieceGO.initialRotation,
                            moveSpeeds[moveSpeedIndex]));
                    break;
                case TetrisCubeSolver.SolveStep.StepType.Solved:
                    yield break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }


    static IEnumerator MovePiece(PuzzlePiece piece, Vector3 fromPosition, Vector3 fromRotation, Vector3 toPosition, Vector3 toRotation, float speed)
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
            StartCoroutine(SolveCoroutine());
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

