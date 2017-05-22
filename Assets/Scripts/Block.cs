using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    [SerializeField]
    MeshRenderer _meshRenderer;

    public Color color;

	// Use this for initialization
	void Start () {
        _meshRenderer.material.color = color;
	}
}
