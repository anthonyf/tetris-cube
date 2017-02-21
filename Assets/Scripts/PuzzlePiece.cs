using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzlePiece : MonoBehaviour {

    [SerializeField]
    Block blockPrefab;

    public const int NUM_OF_CELLS = 4;

    Block[] blocks = new Block[NUM_OF_CELLS];

	// Use this for initialization
	void Start () {
		
	}
	
    public void Initialize(Color color, IntVector3[] blockPositions)
    {
        var b = Instantiate(blockPrefab);
        b.transform.SetParent(transform, false);
        b.position = blockPositions[0];
        b.color = color;

        b = Instantiate(blockPrefab);
        b.transform.SetParent(transform, false);
        b.position = blockPositions[1];
        b.color = color;

        b = Instantiate(blockPrefab);
        b.transform.SetParent(transform, false);
        b.position = blockPositions[2];
        b.color = color;

        b = Instantiate(blockPrefab);
        b.transform.SetParent(transform, false);
        b.position = blockPositions[3];
        b.color = color;
    }

    void Rotate(int x, int y, int z)
    {
        transform.Rotate(new Vector3(x, y, z));
    }
}
