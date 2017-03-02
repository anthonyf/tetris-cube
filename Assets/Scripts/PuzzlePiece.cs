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
    void Start() {
        MakePuzzlePiece(type);
    }

    private void Initialize(Color color, IntVector3[] blockPositions)
    {
        var minV = new IntVector3(4, 4, 4);
        var maxV = new IntVector3(0, 0, 0);
        foreach (var v in blockPositions)
        {
            minV = new IntVector3(Mathf.Min(minV.x, v.x), Mathf.Min(minV.y, v.y), Mathf.Min(minV.z, v.z));
            maxV = new IntVector3(Mathf.Max(maxV.x, v.x), Mathf.Max(maxV.y, v.y), Mathf.Max(maxV.z, v.z));
        }

        foreach (var v in blockPositions)
        {
            var b = Instantiate(blockPrefab);
            b.transform.SetParent(transform, false);
            b.SetPosition(new IntVector3(v.x - (maxV.x - minV.x) / 2, v.y - (maxV.y - minV.y) / 2, v.z - (maxV.z - minV.z) / 2));
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
                Initialize(Color.white, new IntVector3[4] {
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
            default:
                throw new NotImplementedException("Unsupported puzzle piece type");
        }
    }

    public void Rotate(Axis axis)
    {
        StartCoroutine(DoRotation(axis, .5f));
    }

    private List<IntVector3> BlockLocations {
        get
        {
            List<IntVector3> locations = new List<IntVector3>();
            foreach(var block in blocks)
            {
                var t = transform.parent.InverseTransformPoint(block.transform.position);
                locations.Add(new IntVector3(
                    (int)Math.Round(t.x), 
                    (int)Math.Round(t.y), 
                    (int)Math.Round(t.z)));
            }
            return locations;
        }
    }

    int doRotationCounter = 0;
    IEnumerator DoRotation(Axis axis, float speed)
    {
        while (doRotationCounter > 0) { yield return null; }
        doRotationCounter++;
        var startRotation = transform.localEulerAngles;
        var endRotation = new Vector3(
            startRotation.x + IntVector3.AxisEulerAngles[(int)axis].x,
            startRotation.y + IntVector3.AxisEulerAngles[(int)axis].y,
            startRotation.z + IntVector3.AxisEulerAngles[(int)axis].z);
        var startPosition = transform.localPosition;
        var elapsedTime = 0f;
        var time = speed;
        while (time > elapsedTime)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = endRotation;
        doRotationCounter--;
        foreach (var block in BlockLocations) {
            Debug.Log("block location x=" + block.x + " y=" + block.y + " z=" + block.z);
        }
    }

    public class PuzzlePiecePosition
    {
        public Vector3 position;
        public Vector3 eulerAngle;
        public List<IntVector3> blockPositions;
    }

    List<PuzzlePiecePosition> _cachedValidBoardPositions = null;

    public List<PuzzlePiecePosition> ValidBoardPositions()
    {
        if(_cachedValidBoardPositions != null)
        {
            return _cachedValidBoardPositions;
        }

        List<PuzzlePiecePosition> validPostions = new List<PuzzlePiecePosition>();
        PuzzleGrid grid = new PuzzleGrid();

        var savedLocalEulerAngles = transform.localEulerAngles;
        var savedLocalPosition = transform.localPosition;

        // for all 24 rotations rotation
        foreach (var rotation in TwentyFourRotations.rotations)
        {
            transform.localEulerAngles = rotation;
            // for each x position
            for (int x = -(4 - 1); x < (4 + (4 - 1)); x++)
            {
                // for each y position
                for (int y = -(4 - 1); y < (4 + (4 - 1)); y++)
                {
                    // for each z position
                    for (int z = -(4 - 1); z < (4 + (4 - 1)); z++)
                    {
                        // place piece
                        transform.localPosition = new Vector3(x, y, z);
                        var blocks = BlockLocations;
                        if (grid.IsValidBoardPosition(blocks.ToList()))
                        {
                            // eliminate duplicate orientations
                            var shouldAdd = true;
                            var blockSetA = new HashSet<IntVector3>(blocks);
                            foreach (var position in validPostions)
                            {
                                var blockSetB = new HashSet<IntVector3>(position.blockPositions);
                                if(blockSetA.SetEquals(blockSetB))
                                {
                                    shouldAdd = false;
                                    break;     
                                }
                            }
                            if(shouldAdd)
                            {
                                validPostions.Add(new PuzzlePiecePosition()
                                {
                                    eulerAngle = transform.localEulerAngles,
                                    position = transform.localPosition,
                                    blockPositions = blocks
                                });
                            }
                        }
                    }
                }
            }
        }
        transform.localPosition = savedLocalPosition;
        transform.localEulerAngles = savedLocalEulerAngles;

        _cachedValidBoardPositions = validPostions;
        return validPostions;
    }
}
