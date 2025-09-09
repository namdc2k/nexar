using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTarget : MonoBehaviour{
    private new Renderer renderer;
    private Color _curColor = Color.white;

    private void Start() {
        renderer = GetComponent<Renderer>();
    }

    public void SetColor(Color color) {
        if (_curColor == color) return;
        _curColor = color;
        renderer.material.color = color;
    }
}