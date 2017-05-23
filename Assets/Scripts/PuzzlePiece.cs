using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Assertions;

public class PuzzlePiece : MonoBehaviour {
    [SerializeField]
    Block blockPrefab;

    public TetrisPuzzlePiece puzzlePiece;
    public Vector3 initialPosition;
    public Vector3 initialRotation;

    // Use this for initialization
    void Start() {
        Initialize(puzzlePiece);
    }

    private void Initialize(TetrisPuzzlePiece puzzlePiece)
    {
        var blockPositions = puzzlePiece.blocks.Select(b => b.position);

        foreach (var v in blockPositions)
        {
            var b = Instantiate(blockPrefab);
            b.transform.SetParent(transform, false);
            b.color = puzzlePiece.color;
            b.transform.localPosition = v.ToVector3();
        }
    }


    public void Rotate(Axis axis)
    {
        StartCoroutine(DoRotation(axis, .5f));
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
    }
}
