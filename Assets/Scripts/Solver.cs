using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour {

    PuzzlePieces[] puzzlePieces;

    // Use this for initialization
    void Start () {
        GetComponent<PuzzlePieces>().puzzlePieces.ToArray();

        StartCoroutine(Solve());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Solve()
    {
        yield return null;
    }
}
