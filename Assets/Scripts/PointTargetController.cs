using System;
using System.Collections;
using System.Collections.Generic;
using SO;
using UnityEngine;

public class PointTargetController : MonoBehaviour{
    [SerializeField] PointTarget[] targets;
    [SerializeField] private Transform player;

    private ConfigTest1 _configTest1;

    private void Start() {
        _configTest1 = SOExtensions.GetConfigTest1();
        targets = GetComponentsInChildren<PointTarget>();
    }

    private void Update() {
        if (player == null) return;
        CalculatorTarget();
    }

    private void CalculatorTarget() {
        var mx = Mathf.Infinity;
        PointTarget curr = null;
        for (int i = 0; i < targets.Length; i++) {
            targets[i].SetColor(Color.white);
            var dirDistance = (player.position - targets[i].transform.position).sqrMagnitude;
            if (dirDistance < _configTest1.distancePlayerToTarget * _configTest1.distancePlayerToTarget
                && dirDistance < mx) {
                mx = (player.position - targets[i].transform.position).sqrMagnitude;
                curr = targets[i];
            }
        }
        if (curr) curr.SetColor(Color.red);
        GameEvents.TargetPoint?.Invoke(curr == null ? null : curr.transform);
    }
}