using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum PuzzlePieceTypes
{
    IBeam,
    Box,
    L,
    Axis,
    Bump,
    S,
    Helix,
    ReverseHelix,
}

public class PuzzlePiece : MonoBehaviour {

    [SerializeField]
    Block blockPrefab;

    [SerializeField]
    public PuzzlePieceTypes type;

    List<Block> blocks = new List<Block>();

	// Use this for initialization
	void Start () {
        MakePuzzlePiece(type);
	}
	
    private void Initialize(Color color, IntVector3[] blockPositions)
    {
        foreach(var v in blockPositions)
        {
            var b = Instantiate(blockPrefab);
            b.transform.SetParent(transform, false);
            b.SetPosition(v);
            b.color = color;
            blocks.Add(b);
        }
    }

    private void MakePuzzlePiece(PuzzlePieceTypes type)
    {
        switch (type)
        {
            case PuzzlePieceTypes.IBeam:
                Initialize(Color.cyan, new IntVector3[4] {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(3, 0, 0) });
                break;
            case PuzzlePieceTypes.Box:
                Initialize(Color.yellow, new IntVector3[4] {
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(0, 0, 0) });
                break;
            case PuzzlePieceTypes.Axis:
                Initialize(Color.green, new IntVector3[4] {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 0, 1),
                    new IntVector3(1, 1, 0) });
                break;
            case PuzzlePieceTypes.L:
                Initialize(Color.blue, new IntVector3[4] {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(2, 1, 0) });
                break;
            case PuzzlePieceTypes.S:
                Initialize(Color.red, new IntVector3[4] {
                    new IntVector3(1, 0, 0),
                    new IntVector3(2, 0, 0),
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0) });
                break;
            case PuzzlePieceTypes.Bump:
                Initialize(Color.black, new IntVector3[4] {
                    new IntVector3(0, 1, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(2, 1, 0),
                    new IntVector3(1, 0, 0) });
                break;
            case PuzzlePieceTypes.Helix:
                Initialize(Color.magenta, new IntVector3[4] {
                    new IntVector3(0, 0, 0),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 1, 0),
                    new IntVector3(1, 1, 1) });
                break;
            case PuzzlePieceTypes.ReverseHelix:
                Initialize(Color.gray, new IntVector3[4] {
                    new IntVector3(0, 0, 0),
                    new IntVector3(0, 0, 1),
                    new IntVector3(1, 0, 0),
                    new IntVector3(1, 1, 0) });
                break;
        }
    }

    private void Normalize()
    {
        int minX = 4;
        int minY = 4;
        int minZ = 4;
        blocks.ForEach(b =>
        {
            minX = Math.Min(b.position.x, minX);
            minY = Math.Min(b.position.y, minY);
            minZ = Math.Min(b.position.z, minZ);
        });
        blocks.ForEach(b => b.SetPosition(new IntVector3(
            b.position.x - minX,
            b.position.y - minY,
            b.position.z - minZ)));
    }

    public void Rotate(Axis axis)
    {
        blocks.ForEach(b => b.Rotate(axis));
        Normalize();
    }

    public void Move(IntVector3 offset)
    {
        blocks.ForEach(b => b.Move(offset));
    }
}
