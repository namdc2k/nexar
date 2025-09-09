using SO;
using UnityEngine;

public class FollowTarget : MonoBehaviour{
    [SerializeField] Transform player;
    private Quaternion _startLocalRotation;
    private Quaternion _eulerAnglesTarget;
    private ConfigTest1 _configTest1;
    private bool _isHaveTarget;

    private void Start() {
        _startLocalRotation = transform.localRotation;
        _configTest1 = SOExtensions.GetConfigTest1();
    }

    private void OnEnable() {
        GameEvents.TargetPoint += SetTarget;
    }

    private void OnDisable() {
        GameEvents.TargetPoint -= SetTarget;
    }

    private void SetTarget(Transform obj) {
        if (obj == null) {
            _isHaveTarget = false;
            _eulerAnglesTarget = _startLocalRotation;
            return;
        }

        _isHaveTarget = true;
        Vector3 dir = (obj.position - player.position);
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
        _eulerAnglesTarget = lookRot;
    }

    private void Update() {
        if (_isHaveTarget)
            transform.rotation = Quaternion.Lerp(transform.rotation, _eulerAnglesTarget,
                Time.deltaTime * _configTest1.RotateSmooth);
        else {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _eulerAnglesTarget,
                Time.deltaTime * _configTest1.RotateSmooth);
        }
    }
}