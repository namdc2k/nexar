using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ToolWithTest3 : MonoBehaviour{
    public string childNamePrefix = "obj_";
    

    [Button]
    public void LabelAll() {
        int count = 0;
        foreach (Transform child in transform) {
            if (!string.IsNullOrEmpty(childNamePrefix) &&
                !child.name.StartsWith(childNamePrefix, StringComparison.OrdinalIgnoreCase))
                continue;
            if (TryLabelOne(child.gameObject)) count++;
        }

        Debug.Log($"[ColorRegionLabeler] Labeled {count} region(s).");
    }

    public bool TryLabelOne(GameObject region) {
        if (region == null) return false;
        
        var mr = region.GetComponent<MeshRenderer>();
        var mf = region.GetComponent<MeshFilter>();
        if (mr == null || mf == null || mf.sharedMesh == null) return false;
        
        Transform textChild = region.transform.childCount > 0 ? region.transform.GetChild(0) : null;
        if (textChild == null) return false;
        
        if (!TryFindBestIncenter(mf.sharedMesh, region.transform.localToWorldMatrix, out Vector3 pos))
            return false;
        
        textChild.position = pos;
        textChild.rotation = Quaternion.identity;
        textChild.localScale = 0.1f * Vector3.one;
        return true;
    }

    bool TryFindBestIncenter(Mesh mesh, Matrix4x4 localToWorld, out Vector3 pos) {
        pos = Vector3.zero;
        var tmpCenter = mesh.bounds.center;
        var verts = mesh.vertices;
        var tris = mesh.triangles;
        if (tris == null || tris.Length < 3) return false;

        float xMin = -100000, xMax = 100000, yMin = -100000, yMax = 100000;
        for (int i = 0; i < tris.Length; i++) {
            Vector3 point = localToWorld.MultiplyPoint3x4(verts[tris[i]]);
            if (point.x > xMin && point.x < tmpCenter.x) {
                xMin = point.x;
            }
            if (point.x < xMax && point.x > tmpCenter.x) {
                xMax = point.x;
            }
            if (point.y > yMin && point.y < tmpCenter.y) {
                yMin = point.y;
            }
            if (point.y < yMax && point.y > tmpCenter.y) {
                yMax = point.y;
            }
        }
        pos.x = (xMin + xMax) / 2f;
        pos.y = (yMin + yMax) / 2f;
        return true;
    }
    
}