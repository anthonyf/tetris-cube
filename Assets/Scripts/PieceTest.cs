using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTest : MonoBehaviour {

    [SerializeField]
    PuzzlePiece puzzlePiecePrefab;

    [SerializeField]
    PuzzlePieceTypes type;

    PuzzlePiece _puzzlePiece;

	// Use this for initialization
	void Start () {
        _puzzlePiece = Instantiate(puzzlePiecePrefab);
        _puzzlePiece.transform.SetParent(transform, false);
        _puzzlePiece.type = type;
	}

    public void RotateZ()
    {
        _puzzlePiece.Rotate(Axis.Z);
    }
    public void RotateZ2()
    {
        _puzzlePiece.Rotate(Axis.Z2);
    }
    public void RotateY()
    {
        _puzzlePiece.Rotate(Axis.Y);
    }
    public void RotateX()
    {
        _puzzlePiece.Rotate(Axis.X);
    }
    public void RotateX2()
    {
        _puzzlePiece.Rotate(Axis.X2);
    }
}
