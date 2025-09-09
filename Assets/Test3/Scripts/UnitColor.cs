using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UnitColor : MonoBehaviour{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject textPrefab;

    private void OnValidate() {
        Transform text = transform.GetChild(0);
        Vector3 pos = text.localPosition;
        pos.z = -.1f;
        text.localPosition = pos;
        text.GetComponent<TextMeshPro>().text = transform.GetSiblingIndex().ToString();
    }
}