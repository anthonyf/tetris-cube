using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    [SerializeField]
    MeshRenderer _meshRenderer;

    public IntVector3 position { get; set; }

    public Color color;

	// Use this for initialization
	void Start () {
        _meshRenderer.material.color = color;
        transform.localPosition = position.toVector3();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
