﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    [SerializeField]
    MeshRenderer _meshRenderer;

    public IntVector3 position { get; private set; }

    public Color color;

	// Use this for initialization
	void Start () {
        _meshRenderer.material.color = color;
        transform.localPosition = position.ToVector3();
	}

    public void SetPosition(IntVector3 position)
    {
        this.position = position;
        transform.localPosition = position.ToVector3();
    }
}
