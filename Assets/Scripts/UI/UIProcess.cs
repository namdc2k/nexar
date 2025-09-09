using System;
using UnityEngine;
using UnityEngine.UI;

public class UIProcess : MonoBehaviour{
    [SerializeField] private Image processImage;

    private void OnEnable() {
        GameEvents.ProcessMove += SetProcess;
    }

    private void OnDisable() {
        GameEvents.ProcessMove -= SetProcess;
    }

    private void SetProcess(float obj) {
        processImage.fillAmount = obj;
    }
}