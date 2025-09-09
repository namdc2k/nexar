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
            if (dirDistance < mx && Mathf.Abs(GetAngleToTarget(targets[i].transform))< _configTest1.AngleLook) {
                mx = dirDistance;
                curr = targets[i];
            }
        }
        if (curr) curr.SetColor(Color.red);
        GameEvents.TargetPoint?.Invoke(curr == null ? null : curr.transform);
    }

    private float GetAngleToTarget(Transform target) {
        if (target == null) return 0f;
        Vector3 dirToTarget = (target.position - player.position).normalized;
        float angle = Vector3.SignedAngle(player.forward, dirToTarget, Vector3.up);
        return angle;
    }
}