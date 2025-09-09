using System;
using SO;
using UnityEngine;

public class CubicBezierPath : MonoBehaviour{
    [Header("Control Points (P0..P3)")] public Transform P0, P1, P2, P3;

    [Header("Follower")] public Transform player;
    public bool follow = true;
    public bool loop = true;
    public bool constantSpeed = true;
    public bool faceForward = true;
    [Range(0f, 1f)] public float tManual = 0f;

    [Header("Gizmos")] [Range(4, 256)] public int gizmoSegments = 48;
    public Color curveColor = Color.yellow;
    public Color controlColor = new Color(1f, 1f, 1f, 0.6f);

    [SerializeField, HideInInspector] private float totalLength;
    [SerializeField, HideInInspector] private float[] lengthTable;
    [SerializeField, HideInInspector] private float[] tTable;
    [SerializeField, HideInInspector] private int tableSamples = 200;

    private float traveled = 0f; // quãng đường đã đi (m)
    private float tParam = 0f;   // tham số t (0..1)
    private ConfigTest1 _configTest1;
    private LineRenderer _lineRenderer;

    void OnEnable() {
        RebuildTable();
    }

    void OnValidate() {
        RebuildTable();
    }

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _configTest1 = SOExtensions.GetConfigTest1();
    }

    void Update() {
        if (!follow || player == null || !HasAllPoints()) return;

        if (constantSpeed) {
            // speed là m/s
            traveled += _configTest1.speedPlayer * Time.deltaTime;

            if (loop)
                tParam = DistanceToT(Mathf.Repeat(traveled, Mathf.Max(totalLength, 0.0001f)));
            else
                tParam = DistanceToT(Mathf.Min(traveled, totalLength));
        }
        else {
            tParam += _configTest1.speedPlayer * Time.deltaTime;
            tParam = loop ? Mathf.Repeat(tParam, 1f) : Mathf.Clamp01(tParam);
        }

        Vector3 pos = Evaluate(tParam);
        player.position = pos;

        if (faceForward) {
            Vector3 tangent = Derivative(tParam);
            if (tangent.sqrMagnitude > 1e-8f) {
                Quaternion look = Quaternion.LookRotation(tangent.normalized, Vector3.up);
                player.rotation = look;
            }
        }

        DrawLine();
    }

    void DrawLine() {
        int seg = Mathf.Max(2, _lineRenderer.positionCount - 1);
        Vector3 prev = Evaluate(0f);
        _lineRenderer.SetPosition(0, prev);
        for (int i = 1; i <= seg; i++) {
            float t = i / (float)seg;
            Vector3 curr = Evaluate(t);
            _lineRenderer.SetPosition(i, curr);
        }
    }

    private Vector3 Evaluate(float t) {
        Vector3 p0 = P0.position, p1 = P1.position, p2 = P2.position, p3 = P3.position;
        float u = 1f - t;
        float b0 = u * u * u;
        float b1 = 3f * u * u * t;
        float b2 = 3f * u * t * t;
        float b3 = t * t * t;
        return b0 * p0 + b1 * p1 + b2 * p2 + b3 * p3;
    }

    private Vector3 Derivative(float t) {
        Vector3 p0 = P0.position, p1 = P1.position, p2 = P2.position, p3 = P3.position;
        float u = 1f - t;
        return 3f * u * u * (p1 - p0) + 6f * u * t * (p2 - p1) + 3f * t * t * (p3 - p2);
    }

    void RebuildTable() {
        if (!HasAllPoints()) return;

        tableSamples = Mathf.Clamp(tableSamples, 20, 2000);
        tTable = new float[tableSamples + 1];
        lengthTable = new float[tableSamples + 1];

        Vector3 prev = Evaluate(0f);
        tTable[0] = 0f;
        lengthTable[0] = 0f;
        float acc = 0f;

        for (int i = 1; i <= tableSamples; i++) {
            float t = i / (float)tableSamples;
            tTable[i] = t;
            Vector3 curr = Evaluate(t);
            acc += Vector3.Distance(prev, curr);
            lengthTable[i] = acc;
            prev = curr;
        }

        totalLength = acc;
        traveled = Mathf.Clamp(traveled, 0f, Mathf.Max(totalLength, 0.0001f));
        tParam = Mathf.Clamp01(tParam);
    }

    float DistanceToT(float distance) {
        if (totalLength <= 1e-6f) return 0f;

        int hi = System.Array.BinarySearch(lengthTable, distance);
        if (hi >= 0) return tTable[hi];

        hi = ~hi;
        if (hi <= 0) return tTable[0];
        if (hi >= lengthTable.Length) return tTable[lengthTable.Length - 1];

        float l0 = lengthTable[hi - 1];
        float l1 = lengthTable[hi];
        float f = (distance - l0) / Mathf.Max(l1 - l0, 1e-6f);

        return Mathf.Lerp(tTable[hi - 1], tTable[hi], f);
    }

    bool HasAllPoints() => P0 && P1 && P2 && P3;

    // ---------- Gizmos ----------
    void OnDrawGizmos() {
        if (!HasAllPoints()) return;

        Gizmos.color = controlColor;
        Gizmos.DrawLine(P0.position, P1.position);
        Gizmos.DrawLine(P2.position, P3.position);
        Gizmos.color = curveColor;
        int seg = Mathf.Max(2, gizmoSegments);
        Vector3 prev = Evaluate(0f);
        for (int i = 1; i <= seg; i++) {
            float t = i / (float)seg;
            Vector3 curr = Evaluate(t);
            Gizmos.DrawLine(prev, curr);
            prev = curr;
        }

        Vector3 test = Evaluate(tManual);
        Gizmos.DrawSphere(test, 0.06f);
    }
}