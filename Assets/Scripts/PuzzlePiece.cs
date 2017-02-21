using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzlePiece : MonoBehaviour {

    [SerializeField]
    Block blockPrefab;

    List<Block> blocks = new List<Block>();

	// Use this for initialization
	void Start () {
		
	}
	
    public void Initialize(Color color, IntVector3[] blockPositions)
    {
        foreach(var v in blockPositions)
        {
            var b = Instantiate(blockPrefab);
            b.transform.SetParent(transform, false);
            b.position = v;
            b.color = color;
            blocks.Add(b);
        }
    }

    void Rotate(Axis axis)
    {
        
    }

    void Move(IntVector3 offset)
    {

    }
}
